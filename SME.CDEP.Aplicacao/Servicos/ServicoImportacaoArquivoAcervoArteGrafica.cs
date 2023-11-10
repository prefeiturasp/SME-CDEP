using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoImportacaoArquivoAcervoArteGrafica : ServicoImportacaoArquivoBase, IServicoImportacaoArquivoAcervoArteGrafica 
    {
        private readonly IServicoAcervoArteGrafica servicoAcervoArteGrafica;
        
        public ServicoImportacaoArquivoAcervoArteGrafica(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte,IServicoFormato servicoFormato,IServicoAcervoArteGrafica servicoAcervoArteGrafica)
            : base(repositorioImportacaoArquivo, servicoMaterial, servicoEditora,servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor,
                servicoConservacao,servicoAcessoDocumento,servicoCromia,servicoSuporte,servicoFormato)
        {
            this.servicoAcervoArteGrafica = servicoAcervoArteGrafica ?? throw new ArgumentNullException(nameof(servicoAcervoArteGrafica));
        }

        public void DefinirCreditosAutores(List<IdNomeTipoDTO> creditosAutores)
        {
            CreditosAutores = creditosAutores;
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoArteGraficaLinhaRetornoDTO>> ObterImportacaoPendente()
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterUltimaImportacao(TipoAcervo.ArtesGraficas);

            return ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoArteGraficaLinhaDTO>>(arquivoImportado.Conteudo));
        }

        public async Task<bool> RemoverLinhaDoArquivo(long id, int linhaDoArquivo)
        {
            await RemoverLinhaDoArquivo<AcervoArteGraficaLinhaDTO>(id, linhaDoArquivo, TipoAcervo.ArtesGraficas);

            return true;
        }
        
        public async Task<bool> AtualizarLinhaParaSucesso(long id, int linhaDoArquivo)
        {
            var conteudo = await ValidacoesImportacaoArquivo<AcervoArteGraficaLinhaDTO>(id, linhaDoArquivo, TipoAcervo.ArtesGraficas);
            
            var novoConteudo = conteudo.FirstOrDefault(w => w.NumeroLinha.SaoIguais(linhaDoArquivo));
            novoConteudo.DefinirLinhaComoSucesso();

            var status = conteudo.Any(a => a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso;
            
            await AtualizarImportacao(id,JsonConvert.SerializeObject(conteudo), status);

            return true;
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoArteGraficaLinhaRetornoDTO>> ImportarArquivo(IFormFile file)
        {
            ValidarArquivo(file);
        
            var acervosArtesGraficasLinhas = await LerPlanilha(file);

            var importacaoArquivo = ObterImportacaoArquivoParaSalvar(file.FileName, TipoAcervo.ArtesGraficas, JsonConvert.SerializeObject(acervosArtesGraficasLinhas));
            
            var importacaoArquivoId = await PersistirImportacao(importacaoArquivo);
           
            ValidarPreenchimentoValorFormatoQtdeCaracteres(acervosArtesGraficasLinhas);
            
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosArtesGraficasLinhas),ImportacaoStatus.ValidadoPreenchimentoValorFormatoQtdeCaracteres);
            
            await ValidacaoObterOuInserirDominios(acervosArtesGraficasLinhas);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosArtesGraficasLinhas),ImportacaoStatus.ValidacaoDominios);
            
            await PersistenciaAcervo(acervosArtesGraficasLinhas);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosArtesGraficasLinhas), acervosArtesGraficasLinhas.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);

            var arquivoImportado = await repositorioImportacaoArquivo.ObterPorId(importacaoArquivoId);

            return ObterRetornoImportacaoAcervo(arquivoImportado, acervosArtesGraficasLinhas);
        }

        private ImportacaoArquivoRetornoDTO<AcervoArteGraficaLinhaRetornoDTO> ObterRetornoImportacaoAcervo(ImportacaoArquivo arquivoImportado, IEnumerable<AcervoArteGraficaLinhaDTO> acervosArtesGraficasLinhas)
        {
            var acervoArteGraficaRetorno = new ImportacaoArquivoRetornoDTO<AcervoArteGraficaLinhaRetornoDTO>()
            {
                Id = arquivoImportado.Id,
                Nome = arquivoImportado.Nome,
                TipoAcervo = arquivoImportado.TipoAcervo,
                DataImportacao = arquivoImportado.CriadoEm,
                Erros = acervosArtesGraficasLinhas
                        .Where(w => w.PossuiErros)
                        .Select(ObterAcervoArteGraficaLinhaRetornoDto),
                Sucesso = acervosArtesGraficasLinhas
                        .Where(w => !w.PossuiErros)
                        .Select(ObterAcervoArteGraficaLinhaRetornoDto),
            };
            return acervoArteGraficaRetorno;
        }

        private static AcervoArteGraficaLinhaRetornoDTO ObterAcervoArteGraficaLinhaRetornoDto(AcervoArteGraficaLinhaDTO s)
        {
            return new AcervoArteGraficaLinhaRetornoDTO()
            {
                Titulo = ObterConteudoMensagemStatus(s.Titulo),
                Tombo = ObterConteudoMensagemStatus(s.Tombo),
                Credito = ObterConteudoMensagemStatus(s.Credito),
                Localizacao = ObterConteudoMensagemStatus(s.Localizacao),
                Procedencia = ObterConteudoMensagemStatus(s.Procedencia),
                Data = ObterConteudoMensagemStatus(s.Data),
                CopiaDigital = ObterConteudoMensagemStatus(s.CopiaDigital),
                AutorizacaoUsoDeImagem = ObterConteudoMensagemStatus(s.AutorizacaoUsoDeImagem),
                EstadoConservacao = ObterConteudoMensagemStatus(s.EstadoConservacao),
                Cromia = ObterConteudoMensagemStatus(s.Cromia),
                Largura = ObterConteudoMensagemStatus(s.Largura),
                Altura = ObterConteudoMensagemStatus(s.Altura),
                Diametro = ObterConteudoMensagemStatus(s.Diametro),
                Tecnica = ObterConteudoMensagemStatus(s.Tecnica),
                Suporte = ObterConteudoMensagemStatus(s.Suporte),
                Quantidade = ObterConteudoMensagemStatus(s.Quantidade),
                Descricao = ObterConteudoMensagemStatus(s.Descricao),
            };
        }

        public async Task PersistenciaAcervo(IEnumerable<AcervoArteGraficaLinhaDTO> acervosArtesGraficasLinhas)
        {
            foreach (var acervoArteGraficaLinha in acervosArtesGraficasLinhas.Where(w=> !w.PossuiErros))
            {
                try
                {
                    var acervoArteGrafica = new AcervoArteGraficaCadastroDTO()
                    {
                        Titulo = acervoArteGraficaLinha.Titulo.Conteudo,
                        Codigo = acervoArteGraficaLinha.Tombo.Conteudo,
                        CreditosAutoresIds = CreditosAutores
                            .Where(f => acervoArteGraficaLinha.Credito.Conteudo.FormatarTextoEmArray().Contains(f.Nome))
                            .Select(s => s.Id).ToArray(),
                        Localizacao = acervoArteGraficaLinha.Localizacao.Conteudo,
                        Procedencia = acervoArteGraficaLinha.Procedencia.Conteudo,
                        DataAcervo = acervoArteGraficaLinha.Data.Conteudo,
                        CopiaDigital = ObterCopiaDigitalPorValorDoCampo(acervoArteGraficaLinha.CopiaDigital.Conteudo),
                        PermiteUsoImagem = ObterAutorizaUsoDeImagemPorValorDoCampo(acervoArteGraficaLinha.AutorizacaoUsoDeImagem.Conteudo),
                        ConservacaoId = ObterConservacaoIdPorValorDoCampo(acervoArteGraficaLinha.EstadoConservacao.Conteudo),
                        CromiaId = ObterCromiaIdPorValorDoCampo(acervoArteGraficaLinha.Cromia.Conteudo),
                        Largura = acervoArteGraficaLinha.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo(),
                        Altura = acervoArteGraficaLinha.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo(),
                        Diametro = acervoArteGraficaLinha.Diametro.Conteudo.ObterDoubleOuNuloPorValorDoCampo(),
                        Tecnica = acervoArteGraficaLinha.Tecnica.Conteudo,
                        SuporteId = ObterSuporteImagemIdPorValorDoCampo(acervoArteGraficaLinha.Suporte.Conteudo),
                        Quantidade = acervoArteGraficaLinha.Quantidade.Conteudo.ObterLongoPorValorDoCampo(),
                        Descricao = acervoArteGraficaLinha.Descricao.Conteudo,
                    };
                    await servicoAcervoArteGrafica.Inserir(acervoArteGrafica);

                    acervoArteGraficaLinha.DefinirLinhaComoSucesso();
                }
                catch (Exception ex)
                {
                    acervoArteGraficaLinha.DefinirLinhaComoErro(ex.Message);
                }
            }
        } 

        public void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoArteGraficaLinhaDTO> linhas)
        {
            foreach (var linha in linhas)
            {
                try
                {
                    ValidarPreenchimentoLimiteCaracteres(linha.Titulo, Constantes.TITULO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Tombo, Constantes.TOMBO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Credito, Constantes.CREDITO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Localizacao,Constantes.LOCALIZACAO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Procedencia,Constantes.PROCEDENCIA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Data,Constantes.DATA);
                    ValidarPreenchimentoLimiteCaracteres(linha.CopiaDigital,Constantes.COPIA_DIGITAL);
                    ValidarPreenchimentoLimiteCaracteres(linha.AutorizacaoUsoDeImagem,Constantes.AUTORIZACAO_USO_DE_IMAGEM);
                    ValidarPreenchimentoLimiteCaracteres(linha.EstadoConservacao,Constantes.ESTADO_CONSERVACAO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Cromia,Constantes.CROMIA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Largura,Constantes.LARGURA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Altura,Constantes.ALTURA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Diametro,Constantes.DIAMETRO);
                    ValidarPreenchimentoLimiteCaracteres(linha.Tecnica,Constantes.TECNICA);
                    ValidarPreenchimentoLimiteCaracteres(linha.Suporte,Constantes.SUPORTE);
                    ValidarPreenchimentoLimiteCaracteres(linha.Quantidade,Constantes.QUANTIDADE);
                    ValidarPreenchimentoLimiteCaracteres(linha.Descricao,Constantes.DESCRICAO);
                    linha.PossuiErros = PossuiErro(linha);
                }
                catch (Exception e)
                {
                    linha.DefinirLinhaComoErro(string.Format(Constantes.OCORREU_UMA_FALHA_INESPERADA_NA_LINHA_X_MOTIVO_Y, linha.NumeroLinha, e.Message));
                }
            }
        }

        private bool PossuiErro(AcervoArteGraficaLinhaDTO linha)
        {
            return linha.Titulo.PossuiErro 
                   || linha.Tombo.PossuiErro 
                   || linha.Credito.PossuiErro
                   || linha.Localizacao.PossuiErro 
                   || linha.Procedencia.PossuiErro
                   || linha.Data.PossuiErro
                   || linha.CopiaDigital.PossuiErro 
                   || linha.AutorizacaoUsoDeImagem.PossuiErro 
                   || linha.EstadoConservacao.PossuiErro
                   || linha.Cromia.PossuiErro 
                   || linha.Largura.PossuiErro 
                   || linha.Altura.PossuiErro
                   || linha.Diametro.PossuiErro 
                   || linha.Tecnica.PossuiErro
                   || linha.Suporte.PossuiErro
                   || linha.Quantidade.PossuiErro
                   || linha.Descricao.PossuiErro;
        }

        public async Task ValidacaoObterOuInserirDominios(IEnumerable<AcervoArteGraficaLinhaDTO> linhas)
        {
            var linhasComsucesso = linhas.Where(w => !w.PossuiErros);

            try
            {
                await ValidarOuInserirCreditoAutoresCoAutoresTipoAutoria(linhasComsucesso.Select(s => s.Credito.Conteudo).ToArray().UnificarPipe().SplitPipe().Distinct(), TipoCreditoAutoria.Autoria);
                
                await ValidarOuInserirCromia(linhasComsucesso.Select(s => s.Cromia.Conteudo).Distinct());
                
                await ValidarOuInserirSuporte(linhasComsucesso.Select(s => s.Suporte.Conteudo).Distinct(), TipoSuporte.IMAGEM);
                
                await ValidarOuInserirConservacao(linhasComsucesso.Select(s => s.EstadoConservacao.Conteudo).Distinct());
                
            }
            catch (Exception e)
            {
                foreach (var linha in linhas)
                    linha.DefinirLinhaComoErro(string.Format(Constantes.OCORREU_UMA_FALHA_INESPERADA_NO_CADASTRO_DAS_REFERENCIAS_MOTIVO_X, e.Message));
            }
        }

        private async Task<IEnumerable<AcervoArteGraficaLinhaDTO>> LerPlanilha(IFormFile file)
        {
            var linhas = new List<AcervoArteGraficaLinhaDTO>();

            var stream = file.OpenReadStream();

            using (var package = new XLWorkbook(stream))
            {
                var planilha = package.Worksheets.FirstOrDefault();

                var totalLinhas = planilha.Rows().Count();

                for (int numeroLinha = Constantes.INICIO_LINHA_DADOS; numeroLinha <= totalLinhas; numeroLinha++)
                {
                    linhas.Add(new AcervoArteGraficaLinhaDTO()
                    {
                        NumeroLinha = numeroLinha,
                        Titulo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_TITULO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Tombo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_TOMBO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                            EhCampoObrigatorio = true
                        },
                        Credito = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_CREDITO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200,
                        },
                        Localizacao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_LOCALIZACAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_100,
                        },
                        Procedencia = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_PROCEDENCIA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200,
                        },
                        Data = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DATA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_50,
                            EhCampoObrigatorio = true
                        },
                        CopiaDigital = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_COPIA_DIGITAL),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_3, 
                            EhCampoObrigatorio = true
                        },
                        AutorizacaoUsoDeImagem = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_AUTORIZACAO_USO_DE_IMAGEM),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_3,
                            EhCampoObrigatorio = true
                        },
                        EstadoConservacao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_ESTADO_CONSERVACAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Cromia = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_CROMIA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Largura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DIMENSAO_LARGURA),
                            FormatoTipoDeCampo = Constantes.FORMATO_DOUBLE
                        },
                        Altura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DIMENSAO_ALTURA),
                            FormatoTipoDeCampo = Constantes.FORMATO_DOUBLE
                        },
                        Diametro = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DIMENSAO_DIAMETRO),
                            FormatoTipoDeCampo = Constantes.FORMATO_DOUBLE
                        },
                        Tecnica = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_TECNICA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_100,
                        },
                        Suporte = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_SUPORTE),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_500,
                            EhCampoObrigatorio = true
                        },
                        Quantidade = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_QUANTIDADE),
                            FormatoTipoDeCampo = Constantes.FORMATO_INTEIRO,
                            EhCampoObrigatorio = true
                        },
                        Descricao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_DESCRICAO),
                            EhCampoObrigatorio = true
                        }
                    });
                }
            }

            return linhas;
        }
    }
}