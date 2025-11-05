using System.Globalization;
using AutoMapper;
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
    public class ServicoImportacaoArquivoAcervoFotografico : ServicoImportacaoArquivoBase, IServicoImportacaoArquivoAcervoFotografico
    {
        private readonly IServicoMensageria servicoMensageria;
        private const int QTDE_CARECTERES_DATA_PARCIAL = 7;
        public ServicoImportacaoArquivoAcervoFotografico(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
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
            return await RemoverLinhaDoArquivo<AcervoFotograficoLinhaDTO>(id, linhaDoArquivo, TipoAcervo.Fotografico);
        }
        
        public async Task<bool> AtualizarLinhaParaSucesso(long id, LinhaDTO linhaDoArquivo)
        {
            var conteudo = await ValidacoesImportacaoArquivo<AcervoFotograficoLinhaDTO>(id, linhaDoArquivo.NumeroLinha, TipoAcervo.Fotografico);
            
            var novoConteudo = conteudo.FirstOrDefault(w => w.NumeroLinha.SaoIguais(linhaDoArquivo.NumeroLinha));
            novoConteudo.DefinirLinhaComoSucesso();

            var status = conteudo.Any(a => a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso;
            
            await AtualizarImportacao(id,JsonConvert.SerializeObject(conteudo), status);

            return true;
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoFotograficoDTO,AcervoFotograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPendente()
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterUltimaImportacao(TipoAcervo.Fotografico);
            
            if (arquivoImportado.EhNulo())
                return default;
        
            return await ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoFotograficoLinhaDTO>>(arquivoImportado.Conteudo), false);
        }
        
        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoFotograficoDTO,AcervoFotograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPorId(long id)
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterImportacaoPorId(id);
            
            if (arquivoImportado.EhNulo())
                return default;
        
            return await ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoFotograficoLinhaDTO>>(arquivoImportado.Conteudo), false);
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoFotograficoDTO,AcervoFotograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ImportarArquivo(IFormFile file)
        {
            ValidarArquivo(file);
        
            var acervosFotograficoLinhas = await LerPlanilha(file);
        
            var importacaoArquivo = ObterImportacaoArquivoParaSalvar(file.FileName, TipoAcervo.Fotografico, JsonConvert.SerializeObject(acervosFotograficoLinhas));
            
            var importacaoArquivoId = await PersistirImportacao(importacaoArquivo);
           
            await servicoMensageria.Publicar(RotasRabbit.ExecutarImportacaoArquivoAcervoFotografico, importacaoArquivoId, Guid.NewGuid());

            return new ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoFotograficoDTO,AcervoFotograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>()
            {
                Id = importacaoArquivoId,
                Nome = importacaoArquivo.Nome,
                TipoAcervo = importacaoArquivo.TipoAcervo,
                DataImportacao = DateTimeExtension.HorarioBrasilia(),
                Status = ImportacaoStatus.Pendente
            };
        }
        
        private async Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoFotograficoDTO,AcervoFotograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterRetornoImportacaoAcervo(ImportacaoArquivo arquivoImportado, IEnumerable<AcervoFotograficoLinhaDTO> acervosFotograficoLinhas, bool estaImportandoArquivo = true)
        {
            if (!estaImportandoArquivo)
                await CarregarTodosOsDominios();
                
            await ObterCreditosAutoresPorTipo(TipoCreditoAutoria.Credito);
                
            await ObterFormatosPorTipo(TipoFormato.ACERVO_FOTOS);
            
            await ObterSuportesPorTipo(TipoSuporte.IMAGEM);
            
            var acervoFotograficoRetorno = new ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoFotograficoDTO,AcervoFotograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>()
            {
                Id = arquivoImportado.Id,
                Nome = arquivoImportado.Nome,
                TipoAcervo = arquivoImportado.TipoAcervo,
                DataImportacao = arquivoImportado.CriadoEm,
                Status = arquivoImportado.Status,
                Erros = acervosFotograficoLinhas
                        .Where(w => w.PossuiErros)
                        .Select(s=> ObterAcervoLinhaRetornoResumidoDto(s,arquivoImportado.TipoAcervo)),
                Sucesso = acervosFotograficoLinhas
                        .Where(w => !w.PossuiErros)
                        .Select(s=> ObterLinhasComSucessoSufixo(s.Titulo.Conteudo, s.Codigo.Conteudo, s.NumeroLinha, Constantes.SIGLA_ACERVO_FOTOGRAFICO)),
            };
            return acervoFotograficoRetorno;
        }
        
        private AcervoLinhaErroDTO<AcervoFotograficoDTO,AcervoFotograficoLinhaRetornoDTO> ObterAcervoLinhaRetornoResumidoDto(AcervoFotograficoLinhaDTO linha, TipoAcervo tipoAcervo)
        {
            return new AcervoLinhaErroDTO<AcervoFotograficoDTO,AcervoFotograficoLinhaRetornoDTO>()
            {
                Titulo = ObterConteudoTexto(linha.Titulo),
                Tombo = ObterSufixo(ObterConteudoTexto(linha.Codigo),Constantes.SIGLA_ACERVO_FOTOGRAFICO),
                NumeroLinha = linha.NumeroLinha,
                RetornoObjeto = ObterAcervoFotograficoDto(linha,tipoAcervo),
                RetornoErro = ObterLinhasComErros(linha),
            };
        }
        
        private AcervoFotograficoDTO ObterAcervoFotograficoDto(AcervoFotograficoLinhaDTO linha, TipoAcervo tipoAcervo)
        {
            return new AcervoFotograficoDTO()
            {
                Titulo = ObterConteudoTexto(linha.Titulo),
                TipoAcervoId = (int)tipoAcervo,
                Codigo = ObterSufixo(ObterConteudoTexto(linha.Codigo),Constantes.SIGLA_ACERVO_FOTOGRAFICO),
                Localizacao = ObterConteudoTexto(linha.Localizacao),
                Procedencia = ObterConteudoTexto(linha.Procedencia),
                Ano = ObterConteudoTexto(linha.Ano),
                DataAcervo = ObterConteudoTexto(linha.Data),
                CopiaDigital = ObterConteudoSimNao(linha.CopiaDigital),
                PermiteUsoImagem = ObterConteudoSimNao(linha.PermiteUsoImagem),
                ConservacaoId = ObterConservacaoIdOuNuloPorValorDoCampo(linha.EstadoConservacao.Conteudo),
                Descricao = ObterConteudoTexto(linha.Descricao),
                Quantidade = ObterConteudoLongoOuNulo(linha.Quantidade),
                Largura = linha.Largura.Conteudo,
                Altura = linha.Altura.Conteudo,
                SuporteId = ObterSuporteImagemIdOuNuloPorValorDoCampo(linha.Suporte.Conteudo),
                FormatoId = ObterFormatoImagemIdOuNuloPorValorDoCampo(linha.FormatoImagem.Conteudo),
                CromiaId = ObterCromiaIdOuNuloPorValorDoCampo(linha.Cromia.Conteudo),
                Resolucao = ObterConteudoTexto(linha.Resolucao),
                TamanhoArquivo = ObterConteudoTexto(linha.TamanhoArquivo),
                CreditosAutoresIds = ObterCreditoAutoresIdsPorValorDoCampo(linha.Credito.Conteudo, TipoCreditoAutoria.Credito, false),
            };
        }
        
        private AcervoFotograficoLinhaRetornoDTO ObterLinhasComErros(AcervoFotograficoLinhaDTO s)
        {
            return new AcervoFotograficoLinhaRetornoDTO()
            {
                Titulo = ObterConteudoMensagemStatus(s.Titulo),
                Codigo = ObterConteudoMensagemStatus(s.Codigo),
                CreditosAutoresIds = ObterConteudoMensagemStatus(s.Credito),
                Localizacao = ObterConteudoMensagemStatus(s.Localizacao),
                Procedencia = ObterConteudoMensagemStatus(s.Procedencia),
                Ano = ObterConteudoMensagemStatus(s.Ano),
                DataAcervo = ObterConteudoMensagemStatus(s.Data),
                CopiaDigital = ObterConteudoMensagemStatus(s.CopiaDigital),
                PermiteUsoImagem = ObterConteudoMensagemStatus(s.PermiteUsoImagem),
                ConservacaoId = ObterConteudoMensagemStatus(s.EstadoConservacao),
                Descricao = ObterConteudoMensagemStatus(s.Descricao),
                Quantidade = ObterConteudoMensagemStatus(s.Quantidade),
                Largura = ObterConteudoMensagemStatus(s.Largura),
                Altura = ObterConteudoMensagemStatus(s.Altura),
                SuporteId = ObterConteudoMensagemStatus(s.Suporte),
                FormatoId = ObterConteudoMensagemStatus(s.FormatoImagem),
                TamanhoArquivo = ObterConteudoMensagemStatus(s.TamanhoArquivo),
                CromiaId = ObterConteudoMensagemStatus(s.Cromia),
                Resolucao = ObterConteudoMensagemStatus(s.Resolucao),
                NumeroLinha = s.NumeroLinha,
                Status = ImportacaoStatus.Erros,
                Mensagem = s.Mensagem,
                ErrosCampos = ObterMensagemErroLinha(s),
            };
        }
				
        private string[] ObterMensagemErroLinha(AcervoFotograficoLinhaDTO acervoFotograficoLinhaDTO)
        {
            var mensagemErro = new List<string>();

	        if (acervoFotograficoLinhaDTO.Titulo.PossuiErro)
		        mensagemErro.Add(acervoFotograficoLinhaDTO.Titulo.Mensagem);
	        
	        if (acervoFotograficoLinhaDTO.Codigo.PossuiErro)
		        mensagemErro.Add(acervoFotograficoLinhaDTO.Codigo.Mensagem);
	        
	        if (acervoFotograficoLinhaDTO.Credito.PossuiErro)
		        mensagemErro.Add(acervoFotograficoLinhaDTO.Credito.Mensagem);
			        
	        if (acervoFotograficoLinhaDTO.Localizacao.PossuiErro)
		        mensagemErro.Add(acervoFotograficoLinhaDTO.Localizacao.Mensagem);
			        
	        if (acervoFotograficoLinhaDTO.Procedencia.PossuiErro)
		        mensagemErro.Add(acervoFotograficoLinhaDTO.Procedencia.Mensagem);
			
            if (acervoFotograficoLinhaDTO.Ano.PossuiErro)
                mensagemErro.Add(acervoFotograficoLinhaDTO.Ano.Mensagem);
            
	        if (acervoFotograficoLinhaDTO.Data.PossuiErro)
		        mensagemErro.Add(acervoFotograficoLinhaDTO.Data.Mensagem);
			        
	        if (acervoFotograficoLinhaDTO.CopiaDigital.PossuiErro)
		        mensagemErro.Add(acervoFotograficoLinhaDTO.CopiaDigital.Mensagem);
			        
	        if (acervoFotograficoLinhaDTO.PermiteUsoImagem.PossuiErro)
		        mensagemErro.Add(acervoFotograficoLinhaDTO.PermiteUsoImagem.Mensagem);
			        
	        if (acervoFotograficoLinhaDTO.EstadoConservacao.PossuiErro)
		        mensagemErro.Add(acervoFotograficoLinhaDTO.EstadoConservacao.Mensagem);
	        
	        if (acervoFotograficoLinhaDTO.Descricao.PossuiErro)
		        mensagemErro.Add(acervoFotograficoLinhaDTO.Descricao.Mensagem);
	        
	        if (acervoFotograficoLinhaDTO.Suporte.PossuiErro)
		        mensagemErro.Add(acervoFotograficoLinhaDTO.Suporte.Mensagem);
	        
	        if (acervoFotograficoLinhaDTO.Quantidade.PossuiErro)
		        mensagemErro.Add(acervoFotograficoLinhaDTO.Quantidade.Mensagem);
			        
	        if (acervoFotograficoLinhaDTO.Cromia.PossuiErro)
		        mensagemErro.Add(acervoFotograficoLinhaDTO.Cromia.Mensagem);
			        
	        if (acervoFotograficoLinhaDTO.TamanhoArquivo.PossuiErro)
		        mensagemErro.Add(acervoFotograficoLinhaDTO.TamanhoArquivo.Mensagem);
			        
	        if (acervoFotograficoLinhaDTO.Largura.PossuiErro)
		        mensagemErro.Add(acervoFotograficoLinhaDTO.Largura.Mensagem);
			        
	        if (acervoFotograficoLinhaDTO.Altura.PossuiErro)
		        mensagemErro.Add(acervoFotograficoLinhaDTO.Altura.Mensagem);
            
            if (acervoFotograficoLinhaDTO.FormatoImagem.PossuiErro)
                mensagemErro.Add(acervoFotograficoLinhaDTO.FormatoImagem.Mensagem);
            
            if (acervoFotograficoLinhaDTO.Resolucao.PossuiErro)
                mensagemErro.Add(acervoFotograficoLinhaDTO.Resolucao.Mensagem);

            return mensagemErro.ToArray();
        }
        
        private async Task<IEnumerable<AcervoFotograficoLinhaDTO>> LerPlanilha(IFormFile file)
        {
            var linhas = new List<AcervoFotograficoLinhaDTO>();
        
            var stream = file.OpenReadStream();
        
            using (var package = new XLWorkbook(stream))
            {
                var planilha = package.Worksheets.FirstOrDefault();
        
                var totalLinhas = planilha.Rows().Count();
                
                await ValidarQtdeLinhasImportadas(totalLinhas);
                
                ValidarOrdemColunas(planilha, Constantes.INICIO_LINHA_TITULO);
        
                for (int numeroLinha = Constantes.INICIO_LINHA_DADOS; numeroLinha <= totalLinhas; numeroLinha++)
                {
                    linhas.Add(new AcervoFotograficoLinhaDTO()
                    {
                        NumeroLinha = numeroLinha,
                        Titulo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_TITULO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Codigo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_CODIGO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                            EhCampoObrigatorio = true
                        },
                        Credito = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_CREDITO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200,
                            EhCampoObrigatorio = true
                        },
                        Localizacao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_LOCALIZACAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_100,
                        },
                        Procedencia = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_PROCEDENCIA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200,
                            EhCampoObrigatorio = true
                        },
                        Ano = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_ANO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_7,
                            EhCampoObrigatorio = true,
                        },
                        Data = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = ObterData(planilha, numeroLinha),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_50,
                            EhCampoObrigatorio = true
                        },
                        CopiaDigital = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_COPIA_DIGITAL),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_3, 
                            ValoresPermitidos  = new List<string>() { Constantes.OPCAO_SIM, Constantes.OPCAO_NAO }
                        },
                        PermiteUsoImagem = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_AUTORIZACAO_USO_DE_IMAGEM),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_3,
                            ValoresPermitidos  = new List<string>() { Constantes.OPCAO_SIM, Constantes.OPCAO_NAO }
                        },
                        EstadoConservacao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_ESTADO_CONSERVACAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Descricao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_DESCRICAO),
                            EhCampoObrigatorio = true
                        },
                        Quantidade = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_QUANTIDADE),
                            FormatoTipoDeCampo = Constantes.FORMATO_INTEIRO,
                            EhCampoObrigatorio = true
                        },
                        Largura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_LARGURA).TratarLiteralComoDecimalComCasasDecimais(),
                            ValidarComExpressaoRegular = Constantes.PERMITIR_SOMENTE_NUMERAL_SEPARADO_POR_VIRGULA_DUAS_CASAS_DECIMAIS,
                            MensagemValidacao = string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.LARGURA)
                        },
                        Altura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_ALTURA).TratarLiteralComoDecimalComCasasDecimais(),
                            ValidarComExpressaoRegular = Constantes.PERMITIR_SOMENTE_NUMERAL_SEPARADO_POR_VIRGULA_DUAS_CASAS_DECIMAIS,
                            MensagemValidacao = string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.LARGURA)
                        },
                        Suporte = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_SUPORTE),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        FormatoImagem = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_FORMATO_IMAGEM),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        TamanhoArquivo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_TAMANHO_ARQUIVO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                            EhCampoObrigatorio = true
                        },
                        Cromia = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_CROMIA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Resolucao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_RESOLUCAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                            EhCampoObrigatorio = true
                        }
                    });
                }
            }

            return linhas;
        }

        private static string ObterData(IXLWorksheet planilha, int numeroLinha)
        {
            var campoData = planilha.Cell(numeroLinha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_DATA);

            var dataNaoTipada = campoData.Value.ToString().Trim();
            
            if (dataNaoTipada.NaoEstaPreenchido())
                return string.Empty;
            
            if (dataNaoTipada.Length <= QTDE_CARECTERES_DATA_PARCIAL)
                return dataNaoTipada;

            if (DateTime.TryParse(dataNaoTipada,out DateTime dataTipada))
                return dataTipada.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-BR"));

            throw new NegocioException(string.Format(MensagemNegocio.NAO_FOI_POSSIVEL_CONVERTER_A_DATA_ACERVO, dataNaoTipada));
        }

        private void ValidarOrdemColunas(IXLWorksheet planilha, int numeroLinha)
        {
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TITULO, 
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_TITULO, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TOMBO, 
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_CODIGO, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_CREDITO, 
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_CREDITO, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_LOCALIZACAO, 
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_LOCALIZACAO, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_PROCEDENCIA, 
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_PROCEDENCIA, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ANO, 
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_ANO, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DATA, 
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_DATA, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_COPIA_DIGITAL,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_COPIA_DIGITAL, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_AUTORIZACAO_USO_DE_IMAGEM,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_AUTORIZACAO_USO_DE_IMAGEM, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_ESTADO_DE_CONSERVACAO,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_ESTADO_CONSERVACAO, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DESCRICAO,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_DESCRICAO, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_QUANTIDADE,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_QUANTIDADE, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_LARGURA,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_LARGURA, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_DIMENSAO_ALTURA,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_ALTURA, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_SUPORTE,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_SUPORTE, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_FORMATO_DA_IMAGEM,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_FORMATO_IMAGEM, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_TAMANHO_DO_ARQUIVO,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_TAMANHO_ARQUIVO, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_CROMIA,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_CROMIA, Constantes.FOTOGRAFICO);
            
            ValidarTituloDaColuna(planilha, numeroLinha, Constantes.NOME_DA_COLUNA_RESOLUCAO,
                Constantes.ACERVO_FOTOGRAFICO_CAMPO_RESOLUCAO, Constantes.FOTOGRAFICO);
        }
    }
}