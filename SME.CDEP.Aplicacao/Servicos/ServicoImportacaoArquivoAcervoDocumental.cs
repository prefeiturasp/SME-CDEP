using AutoMapper;
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
    public class ServicoImportacaoArquivoAcervoDocumental : ServicoImportacaoArquivoBase, IServicoImportacaoArquivoAcervoDocumental 
    {
        private readonly IServicoAcervoDocumental servicoAcervoDocumental;
        
        public ServicoImportacaoArquivoAcervoDocumental(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma, IServicoAssunto servicoAssunto,
            IServicoCreditoAutor servicoCreditoAutor,IServicoConservacao servicoConservacao, IServicoAcessoDocumento servicoAcessoDocumento,
            IServicoCromia servicoCromia, IServicoSuporte servicoSuporte,IServicoFormato servicoFormato,IServicoAcervoDocumental servicoAcervoDocumental)
            : base(repositorioImportacaoArquivo, servicoMaterial, servicoEditora,servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor,
                servicoConservacao,servicoAcessoDocumento,servicoCromia,servicoSuporte,servicoFormato)
        {
            this.servicoAcervoDocumental = servicoAcervoDocumental ?? throw new ArgumentNullException(nameof(servicoAcervoDocumental));
        }

        public void DefinirCreditosAutores(List<IdNomeTipoDTO> creditosAutores)
        {
            CreditosAutores = creditosAutores;
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoDocumentalLinhaRetornoDTO>> ObterImportacaoPendente()
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterUltimaImportacao(TipoAcervo.DocumentacaoHistorica);

            return ObterRetornoImportacaoAcervo(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoDocumentalLinhaDTO>>(arquivoImportado.Conteudo));
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoDocumentalLinhaRetornoDTO>> ImportarArquivo(IFormFile file)
        {
            ValidarArquivo(file);
        
            var acervosDocumentalLinhas = await LerPlanilha(file);

            var importacaoArquivo = ObterImportacaoArquivoParaSalvar(file.FileName, TipoAcervo.DocumentacaoHistorica, JsonConvert.SerializeObject(acervosDocumentalLinhas));
            
            var importacaoArquivoId = await PersistirImportacao(importacaoArquivo);
           
            ValidarPreenchimentoValorFormatoQtdeCaracteres(acervosDocumentalLinhas);
            
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosDocumentalLinhas),ImportacaoStatus.ValidadoPreenchimentoValorFormatoQtdeCaracteres);
            
            await ValidacaoObterOuInserirDominios(acervosDocumentalLinhas);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosDocumentalLinhas),ImportacaoStatus.ValidacaoDominios);
            
            await PersistenciaAcervo(acervosDocumentalLinhas);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosDocumentalLinhas), acervosDocumentalLinhas.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);

            var arquivoImportado = await repositorioImportacaoArquivo.ObterPorId(importacaoArquivoId);

            return ObterRetornoImportacaoAcervo(arquivoImportado, acervosDocumentalLinhas);
        }

        private ImportacaoArquivoRetornoDTO<AcervoDocumentalLinhaRetornoDTO> ObterRetornoImportacaoAcervo(ImportacaoArquivo arquivoImportado, IEnumerable<AcervoDocumentalLinhaDTO> acervosDocumentalLinhas)
        {
            var acervoDocumentalRetorno = new ImportacaoArquivoRetornoDTO<AcervoDocumentalLinhaRetornoDTO>()
            {
                Id = arquivoImportado.Id,
                Nome = arquivoImportado.Nome,
                TipoAcervo = arquivoImportado.TipoAcervo,
                DataImportacao = arquivoImportado.CriadoEm,
                Erros = acervosDocumentalLinhas
                        .Where(w => w.PossuiErros)
                        .Select(ObterAcervoDocumentalLinhaRetornoDto),
                Sucesso = acervosDocumentalLinhas
                        .Where(w => !w.PossuiErros)
                        .Select(ObterAcervoDocumentalLinhaRetornoDto)
            };
            return acervoDocumentalRetorno;
        }

        private static AcervoDocumentalLinhaRetornoDTO ObterAcervoDocumentalLinhaRetornoDto(AcervoDocumentalLinhaDTO s)
        {
            return new AcervoDocumentalLinhaRetornoDTO()
            {
                Titulo = ObterConteudoMensagemStatus(s.Titulo),
                CodigoAntigo = ObterConteudoMensagemStatus(s.CodigoAntigo),
                CodigoNovo = ObterConteudoMensagemStatus(s.CodigoNovo),
                Material = ObterConteudoMensagemStatus(s.Material),
                Idioma = ObterConteudoMensagemStatus(s.Idioma),
                Autor = ObterConteudoMensagemStatus(s.Autor),
                Ano = ObterConteudoMensagemStatus(s.Ano),
                NumeroPaginas = ObterConteudoMensagemStatus(s.NumeroPaginas),
                Volume = ObterConteudoMensagemStatus(s.Volume),
                Descricao = ObterConteudoMensagemStatus(s.Descricao),
                TipoAnexo = ObterConteudoMensagemStatus(s.TipoAnexo),
                Largura = ObterConteudoMensagemStatus(s.Largura),
                Altura = ObterConteudoMensagemStatus(s.Altura),
                TamanhoArquivo = ObterConteudoMensagemStatus(s.TamanhoArquivo),
                AcessoDocumento = ObterConteudoMensagemStatus(s.AcessoDocumento),
                Localizacao = ObterConteudoMensagemStatus(s.Localizacao),
                CopiaDigital = ObterConteudoMensagemStatus(s.CopiaDigital),
                EstadoConservacao = ObterConteudoMensagemStatus(s.EstadoConservacao)
            };
        }

        public async Task PersistenciaAcervo(IEnumerable<AcervoDocumentalLinhaDTO> acervosDocumentalLinhas)
        {
            foreach (var acervoDocumentalLinha in acervosDocumentalLinhas.Where(w=> !w.PossuiErros))
            {
                try
                {
                    var acervoDocumental = new AcervoDocumentalCadastroDTO()
                    {
                        Titulo = acervoDocumentalLinha.Titulo.Conteudo,
                        Codigo = acervoDocumentalLinha.CodigoAntigo.Conteudo,
                        CodigoNovo = acervoDocumentalLinha.CodigoNovo.Conteudo,
                        MaterialId = ObterMaterialDocumentalIdPorValorDoCampo(acervoDocumentalLinha.Material.Conteudo),
                        IdiomaId = ObterIdiomaIdPorValorDoCampo(acervoDocumentalLinha.Idioma.Conteudo),
                        CreditosAutoresIds = CreditosAutores
                            .Where(f => acervoDocumentalLinha.Autor.Conteudo.FormatarTextoEmArray().Contains(f.Nome))
                            .Select(s => s.Id).ToArray(),
                        Ano = acervoDocumentalLinha.Ano.Conteudo,
                        NumeroPagina = acervoDocumentalLinha.NumeroPaginas.Conteudo,
                        Volume = acervoDocumentalLinha.Volume.Conteudo,
                        Descricao = acervoDocumentalLinha.Descricao.Conteudo,
                        TipoAnexo = acervoDocumentalLinha.TipoAnexo.Conteudo,
                        Largura = acervoDocumentalLinha.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo(),
                        Altura = acervoDocumentalLinha.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo(),
                        TamanhoArquivo = acervoDocumentalLinha.TamanhoArquivo.Conteudo,
                        AcessoDocumentosIds = CreditosAutores
                            .Where(f => acervoDocumentalLinha.AcessoDocumento.Conteudo.FormatarTextoEmArray().Contains(f.Nome))
                            .Select(s => s.Id).ToArray(),
                        Localizacao = acervoDocumentalLinha.Localizacao.Conteudo,
                        CopiaDigital = acervoDocumentalLinha.CopiaDigital.Conteudo.EhOpcaoSim(),
                        ConservacaoId = ObterConservacaoIdPorValorDoCampo(acervoDocumentalLinha.EstadoConservacao.Conteudo)
                    };
                    await servicoAcervoDocumental.Inserir(acervoDocumental);

                    acervoDocumentalLinha.Status = ImportacaoStatus.Sucesso;
                    acervoDocumentalLinha.Mensagem = string.Empty;
                    acervoDocumentalLinha.PossuiErros = false;
                }
                catch (Exception ex)
                {
                    acervoDocumentalLinha.PossuiErros = true;
                    acervoDocumentalLinha.Status = ImportacaoStatus.Erros;
                    acervoDocumentalLinha.Mensagem = ex.Message;
                }
            }
        } 

        public void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoDocumentalLinhaDTO> linhas)
        {
            foreach (var linha in linhas)
            {
                try
                {
                    ValidarPreenchimentoLimiteCaracteres(linha.Titulo, Constantes.TITULO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.CodigoAntigo, Constantes.CODIGO_ANTIGO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.CodigoNovo, Constantes.CODIGO_NOVO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Material,Constantes.MATERIAL, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Idioma,Constantes.IDIOMA, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Autor,Constantes.AUTOR, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Ano,Constantes.ANO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.NumeroPaginas,Constantes.NUMERO_PAGINAS, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Volume,Constantes.VOLUME, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Descricao,Constantes.DESCRICAO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.TipoAnexo,Constantes.TIPO_ANEXO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Largura,Constantes.LARGURA, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Altura,Constantes.ALTURA, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.TamanhoArquivo,Constantes.TAMANHO_ARQUIVO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.AcessoDocumento,Constantes.ACESSO_DOCUMENTO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Localizacao,Constantes.LOCALIZACAO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.CopiaDigital,Constantes.COPIA_DIGITAL, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.EstadoConservacao,Constantes.ESTADO_CONSERVACAO, linha.NumeroLinha);

                    if (linha.CodigoAntigo.Conteudo.NaoEstaPreenchido() && linha.CodigoNovo.Conteudo.NaoEstaPreenchido())
                    {
                        DefinirMensagemErro(linha.CodigoAntigo, Constantes.CAMPO_CODIGO_ANTIGO_OU_CODIGO_NOVO_DEVE_SER_PREENCHIDO, linha.NumeroLinha);
                        DefinirMensagemErro(linha.CodigoNovo, Constantes.CAMPO_CODIGO_ANTIGO_OU_CODIGO_NOVO_DEVE_SER_PREENCHIDO, linha.NumeroLinha);
                    }
                    linha.PossuiErros = PossuiErro(linha);
                }
                catch (Exception e)
                {
                    linha.PossuiErros = true;
                    linha.Status = ImportacaoStatus.Erros;
                    linha.Mensagem = string.Format(Constantes.OCORREU_UMA_FALHA_INESPERADA_NA_LINHA_X_MOTIVO_Y, linha.NumeroLinha, e.Message);
                }
            }
        }

        private bool PossuiErro(AcervoDocumentalLinhaDTO linha)
        {
            return linha.Titulo.PossuiErro 
                   || linha.CodigoAntigo.PossuiErro 
                   || linha.CodigoNovo.PossuiErro 
                   || linha.Material.PossuiErro
                   || linha.Idioma.PossuiErro
                   || linha.Autor.PossuiErro 
                   || linha.Ano.PossuiErro 
                   || linha.NumeroPaginas.PossuiErro
                   || linha.Volume.PossuiErro
                   || linha.Descricao.PossuiErro
                   || linha.TipoAnexo.PossuiErro
                   || linha.Largura.PossuiErro 
                   || linha.Altura.PossuiErro
                   || linha.TamanhoArquivo.PossuiErro 
                   || linha.AcessoDocumento.PossuiErro 
                   || linha.Localizacao.PossuiErro 
                   || linha.CopiaDigital.PossuiErro 
                   || linha.EstadoConservacao.PossuiErro;
        }

        public async Task ValidacaoObterOuInserirDominios(IEnumerable<AcervoDocumentalLinhaDTO> linhas)
        {
            var linhasComsucesso = linhas.Where(w => !w.PossuiErros);

            try
            {
                await ValidarOuInserirMateriais(linhasComsucesso.Select(s => s.Material.Conteudo).Distinct(), TipoMaterial.DOCUMENTAL);

                await ValidarOuInserirIdiomas(linhasComsucesso.Select(s => s.Idioma.Conteudo).Distinct());
                
                await ValidarOuInserirCreditoAutoresCoAutoresTipoAutoria(linhasComsucesso.Select(s => s.Autor.Conteudo).ToArray().UnificarPipe().SplitPipe().Distinct(), TipoCreditoAutoria.Autoria);
                
                await ValidarOuInserirAcessoDocumento(linhasComsucesso.Select(s => s.AcessoDocumento.Conteudo).ToArray().UnificarPipe().SplitPipe().Distinct());
                
                await ValidarOuInserirConservacao(linhasComsucesso.Select(s => s.EstadoConservacao.Conteudo).Distinct());
                
            }
            catch (Exception e)
            {
                foreach (var linha in linhas)
                {
                    linha.PossuiErros = true;
                    linha.Status = ImportacaoStatus.Erros;
                    linha.Mensagem = string.Format(Constantes.OCORREU_UMA_FALHA_INESPERADA_NO_CADASTRO_DAS_REFERENCIAS_MOTIVO_X, e.Message);
                }
            }
        }

        private async Task<IEnumerable<AcervoDocumentalLinhaDTO>> LerPlanilha(IFormFile file)
        {
            var linhas = new List<AcervoDocumentalLinhaDTO>();

            var stream = file.OpenReadStream();

            using (var package = new XLWorkbook(stream))
            {
                var planilha = package.Worksheets.FirstOrDefault();

                var totalLinhas = planilha.Rows().Count();

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
                        CodigoAntigo = new LinhaConteudoAjustarDTO()
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
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                        },
                        NumeroPaginas = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_NUMERO_PAGINAS),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_4,
                            EhCampoObrigatorio = true
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
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_DIMENSAO_LARGURA),
                            FormatoTipoDeCampo = Constantes.FORMATO_DOUBLE
                        },
                        Altura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_DIMENSAO_ALTURA),
                            FormatoTipoDeCampo = Constantes.FORMATO_DOUBLE
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
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_3, //aqui nas planilhas, não parece um campo booleano
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
    }
}