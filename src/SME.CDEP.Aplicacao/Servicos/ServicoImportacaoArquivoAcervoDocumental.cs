using AutoMapper;
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
    public class ServicoImportacaoArquivoAcervoDocumental : ServicoImportacaoArquivoBase, IServicoImportacaoArquivoAcervoDocumental 
    {
        private readonly IServicoMensageria servicoMensageria;
        
        public ServicoImportacaoArquivoAcervoDocumental(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte,IServicoFormato servicoFormato, 
            IMapper mapper,IRepositorioParametroSistema repositorioParametroSistema, IServicoMensageria servicoMensageria)
            : base(repositorioImportacaoArquivo, servicoMaterial, servicoEditora,servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor,
                servicoConservacao,servicoAcessoDocumento,servicoCromia,servicoSuporte,servicoFormato, mapper,repositorioParametroSistema)
        {
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
        }

        public async Task<bool> RemoverLinhaDoArquivo(long id, LinhaDTO linhaDoArquivo)
        {
            return await RemoverLinhaDoArquivo<AcervoDocumentalLinhaDTO>(id, linhaDoArquivo, TipoAcervo.DocumentacaoTextual);
        }
        
        public async Task<bool> AtualizarLinhaParaSucesso(long id, LinhaDTO linhaDoArquivo)
        {
            var conteudo = await ValidacoesImportacaoArquivo<AcervoDocumentalLinhaDTO>(id, linhaDoArquivo.NumeroLinha, TipoAcervo.DocumentacaoTextual);
            
            var novoConteudo = conteudo.FirstOrDefault(w => w.NumeroLinha.SaoIguais(linhaDoArquivo.NumeroLinha));
            novoConteudo.DefinirLinhaComoSucesso();

            var status = conteudo.Any(a => a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso;
            
            await AtualizarImportacao(id,JsonConvert.SerializeObject(conteudo), status);

            return true;
        }
        
        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoDocumentalDTO,AcervoDocumentalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPendente()
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterUltimaImportacao(TipoAcervo.DocumentacaoTextual);
            
            if (arquivoImportado.EhNulo())
                return default;

            return await ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoDocumentalLinhaDTO>>(arquivoImportado.Conteudo), false);
        }
        
        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoDocumentalDTO,AcervoDocumentalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPorId(long id)
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterImportacaoPorId(id);
            
            if (arquivoImportado.EhNulo())
                return default;

            return await ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoDocumentalLinhaDTO>>(arquivoImportado.Conteudo), false);
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoDocumentalDTO,AcervoDocumentalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ImportarArquivo(IFormFile file)
        {
            ValidarArquivo(file);
        
            var acervosDocumentalLinhas = await LerPlanilha(file);

            var importacaoArquivo = ObterImportacaoArquivoParaSalvar(file.FileName, TipoAcervo.DocumentacaoTextual, JsonConvert.SerializeObject(acervosDocumentalLinhas));
            
            var importacaoArquivoId = await PersistirImportacao(importacaoArquivo);
            
            await servicoMensageria.Publicar(RotasRabbit.ExecutarImportacaoArquivoAcervoDocumental, importacaoArquivoId, Guid.NewGuid());

            return new ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoDocumentalDTO,AcervoDocumentalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>()
            {
                Id = importacaoArquivoId,
                Nome = importacaoArquivo.Nome,
                TipoAcervo = importacaoArquivo.TipoAcervo,
                DataImportacao = DateTimeExtension.HorarioBrasilia(),
                Status = ImportacaoStatus.Pendente
            };
        }

        private async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoDocumentalDTO,AcervoDocumentalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterRetornoImportacaoAcervo(ImportacaoArquivo arquivoImportado, IEnumerable<AcervoDocumentalLinhaDTO> acervosDocumentalLinhas, bool estaImportandoArquivo = true)
        {
            if (!estaImportandoArquivo)
                await CarregarTodosOsDominios();
            
            await ObterCreditosAutoresPorTipo(TipoCreditoAutoria.Autoria);
                
            await ObterMateriaisPorTipo(TipoMaterial.DOCUMENTAL);
            
            var acervoDocumentalRetorno = new ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoDocumentalDTO,AcervoDocumentalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>()
            {
                Id = arquivoImportado.Id,
                Nome = arquivoImportado.Nome,
                TipoAcervo = arquivoImportado.TipoAcervo,
                DataImportacao = arquivoImportado.CriadoEm,
                Status = arquivoImportado.Status,
                Erros = acervosDocumentalLinhas
                        .Where(w => w.PossuiErros)
                        .Select(s=> ObterAcervoLinhaRetornoResumidoDto(s,arquivoImportado.TipoAcervo)),
                Sucesso = acervosDocumentalLinhas
                        .Where(w => !w.PossuiErros)
                        .Select(s=> ObterLinhasComSucesso(s.Titulo.Conteudo, ObterCodigo(s), s.NumeroLinha)),
            };
            return acervoDocumentalRetorno;
        }

        private string ObterCodigo(AcervoDocumentalLinhaDTO s)
        {
            if (s.Codigo.Conteudo.EstaPreenchido() && s.CodigoNovo.Conteudo.EstaPreenchido())
                return $"{s.Codigo.Conteudo}/{s.CodigoNovo.Conteudo}";
            
            if (s.Codigo.Conteudo.EstaPreenchido())
                return s.Codigo.Conteudo;
            
            return s.CodigoNovo.Conteudo.EstaPreenchido() ? s.CodigoNovo.Conteudo : default;
        }
        
        private AcervoLinhaErroDTO<AcervoDocumentalDTO,AcervoDocumentalLinhaRetornoDTO> ObterAcervoLinhaRetornoResumidoDto(AcervoDocumentalLinhaDTO linha, TipoAcervo tipoAcervo)
        {
            return new AcervoLinhaErroDTO<AcervoDocumentalDTO,AcervoDocumentalLinhaRetornoDTO>()
            {
                Titulo = ObterConteudoTexto(linha.Titulo),
                Tombo = ObterCodigo(linha),
                NumeroLinha = linha.NumeroLinha,
                RetornoObjeto = ObterAcervoDocumentalDto(linha,tipoAcervo),
                RetornoErro = ObterLinhasComErros(linha),
            };
        }
        
        private AcervoDocumentalDTO ObterAcervoDocumentalDto(AcervoDocumentalLinhaDTO linha, TipoAcervo tipoAcervo)
        {
            return new AcervoDocumentalDTO()
            {
                Titulo = ObterConteudoTexto(linha.Titulo),
                Codigo = ObterConteudoTexto(linha.Codigo),
                CodigoNovo = ObterConteudoTexto(linha.CodigoNovo),
                TipoAcervoId = (int)tipoAcervo,
                MaterialId = ObterMaterialDocumentalIdOuNuloPorValorDoCampo(linha.Material.Conteudo),
                IdiomaId = ObterIdiomaIdOuNuloPorValorDoCampo(linha.Idioma.Conteudo),
                Ano = ObterConteudoTexto(linha.Ano),
                NumeroPagina = ObterConteudoInteiroOuNulo(linha.NumeroPaginas),
                Volume = ObterConteudoTexto(linha.Volume),
                Descricao = ObterConteudoTexto(linha.Descricao),
                TipoAnexo = ObterConteudoTexto(linha.TipoAnexo),
                Largura = linha.Largura.Conteudo,
                Altura = linha.Altura.Conteudo,
                TamanhoArquivo = ObterConteudoTexto(linha.TamanhoArquivo),
                Localizacao = ObterConteudoTexto(linha.Localizacao),
                CopiaDigital = ObterConteudoSimNao(linha.CopiaDigital),
                ConservacaoId = ObterConservacaoIdOuNuloPorValorDoCampo(linha.EstadoConservacao.Conteudo),
                AcessoDocumentosIds = ObterAcessoDocumentosIdsPorValorDoCampo(linha.AcessoDocumento.Conteudo,false),
                CreditosAutoresIds = ObterCreditoAutoresIdsPorValorDoCampo(linha.Autor.Conteudo, TipoCreditoAutoria.Autoria, false),
            };
        }

        private AcervoDocumentalLinhaRetornoDTO ObterLinhasComErros(AcervoDocumentalLinhaDTO s)
        {
            return new AcervoDocumentalLinhaRetornoDTO()
            {
                Titulo = ObterConteudoMensagemStatus(s.Titulo),
                Codigo = ObterConteudoMensagemStatus(s.Codigo),
                CodigoNovo = ObterConteudoMensagemStatus(s.CodigoNovo),
                MaterialId = ObterConteudoMensagemStatus(s.Material),
                IdiomaId = ObterConteudoMensagemStatus(s.Idioma),
                CreditosAutoresIds = ObterConteudoMensagemStatus(s.Autor),
                Ano = ObterConteudoMensagemStatus(s.Ano),
                NumeroPagina = ObterConteudoMensagemStatus(s.NumeroPaginas),
                Volume = ObterConteudoMensagemStatus(s.Volume),
                Descricao = ObterConteudoMensagemStatus(s.Descricao),
                TipoAnexo = ObterConteudoMensagemStatus(s.TipoAnexo),
                Largura = ObterConteudoMensagemStatus(s.Largura),
                Altura = ObterConteudoMensagemStatus(s.Altura),
                TamanhoArquivo = ObterConteudoMensagemStatus(s.TamanhoArquivo),
                AcessoDocumentosIds = ObterConteudoMensagemStatus(s.AcessoDocumento),
                Localizacao = ObterConteudoMensagemStatus(s.Localizacao),
                CopiaDigital = ObterConteudoMensagemStatus(s.CopiaDigital),
                ConservacaoId = ObterConteudoMensagemStatus(s.EstadoConservacao),
                NumeroLinha = s.NumeroLinha,
                Status = ImportacaoStatus.Erros,
                Mensagem = s.Mensagem,
                ErrosCampos = ObterMensagemErroLinha(s),
            };
        }
        
        private string[] ObterMensagemErroLinha(AcervoDocumentalLinhaDTO acervoDocumentalLinhaDTO)
        {
            var mensagemErro = new List<string>();

	        if (acervoDocumentalLinhaDTO.Titulo.PossuiErro)
		        mensagemErro.Add(acervoDocumentalLinhaDTO.Titulo.Mensagem);
	        
	        if (acervoDocumentalLinhaDTO.Codigo.PossuiErro)
		        mensagemErro.Add(acervoDocumentalLinhaDTO.Codigo.Mensagem);
	        
	        if (acervoDocumentalLinhaDTO.CodigoNovo.PossuiErro)
		        mensagemErro.Add(acervoDocumentalLinhaDTO.CodigoNovo.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.Material.PossuiErro)
		        mensagemErro.Add(acervoDocumentalLinhaDTO.Material.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.Idioma.PossuiErro)
		        mensagemErro.Add(acervoDocumentalLinhaDTO.Idioma.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.Autor.PossuiErro)
		        mensagemErro.Add(acervoDocumentalLinhaDTO.Autor.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.Ano.PossuiErro)
		        mensagemErro.Add(acervoDocumentalLinhaDTO.Ano.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.NumeroPaginas.PossuiErro)
		        mensagemErro.Add(acervoDocumentalLinhaDTO.NumeroPaginas.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.Volume.PossuiErro)
		        mensagemErro.Add(acervoDocumentalLinhaDTO.Volume.Mensagem);
	        
	        if (acervoDocumentalLinhaDTO.Descricao.PossuiErro)
		        mensagemErro.Add(acervoDocumentalLinhaDTO.Descricao.Mensagem);
	        
	        if (acervoDocumentalLinhaDTO.TipoAnexo.PossuiErro)
		        mensagemErro.Add(acervoDocumentalLinhaDTO.TipoAnexo.Mensagem);
	        
	        if (acervoDocumentalLinhaDTO.Largura.PossuiErro)
		        mensagemErro.Add(acervoDocumentalLinhaDTO.Largura.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.Altura.PossuiErro)
		        mensagemErro.Add(acervoDocumentalLinhaDTO.Altura.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.TamanhoArquivo.PossuiErro)
		        mensagemErro.Add(acervoDocumentalLinhaDTO.TamanhoArquivo.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.AcessoDocumento.PossuiErro)
		        mensagemErro.Add(acervoDocumentalLinhaDTO.AcessoDocumento.Mensagem);
			        
	        if (acervoDocumentalLinhaDTO.Localizacao.PossuiErro)
		        mensagemErro.Add(acervoDocumentalLinhaDTO.Localizacao.Mensagem);
            
            if (acervoDocumentalLinhaDTO.CopiaDigital.PossuiErro)
                mensagemErro.Add(acervoDocumentalLinhaDTO.CopiaDigital.Mensagem);
            
            if (acervoDocumentalLinhaDTO.EstadoConservacao.PossuiErro)
                mensagemErro.Add(acervoDocumentalLinhaDTO.EstadoConservacao.Mensagem);

            return mensagemErro.ToArray();
        }

        private async Task<IEnumerable<AcervoDocumentalLinhaDTO>> LerPlanilha(IFormFile file)
        {
            var linhas = new List<AcervoDocumentalLinhaDTO>();

            var stream = file.OpenReadStream();

            using (var package = new XLWorkbook(stream))
            {
                var planilha = package.Worksheets.FirstOrDefault();

                var totalLinhas = planilha.Rows().Count();
                
                await ValidarQtdeLinhasImportadas(totalLinhas);
                
                ValidarOrdemColunas(planilha, Constantes.INICIO_LINHA_TITULO);

                for (int numeroLinha = Constantes.INICIO_LINHA_DADOS; numeroLinha <= totalLinhas; numeroLinha++)
                {
                    linhas.Add(new AcervoDocumentalLinhaDTO()
                    {
                        NumeroLinha = numeroLinha,
                        Titulo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_TITULO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Codigo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_CODIGO_ANTIGO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                        },
                        CodigoNovo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_CODIGO_NOVO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                        },
                        Material = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_MATERIAL),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                        },
                        Idioma = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_IDIOMA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Autor = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_AUTOR),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200,
                        },
                        Ano = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_ANO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_7,
                            EhCampoObrigatorio = true
                        },
                        NumeroPaginas = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_NUMERO_PAGINAS),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_4,
                            EhCampoObrigatorio = true,
                            FormatoTipoDeCampo = Constantes.FORMATO_INTEIRO
                        },
                        Volume = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_VOLUME),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                        },
                        Descricao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_DESCRICAO),
                            EhCampoObrigatorio = true
                        },
                        TipoAnexo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_TIPO_ANEXO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_50
                        },
                        Largura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_DIMENSAO_LARGURA).TratarLiteralComoDecimalComCasasDecimais(),
                            ValidarComExpressaoRegular = Constantes.PERMITIR_SOMENTE_NUMERAL_SEPARADO_POR_VIRGULA_DUAS_CASAS_DECIMAIS,
                            MensagemValidacao = string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.LARGURA)
                        },
                        Altura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_DIMENSAO_ALTURA).TratarLiteralComoDecimalComCasasDecimais(),
                            ValidarComExpressaoRegular = Constantes.PERMITIR_SOMENTE_NUMERAL_SEPARADO_POR_VIRGULA_DUAS_CASAS_DECIMAIS,
                            MensagemValidacao = string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.ALTURA)
                        },
                        TamanhoArquivo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_TAMANHO_ARQUIVO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15
                        },
                        AcessoDocumento = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_ACESSO_DOCUMENTO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Localizacao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_LOCALIZACAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_100,
                        },
                        CopiaDigital = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_COPIA_DIGITAL),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_3, 
                            ValoresPermitidos  = new List<string>() { Constantes.OPCAO_SIM, Constantes.OPCAO_NAO }
                        },
                        EstadoConservacao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_ESTADO_CONSERVACAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                        }
                    });
                }
            }

            return linhas;
        }
        
        private void ValidarOrdemColunas(IXLWorksheet planilha, int numeroLinha)
        {
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TITULO, 
                Constantes.ACERVO_DOCUMENTAL_CAMPO_TITULO, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_CODIGO_ANTIGO, 
                Constantes.ACERVO_DOCUMENTAL_CAMPO_CODIGO_ANTIGO, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_CODIGO_NOVO, 
                Constantes.ACERVO_DOCUMENTAL_CAMPO_CODIGO_NOVO, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_MATERIAL, 
                Constantes.ACERVO_DOCUMENTAL_CAMPO_MATERIAL, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_IDIOMA, 
                Constantes.ACERVO_DOCUMENTAL_CAMPO_IDIOMA, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_AUTOR, 
                Constantes.ACERVO_DOCUMENTAL_CAMPO_AUTOR, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ANO, 
                Constantes.ACERVO_DOCUMENTAL_CAMPO_ANO, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_NUMERO_PAGINAS,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_NUMERO_PAGINAS, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_VOLUME,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_VOLUME, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DESCRICAO,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_DESCRICAO, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TIPO_DE_ANEXO,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_TIPO_ANEXO, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_LARGURA,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_DIMENSAO_LARGURA, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_ALTURA,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_DIMENSAO_ALTURA, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TAMANHO_DO_ARQUIVO,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_TAMANHO_ARQUIVO, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ACESSO_DO_DOCUMENTO,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_ACESSO_DOCUMENTO, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_LOCALIZACAO,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_LOCALIZACAO, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_COPIA_DIGITAL,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_COPIA_DIGITAL, Constantes.DOCUMENTAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ESTADO_DE_CONSERVACAO,
                Constantes.ACERVO_DOCUMENTAL_CAMPO_ESTADO_CONSERVACAO, Constantes.DOCUMENTAL);
        }
    }
}