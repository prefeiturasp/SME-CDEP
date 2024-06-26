﻿using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoImportacaoArquivoAcervoBibliografico : ServicoImportacaoArquivoBase, IServicoImportacaoArquivoAcervoBibliografico 
    {
        private readonly IServicoMensageria servicoMensageria;

        public ServicoImportacaoArquivoAcervoBibliografico(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora, IServicoSerieColecao servicoSerieColecao, IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor, IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte, IServicoFormato servicoFormato, IMapper mapper,
            IRepositorioParametroSistema repositorioParametroSistema, IServicoMensageria servicoMensageria)
            : base(repositorioImportacaoArquivo, servicoMaterial, servicoEditora, servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor,
                servicoConservacao, servicoAcessoDocumento, servicoCromia, servicoSuporte, servicoFormato, mapper, repositorioParametroSistema)
        {
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
        }

        public void DefinirCoAutores(List<IdNomeTipoDTO> coAutores)
        {
            CoAutores = coAutores;
        }
        
        public async Task<bool> RemoverLinhaDoArquivo(long id, LinhaDTO linhaDoArquivo)
        {
            return await RemoverLinhaDoArquivo<AcervoBibliograficoLinhaDTO>(id, linhaDoArquivo, TipoAcervo.Bibliografico);
        }
        
        public async Task<bool> AtualizarLinhaParaSucesso(long id, LinhaDTO linhaDoArquivo)
        {
            var conteudo = await ValidacoesImportacaoArquivo<AcervoBibliograficoLinhaDTO>(id, linhaDoArquivo.NumeroLinha, TipoAcervo.Bibliografico);
            
            var novoConteudo = conteudo.FirstOrDefault(w => w.NumeroLinha.SaoIguais(linhaDoArquivo.NumeroLinha));
            novoConteudo.DefinirLinhaComoSucesso();

            var status = conteudo.Any(a => a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso;
            
            await AtualizarImportacao(id,JsonConvert.SerializeObject(conteudo), status);

            return true;
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoBibliograficoDTO,AcervoBibliograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPendente()
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterUltimaImportacao(TipoAcervo.Bibliografico);
            
            if (arquivoImportado.EhNulo())
                return default;

            return await ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoBibliograficoLinhaDTO>>(arquivoImportado.Conteudo), false);
        }
        
        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoBibliograficoDTO,AcervoBibliograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPorId(long id)
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterImportacaoPorId(id);
            
            if (arquivoImportado.EhNulo())
                return default;

            return await ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoBibliograficoLinhaDTO>>(arquivoImportado.Conteudo), false);
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoBibliograficoDTO,AcervoBibliograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ImportarArquivo(IFormFile file)
        {
            ValidarArquivo(file);
        
            var acervosBibliograficosLinhas = await LerPlanilha(file);

            var importacaoArquivo = ObterImportacaoArquivoParaSalvar(file.FileName, TipoAcervo.Bibliografico, JsonConvert.SerializeObject(acervosBibliograficosLinhas));
            
            var importacaoArquivoId = await PersistirImportacao(importacaoArquivo);
            
            await servicoMensageria.Publicar(RotasRabbit.ExecutarImportacaoArquivoAcervoBibliografico, importacaoArquivoId, Guid.NewGuid());

            return new ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoBibliograficoDTO, AcervoBibliograficoLinhaRetornoDTO>, AcervoLinhaRetornoSucessoDTO>()
            {
                Id = importacaoArquivoId,
                Nome = importacaoArquivo.Nome,
                TipoAcervo = importacaoArquivo.TipoAcervo,
                DataImportacao = DateTimeExtension.HorarioBrasilia(),
                Status = ImportacaoStatus.Pendente
            };
        }

        private async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoBibliograficoDTO,AcervoBibliograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterRetornoImportacaoAcervo(ImportacaoArquivo arquivoImportado, IEnumerable<AcervoBibliograficoLinhaDTO> acervosBibliograficosLinhas, bool estaImportandoArquivo = true)
        {
            if (!estaImportandoArquivo)
                await base.CarregarTodosOsDominios();
                
            await ObterMateriaisPorTipo(TipoMaterial.BIBLIOGRAFICO);
                
            await ObterCreditosAutoresPorTipo(TipoCreditoAutoria.Autoria);
            
            var acervoBibliograficoRetorno = new ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoBibliograficoDTO,AcervoBibliograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>()
            {
                Id = arquivoImportado.Id,
                Nome = arquivoImportado.Nome,
                TipoAcervo = arquivoImportado.TipoAcervo,
                DataImportacao = arquivoImportado.CriadoEm,
                Status = arquivoImportado.Status,
                Erros = acervosBibliograficosLinhas
                        .Where(w => w.PossuiErros)
                        .Select(s=> ObterAcervoLinhaRetornoResumidoDto(s,arquivoImportado.TipoAcervo)),
                Sucesso = acervosBibliograficosLinhas
                        .Where(w => !w.PossuiErros)
                        .Select(s=> ObterLinhasComSucesso(s.Titulo.Conteudo, s.Codigo.Conteudo, s.NumeroLinha)),
            };
            return acervoBibliograficoRetorno;
        }

        private AcervoLinhaErroDTO<AcervoBibliograficoDTO,AcervoBibliograficoLinhaRetornoDTO> ObterAcervoLinhaRetornoResumidoDto(AcervoBibliograficoLinhaDTO linha, TipoAcervo tipoAcervo)
        {
            return new AcervoLinhaErroDTO<AcervoBibliograficoDTO,AcervoBibliograficoLinhaRetornoDTO>()
            {
                Titulo = ObterConteudoTexto(linha.Titulo),
                Tombo = ObterConteudoTexto(linha.Codigo),
                NumeroLinha = linha.NumeroLinha,
                RetornoObjeto = ObterAcervoBibliograficoDto(linha,tipoAcervo),
                RetornoErro = ObterLinhasComErros(linha),
            };
        }
        
        private AcervoBibliograficoDTO ObterAcervoBibliograficoDto(AcervoBibliograficoLinhaDTO linha, TipoAcervo tipoAcervo)
        {
            return new AcervoBibliograficoDTO()
            {
                Titulo = ObterConteudoTexto(linha.Titulo),
                SubTitulo = ObterConteudoTexto(linha.SubTitulo),
                TipoAcervoId = (int)tipoAcervo,
                Codigo = ObterConteudoTexto(linha.Codigo),
                MaterialId = ObterMaterialBibliograficoIdOuNuloPorValorDoCampo(linha.Material.Conteudo),
                EditoraId = ObterEditoraIdOuNuloPorValorDoCampo(linha.Editora.Conteudo),
                AssuntosIds = ObterAssuntosIdsPorValorDoCampo(linha.Assunto.Conteudo, false),
                Ano = ObterConteudoTexto(linha.Ano),
                Edicao = ObterConteudoTexto(linha.Edicao),
                NumeroPagina = ObterConteudoInteiroOuNulo(linha.NumeroPaginas),
                Largura = linha.Largura.Conteudo,
                Altura = linha.Altura.Conteudo,
                SerieColecaoId = ObterSerieColecaoIdOuNuloPorValorDoCampo(linha.SerieColecao.Conteudo),
                IdiomaId = ObterIdiomaIdOuNuloPorValorDoCampo(linha.Idioma.Conteudo),
                Volume = ObterConteudoTexto(linha.Volume),
                LocalizacaoCDD = ObterConteudoTexto(linha.LocalizacaoCDD),
                LocalizacaoPHA = ObterConteudoTexto(linha.LocalizacaoPHA),
                NotasGerais = ObterConteudoTexto(linha.NotasGerais),
                Isbn = ObterConteudoTexto(linha.Isbn),
                CreditosAutoresIds = ObterCreditoAutoresIdsPorValorDoCampo(linha.Autor.Conteudo, TipoCreditoAutoria.Autoria, false),
                CoAutores = ObterCoAutoresTipoAutoria(linha.CoAutor.Conteudo, linha.TipoAutoria.Conteudo)
            };
        }
        
        private AcervoBibliograficoLinhaRetornoDTO ObterLinhasComErros(AcervoBibliograficoLinhaDTO s)
        {
            return new AcervoBibliograficoLinhaRetornoDTO()
            {
                Titulo = ObterConteudoMensagemStatus(s.Titulo),
                SubTitulo = ObterConteudoMensagemStatus(s.SubTitulo),
                MaterialId = ObterConteudoMensagemStatus(s.Material),
                CreditosAutoresIds = ObterConteudoMensagemStatus(s.Autor),
                CoAutores = ObterConteudoMensagemStatus(s.CoAutor),
                TipoAutoria = ObterConteudoMensagemStatus(s.TipoAutoria),
                EditoraId = ObterConteudoMensagemStatus(s.Editora),
                AssuntosIds = ObterConteudoMensagemStatus(s.Assunto),
                Ano = ObterConteudoMensagemStatus(s.Ano),
                Edicao = ObterConteudoMensagemStatus(s.Edicao),
                NumeroPagina = ObterConteudoMensagemStatus(s.NumeroPaginas),
                Altura = ObterConteudoMensagemStatus(s.Altura),
                Largura = ObterConteudoMensagemStatus(s.Largura),
                SerieColecaoId = ObterConteudoMensagemStatus(s.SerieColecao),
                Volume = ObterConteudoMensagemStatus(s.Volume),
                IdiomaId = ObterConteudoMensagemStatus(s.Idioma),
                LocalizacaoCDD = ObterConteudoMensagemStatus(s.LocalizacaoCDD),
                LocalizacaoPHA = ObterConteudoMensagemStatus(s.LocalizacaoPHA),
                NotasGerais = ObterConteudoMensagemStatus(s.NotasGerais),
                Isbn = ObterConteudoMensagemStatus(s.Isbn),
                Codigo = ObterConteudoMensagemStatus(s.Codigo),
                NumeroLinha = s.NumeroLinha,
                Status = ImportacaoStatus.Erros,
                Mensagem = s.Mensagem,
                ErrosCampos = ObterMensagemErroLinha(s),
            };
        }
        
        private string[] ObterMensagemErroLinha(AcervoBibliograficoLinhaDTO acervoBibliograficoLinhaDTO)
        {
            var mensagemErro = new List<string>();

	        if (acervoBibliograficoLinhaDTO.Titulo.PossuiErro)
		        mensagemErro.Add(acervoBibliograficoLinhaDTO.Titulo.Mensagem);
	        
	        if (acervoBibliograficoLinhaDTO.Codigo.PossuiErro)
		        mensagemErro.Add(acervoBibliograficoLinhaDTO.Codigo.Mensagem);
	        
	        if (acervoBibliograficoLinhaDTO.SubTitulo.PossuiErro)
		        mensagemErro.Add(acervoBibliograficoLinhaDTO.SubTitulo.Mensagem);
			        
	        if (acervoBibliograficoLinhaDTO.Material.PossuiErro)
		        mensagemErro.Add(acervoBibliograficoLinhaDTO.Material.Mensagem);
			        
	        if (acervoBibliograficoLinhaDTO.Autor.PossuiErro)
		        mensagemErro.Add(acervoBibliograficoLinhaDTO.Autor.Mensagem);
			        
	        if (acervoBibliograficoLinhaDTO.CoAutor.PossuiErro)
		        mensagemErro.Add(acervoBibliograficoLinhaDTO.CoAutor.Mensagem);
			        
	        if (acervoBibliograficoLinhaDTO.TipoAutoria.PossuiErro)
		        mensagemErro.Add(acervoBibliograficoLinhaDTO.TipoAutoria.Mensagem);
			        
	        if (acervoBibliograficoLinhaDTO.Editora.PossuiErro)
		        mensagemErro.Add(acervoBibliograficoLinhaDTO.Editora.Mensagem);
			        
	        if (acervoBibliograficoLinhaDTO.Edicao.PossuiErro)
		        mensagemErro.Add(acervoBibliograficoLinhaDTO.Edicao.Mensagem);
	        
	        if (acervoBibliograficoLinhaDTO.Assunto.PossuiErro)
		        mensagemErro.Add(acervoBibliograficoLinhaDTO.Assunto.Mensagem);
	        
	        if (acervoBibliograficoLinhaDTO.Ano.PossuiErro)
		        mensagemErro.Add(acervoBibliograficoLinhaDTO.Ano.Mensagem);
	        
	        if (acervoBibliograficoLinhaDTO.NumeroPaginas.PossuiErro)
		        mensagemErro.Add(acervoBibliograficoLinhaDTO.NumeroPaginas.Mensagem);
			        
	        if (acervoBibliograficoLinhaDTO.Largura.PossuiErro)
		        mensagemErro.Add(acervoBibliograficoLinhaDTO.Largura.Mensagem);
			        
	        if (acervoBibliograficoLinhaDTO.Altura.PossuiErro)
		        mensagemErro.Add(acervoBibliograficoLinhaDTO.Altura.Mensagem);
			        
	        if (acervoBibliograficoLinhaDTO.SerieColecao.PossuiErro)
		        mensagemErro.Add(acervoBibliograficoLinhaDTO.SerieColecao.Mensagem);
			        
	        if (acervoBibliograficoLinhaDTO.Volume.PossuiErro)
		        mensagemErro.Add(acervoBibliograficoLinhaDTO.Volume.Mensagem);
            
            if (acervoBibliograficoLinhaDTO.Idioma.PossuiErro)
                mensagemErro.Add(acervoBibliograficoLinhaDTO.Idioma.Mensagem);
            
            if (acervoBibliograficoLinhaDTO.LocalizacaoCDD.PossuiErro)
                mensagemErro.Add(acervoBibliograficoLinhaDTO.LocalizacaoCDD.Mensagem);

            if (acervoBibliograficoLinhaDTO.LocalizacaoPHA.PossuiErro)
                mensagemErro.Add(acervoBibliograficoLinhaDTO.LocalizacaoPHA.Mensagem);
            
            if (acervoBibliograficoLinhaDTO.NotasGerais.PossuiErro)
                mensagemErro.Add(acervoBibliograficoLinhaDTO.NotasGerais.Mensagem);
            
            if (acervoBibliograficoLinhaDTO.Isbn.PossuiErro)
                mensagemErro.Add(acervoBibliograficoLinhaDTO.Isbn.Mensagem);

            return mensagemErro.ToArray();
        }

        public CoAutorDTO[] ObterCoAutoresTipoAutoria(string coautores, string tiposAutoria)
        {
            if (coautores.NaoEstaPreenchido())
                return null;
            
            var coAutoresEmTexto = coautores.FormatarTextoEmArray();
            
            var coAutoresEmTextoAutoNumerados = coAutoresEmTexto.Select((coautoresEmTexto, indice) => new IdNomeDTO { Id = indice + 1, Nome = coautoresEmTexto }).ToList();

            var tiposAutoriaEmTextoAutoNumerados = Enumerable.Empty<IdNomeDTO>();

            if (tiposAutoria.EstaPreenchido())
            {
                var tiposAutoriaEmTexto = tiposAutoria.FormatarTextoEmArraySemDistinct();
            
                tiposAutoriaEmTextoAutoNumerados = tiposAutoriaEmTexto.Select((tipoAutoria, indice) => new IdNomeDTO() { Id = indice + 1, Nome = tipoAutoria });    
            }
            
            var coAutoresCompletos = coAutoresEmTextoAutoNumerados.Select(coAutorAutoNumerado => new CoAutorDTO
            {
                CreditoAutorId = CoAutores.FirstOrDefault(f=> f.Nome.RemoverAcentuacao().SaoIguais(coAutorAutoNumerado.Nome.RemoverAcentuacao()))?.Id,
                TipoAutoria = tiposAutoriaEmTextoAutoNumerados.FirstOrDefault(f => f.Id.SaoIguais(coAutorAutoNumerado.Id))?.Nome,
                CreditoAutorNome = CoAutores.FirstOrDefault(f=> f.Nome.RemoverAcentuacao().SaoIguais(coAutorAutoNumerado.Nome.RemoverAcentuacao()))?.Nome,
            }).ToArray();

            return coAutoresCompletos;
        }

        private async Task<IEnumerable<AcervoBibliograficoLinhaDTO>> LerPlanilha(IFormFile file)
        {
            var linhas = new List<AcervoBibliograficoLinhaDTO>();

            var stream = file.OpenReadStream();

            using (var package = new XLWorkbook(stream))
            {
                var planilha = package.Worksheets.FirstOrDefault();

                var totalLinhas = planilha.Rows().Count();
                
                await ValidarQtdeLinhasImportadas(totalLinhas);
                
                ValidarOrdemColunas(planilha, Constantes.INICIO_LINHA_TITULO);

                for (int numeroLinha = Constantes.INICIO_LINHA_DADOS; numeroLinha <= totalLinhas; numeroLinha++)
                {
                    linhas.Add(new AcervoBibliograficoLinhaDTO()
                    {
                        NumeroLinha = numeroLinha,
                        Titulo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_TITULO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        SubTitulo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_SUB_TITULO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                        },
                        Material = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_MATERIAL),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Autor = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_AUTOR),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200,
                            EhCampoObrigatorio = true
                        },
                        CoAutor = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_COAUTOR),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200
                        },
                        TipoAutoria = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_TIPO_DE_AUTORIA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_30
                        },
                        Editora = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_EDITORA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200
                        },
                        Assunto = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_ASSUNTO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_270,
                            EhCampoObrigatorio = true
                        },
                        Ano = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_ANO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_7,
                            EhCampoObrigatorio = true
                        },
                        Edicao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_EDICAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_30,
                        },
                        NumeroPaginas = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_NUMERO_PAGINAS),
                            FormatoTipoDeCampo = Constantes.FORMATO_INTEIRO
                        },
                        Altura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_DIMENSAO_ALTURA).TratarLiteralComoDecimalComCasasDecimais(),
                            ValidarComExpressaoRegular = Constantes.PERMITIR_SOMENTE_NUMERAL_SEPARADO_POR_VIRGULA_DUAS_CASAS_DECIMAIS,
                            MensagemValidacao = string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.ALTURA)
                        },
                        Largura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_DIMENSAO_LARGURA).TratarLiteralComoDecimalComCasasDecimais(),
                            ValidarComExpressaoRegular = Constantes.PERMITIR_SOMENTE_NUMERAL_SEPARADO_POR_VIRGULA_DUAS_CASAS_DECIMAIS,
                            MensagemValidacao = string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.LARGURA)
                        },
                        SerieColecao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_SERIE_COLECAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200,
                        },
                        Volume = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_VOLUME),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                        },
                        Idioma = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_IDIOMA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        LocalizacaoCDD = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_LOCALIZACAO_CDD),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_50,
                            EhCampoObrigatorio = true
                        },
                        LocalizacaoPHA = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_LOCALIZACAO_PHA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_50
                        },
                        NotasGerais = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_NOTAS_GERAIS),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                        },
                        Isbn = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_ISBN),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_50,
                        },
                        Codigo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_TOMBO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                            EhCampoObrigatorio = true
                        }
                    });
                }
            }

            return linhas;
        }
        
        private void ValidarOrdemColunas(IXLWorksheet planilha, int numeroLinha)
        {
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TITULO, 
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_TITULO, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_SUBTITULO, 
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_SUB_TITULO, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_MATERIAL, 
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_MATERIAL, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_AUTOR, 
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_AUTOR, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_COAUTOR, 
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_COAUTOR, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TIPO_DE_AUTORIA, 
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_TIPO_DE_AUTORIA, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_EDITORA,
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_EDITORA, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ASSUNTO,
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_ASSUNTO, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ANO,
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_ANO, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_EDICAO,
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_EDICAO, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_NUMERO_PAGINAS,
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_NUMERO_PAGINAS, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_ALTURA,
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_DIMENSAO_ALTURA, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_LARGURA,
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_DIMENSAO_LARGURA, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_SERIE_COLECAO,
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_SERIE_COLECAO, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_VOLUME,
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_VOLUME, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_IDIOMA,
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_IDIOMA, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_LOCALIZACAO_CDD,
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_LOCALIZACAO_CDD, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_LOCALIZACAO_PHA,
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_LOCALIZACAO_PHA, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_NOTAS_GERAIS,
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_NOTAS_GERAIS, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ISBN,
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_ISBN, Constantes.BIBLIOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TOMBO,
                Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_TOMBO, Constantes.BIBLIOGRAFICO);
        }
    }
}