﻿using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoImportacaoArquivoAcervoTridimensional : ServicoImportacaoArquivoBase, IServicoImportacaoArquivoAcervoTridimensional 
    {
        private readonly IServicoMensageria servicoMensageria;
        
        public ServicoImportacaoArquivoAcervoTridimensional(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte, IServicoFormato servicoFormato,IServicoMensageria servicoMensageria, 
            IMapper mapper,IRepositorioParametroSistema repositorioParametroSistema)
            : base(repositorioImportacaoArquivo, servicoMaterial, servicoEditora,servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor,
                servicoConservacao,servicoAcessoDocumento,servicoCromia,servicoSuporte,servicoFormato, mapper,repositorioParametroSistema)
        {
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
        }
        
        public async Task<bool> RemoverLinhaDoArquivo(long id, LinhaDTO linhaDoArquivo)
        {
            return await RemoverLinhaDoArquivo<AcervoTridimensionalLinhaDTO>(id, linhaDoArquivo, TipoAcervo.Tridimensional);
        }

        public async Task<bool> AtualizarLinhaParaSucesso(long id, LinhaDTO linhaDoArquivo)
        {
            var conteudo = await ValidacoesImportacaoArquivo<AcervoTridimensionalLinhaDTO>(id, linhaDoArquivo.NumeroLinha, TipoAcervo.Tridimensional);
            
            var novoConteudo = conteudo.FirstOrDefault(w => w.NumeroLinha.SaoIguais(linhaDoArquivo.NumeroLinha));
            novoConteudo.DefinirLinhaComoSucesso();

            var status = conteudo.Any(a => a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso;
            
            await AtualizarImportacao(id,JsonConvert.SerializeObject(conteudo), status);

            return true;
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoTridimensionalDTO,AcervoTridimensionalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPendente()
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterUltimaImportacao(TipoAcervo.Tridimensional);
        
            if (arquivoImportado.EhNulo())
                return default;
            
            return await ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoTridimensionalLinhaDTO>>(arquivoImportado.Conteudo), false);
        }
        
        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoTridimensionalDTO,AcervoTridimensionalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPorId(long id)
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterImportacaoPorId(id);
        
            if (arquivoImportado.EhNulo())
                return default;
            
            return await ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoTridimensionalLinhaDTO>>(arquivoImportado.Conteudo), false);
        }

        public async Task CarregarDominiosTridimensionais()
        {
            await CarregarTodosOsDominios();
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoTridimensionalDTO,AcervoTridimensionalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ImportarArquivo(IFormFile file)
        {
            ValidarArquivo(file);
        
            var acervosTridimensionalLinhas = await LerPlanilha(file);
        
            var importacaoArquivo = ObterImportacaoArquivoParaSalvar(file.FileName, TipoAcervo.Tridimensional, JsonConvert.SerializeObject(acervosTridimensionalLinhas));
            
           var importacaoArquivoId = await PersistirImportacao(importacaoArquivo);
           
           await servicoMensageria.Publicar(RotasRabbit.ExecutarImportacaoArquivoAcervoTridimensional, importacaoArquivoId, Guid.NewGuid());

            return new ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoTridimensionalDTO,AcervoTridimensionalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>()
            {
                Id = importacaoArquivoId,
                Nome = importacaoArquivo.Nome,
                TipoAcervo = importacaoArquivo.TipoAcervo,
                DataImportacao = DateTimeExtension.HorarioBrasilia(),
                Status = ImportacaoStatus.Pendente
            };
        }
        
        private async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoTridimensionalDTO,AcervoTridimensionalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterRetornoImportacaoAcervo(ImportacaoArquivo arquivoImportado, IEnumerable<AcervoTridimensionalLinhaDTO> acervosTridimensionalLinhas, bool estaImportandoArquivo = true)
        {
            await base.CarregarTodosOsDominios();
            
            var acervoTridimensionalRetorno = new ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoTridimensionalDTO,AcervoTridimensionalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>()
            {
                Id = arquivoImportado.Id,
                Nome = arquivoImportado.Nome,
                TipoAcervo = arquivoImportado.TipoAcervo,
                DataImportacao = arquivoImportado.CriadoEm,
                Status = arquivoImportado.Status,
                Erros = acervosTridimensionalLinhas
                        .Where(w => w.PossuiErros)
                        .Select(s=> ObterAcervoLinhaRetornoResumidoDto(s,arquivoImportado.TipoAcervo)),
                Sucesso = acervosTridimensionalLinhas
                        .Where(w => !w.PossuiErros)
                        .Select(s=> ObterLinhasComSucessoSufixo(s.Titulo.Conteudo, s.Codigo.Conteudo, s.NumeroLinha,Constantes.SIGLA_ACERVO_TRIDIMENSIONAL)),
            };
            return acervoTridimensionalRetorno;
        }
        
         private AcervoLinhaErroDTO<AcervoTridimensionalDTO,AcervoTridimensionalLinhaRetornoDTO> ObterAcervoLinhaRetornoResumidoDto(AcervoTridimensionalLinhaDTO linha, TipoAcervo tipoAcervo)
        {
            return new AcervoLinhaErroDTO<AcervoTridimensionalDTO,AcervoTridimensionalLinhaRetornoDTO>()
            {
                Titulo = ObterConteudoTexto(linha.Titulo),
                Tombo = ObterSufixo(ObterConteudoTexto(linha.Codigo),Constantes.SIGLA_ACERVO_TRIDIMENSIONAL),
                NumeroLinha = linha.NumeroLinha,
                RetornoObjeto = ObterAcervoTridimensionalDto(linha,tipoAcervo),
                RetornoErro = ObterLinhasComErros(linha),
            };
        }
        
        private AcervoTridimensionalDTO ObterAcervoTridimensionalDto(AcervoTridimensionalLinhaDTO linha, TipoAcervo tipoAcervo)
        {
            return new AcervoTridimensionalDTO()
            {
                Titulo = ObterConteudoTexto(linha.Titulo),
                TipoAcervoId = (int)tipoAcervo,
                Codigo = ObterSufixo(ObterConteudoTexto(linha.Codigo),Constantes.SIGLA_ACERVO_TRIDIMENSIONAL),
                Procedencia = ObterConteudoTexto(linha.Procedencia),
                Ano = ObterConteudoTexto(linha.Ano),
                ConservacaoId = ObterConservacaoIdOuNuloPorValorDoCampo(linha.EstadoConservacao.Conteudo),
                Quantidade = ObterConteudoLongoOuNulo(linha.Quantidade),
                Descricao = ObterConteudoTexto(linha.Descricao),
                Largura = linha.Largura.Conteudo,
                Altura = linha.Altura.Conteudo,
                Profundidade = linha.Profundidade.Conteudo,
                Diametro = linha.Diametro.Conteudo,
            };
        }

        private AcervoTridimensionalLinhaRetornoDTO ObterLinhasComErros(AcervoTridimensionalLinhaDTO s)
        {
            return new AcervoTridimensionalLinhaRetornoDTO()
            {
                Titulo = ObterConteudoMensagemStatus(s.Titulo),
                Codigo = ObterConteudoMensagemStatus(s.Codigo),
                Procedencia = ObterConteudoMensagemStatus(s.Procedencia),
                Ano = ObterConteudoMensagemStatus(s.Ano),
                ConservacaoId = ObterConteudoMensagemStatus(s.EstadoConservacao),
                Quantidade = ObterConteudoMensagemStatus(s.Quantidade),
                Descricao = ObterConteudoMensagemStatus(s.Descricao),
                Largura = ObterConteudoMensagemStatus(s.Largura),
                Altura = ObterConteudoMensagemStatus(s.Altura),
                Profundidade = ObterConteudoMensagemStatus(s.Profundidade),
                Diametro = ObterConteudoMensagemStatus(s.Diametro),
                NumeroLinha = s.NumeroLinha,
                Status = ImportacaoStatus.Erros,
                Mensagem = s.Mensagem,
                ErrosCampos = ObterMensagemErroLinha(s),
            };
        }
        
        private string[] ObterMensagemErroLinha(AcervoTridimensionalLinhaDTO acervoTridimensionalLinhaDTO)
        {
	        var mensagemErro = new List<string>();

	        if (acervoTridimensionalLinhaDTO.Titulo.PossuiErro)
		        mensagemErro.Add(acervoTridimensionalLinhaDTO.Titulo.Mensagem);
	        
	        if (acervoTridimensionalLinhaDTO.Codigo.PossuiErro)
		        mensagemErro.Add(acervoTridimensionalLinhaDTO.Codigo.Mensagem);
	        
	        if (acervoTridimensionalLinhaDTO.Procedencia.PossuiErro)
		        mensagemErro.Add(acervoTridimensionalLinhaDTO.Procedencia.Mensagem);
			
            if (acervoTridimensionalLinhaDTO.Ano.PossuiErro)
                mensagemErro.Add(acervoTridimensionalLinhaDTO.Ano.Mensagem);
            
	        if (acervoTridimensionalLinhaDTO.EstadoConservacao.PossuiErro)
		        mensagemErro.Add(acervoTridimensionalLinhaDTO.EstadoConservacao.Mensagem);
	        
	        if (acervoTridimensionalLinhaDTO.Descricao.PossuiErro)
		        mensagemErro.Add(acervoTridimensionalLinhaDTO.Descricao.Mensagem);
	        
	        if (acervoTridimensionalLinhaDTO.Quantidade.PossuiErro)
		        mensagemErro.Add(acervoTridimensionalLinhaDTO.Quantidade.Mensagem);
	        
	        if (acervoTridimensionalLinhaDTO.Largura.PossuiErro)
		        mensagemErro.Add(acervoTridimensionalLinhaDTO.Largura.Mensagem);
			        
	        if (acervoTridimensionalLinhaDTO.Altura.PossuiErro)
		        mensagemErro.Add(acervoTridimensionalLinhaDTO.Altura.Mensagem);
			        
	        if (acervoTridimensionalLinhaDTO.Profundidade.PossuiErro)
		        mensagemErro.Add(acervoTridimensionalLinhaDTO.Profundidade.Mensagem);
			        
	        if (acervoTridimensionalLinhaDTO.Diametro.PossuiErro)
		        mensagemErro.Add(acervoTridimensionalLinhaDTO.Diametro.Mensagem);
			        
            return mensagemErro.ToArray();
        }
        
        private async Task<IEnumerable<AcervoTridimensionalLinhaDTO>> LerPlanilha(IFormFile file)
        {
            var linhas = new List<AcervoTridimensionalLinhaDTO>();
        
            var stream = file.OpenReadStream();
        
            using (var package = new XLWorkbook(stream))
            {
                var planilha = package.Worksheets.FirstOrDefault();

                if (planilha == null)
                    throw new NegocioException(Constantes.NAO_FOI_POSSIVEL_LER_A_PLANILHA);
        
                var totalLinhas = planilha.Rows().Count();
                
                await ValidarQtdeLinhasImportadas(totalLinhas);
        
                ValidarOrdemColunas(planilha, Constantes.INICIO_LINHA_TITULO);
                
                for (int numeroLinha = Constantes.INICIO_LINHA_DADOS; numeroLinha <= totalLinhas; numeroLinha++)
                {
                    linhas.Add(new AcervoTridimensionalLinhaDTO()
                    {
                        NumeroLinha = numeroLinha,
                        Titulo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_TITULO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Codigo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_TOMBO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                            EhCampoObrigatorio = true
                        },
                        Procedencia = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_PROCEDENCIA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200,
                            EhCampoObrigatorio = true
                        },
                        Ano = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_ANO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_7,
                            EhCampoObrigatorio = true,
                        },
                        EstadoConservacao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_ESTADO_CONSERVACAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Quantidade = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_QUANTIDADE),
                            FormatoTipoDeCampo = Constantes.FORMATO_INTEIRO,
                            EhCampoObrigatorio = true
                        },
                        Descricao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_DESCRICAO),
                            EhCampoObrigatorio = true
                        },
                        Largura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_LARGURA).TratarLiteralComoDecimalComCasasDecimais(),
                            ValidarComExpressaoRegular = Constantes.PERMITIR_SOMENTE_NUMERAL_SEPARADO_POR_VIRGULA_DUAS_CASAS_DECIMAIS,
                            MensagemValidacao = string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.LARGURA)
                        },
                        Altura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_ALTURA).TratarLiteralComoDecimalComCasasDecimais(),
                            ValidarComExpressaoRegular = Constantes.PERMITIR_SOMENTE_NUMERAL_SEPARADO_POR_VIRGULA_DUAS_CASAS_DECIMAIS,
                            MensagemValidacao = string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.ALTURA)
                        },
                        Profundidade = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_PROFUNDIDADE).TratarLiteralComoDecimalComCasasDecimais(),
                            ValidarComExpressaoRegular = Constantes.PERMITIR_SOMENTE_NUMERAL_SEPARADO_POR_VIRGULA_DUAS_CASAS_DECIMAIS,
                            MensagemValidacao = string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.PROFUNDIDADE)
                        },
                        Diametro = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_DIAMETRO).TratarLiteralComoDecimalComCasasDecimais(),
                            ValidarComExpressaoRegular = Constantes.PERMITIR_SOMENTE_NUMERAL_SEPARADO_POR_VIRGULA_DUAS_CASAS_DECIMAIS,
                            MensagemValidacao = string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.DIAMETRO)
                        }
                    });
                }
            }

            return linhas;
        }

        private static void ValidarOrdemColunas(IXLWorksheet planilha, int numeroLinha)
        {
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TITULO, 
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_TITULO, Constantes.TRIDIMENSIONAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TOMBO, 
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_TOMBO, Constantes.TRIDIMENSIONAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_PROCEDENCIA, 
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_PROCEDENCIA, Constantes.TRIDIMENSIONAL);
           
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ANO, 
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_ANO, Constantes.TRIDIMENSIONAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ESTADO_DE_CONSERVACAO, 
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_ESTADO_CONSERVACAO, Constantes.TRIDIMENSIONAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_QUANTIDADE,
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_QUANTIDADE, Constantes.TRIDIMENSIONAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DESCRICAO,
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_DESCRICAO, Constantes.TRIDIMENSIONAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_LARGURA,
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_LARGURA, Constantes.TRIDIMENSIONAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_ALTURA,
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_ALTURA, Constantes.TRIDIMENSIONAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_PROFUNDIDADE,
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_PROFUNDIDADE, Constantes.TRIDIMENSIONAL);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_DIAMETRO,
                Constantes.ACERVO_TRIDIMENSIONAL_CAMPO_DIAMETRO, Constantes.TRIDIMENSIONAL);
        }
    }
}