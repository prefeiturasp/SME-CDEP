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
    public class ServicoImportacaoArquivoAcervoBibliografico : ServicoImportacaoArquivoBase, IServicoImportacaoArquivoAcervoBibliografico 
    {
        private readonly IServicoAcervoBibliografico servicoAcervoBibliografico;
        private readonly IMapper mapper;
        
        public ServicoImportacaoArquivoAcervoBibliografico(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IMapper mapper, IServicoMaterial servicoMaterial,
            IServicoEditora servicoEditora,IServicoSerieColecao servicoSerieColecao,IServicoIdioma servicoIdioma,
            IServicoAssunto servicoAssunto,IServicoCreditoAutor servicoCreditoAutor,
            IServicoAcervoBibliografico servicoAcervoBibliografico)
            : base(repositorioImportacaoArquivo, servicoMaterial, servicoEditora,servicoSerieColecao, servicoIdioma, servicoAssunto, servicoCreditoAutor)
        {
            this.servicoAcervoBibliografico = servicoAcervoBibliografico ?? throw new ArgumentNullException(nameof(servicoAcervoBibliografico));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void DefinirCreditosAutores(List<IdNomeTipoDTO> creditosAutores)
        {
            CreditosAutores = creditosAutores;
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoBibliograficoLinhaRetornoDTO>> ObterImportacaoPendente()
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterUltimaImportacao(TipoAcervo.Bibliografico);

            return ObterRetornoImportacaoAcervoBibliografico(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoBibliograficoLinhaDTO>>(arquivoImportado.Conteudo));
        }

        public async Task<ImportacaoArquivoRetornoDTO<AcervoBibliograficoLinhaRetornoDTO>> ImportarArquivo(IFormFile file)
        {
            ValidarArquivo(file);
        
            var acervosBibliograficosLinhas = await LerPlanilha(file);

            var importacaoArquivo = ObterImportacaoArquivoParaSalvar(file.FileName, TipoAcervo.Bibliografico, JsonConvert.SerializeObject(acervosBibliograficosLinhas));
            
            var importacaoArquivoId = await PersistirImportacao(importacaoArquivo);
           
            ValidarPreenchimentoValorFormatoQtdeCaracteres(acervosBibliograficosLinhas);
            
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosBibliograficosLinhas),ImportacaoStatus.ValidadoPreenchimentoValorFormatoQtdeCaracteres);
            
            await ValidacaoObterOuInserirDominios(acervosBibliograficosLinhas);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosBibliograficosLinhas),ImportacaoStatus.ValidacaoDominios);
            
            await PersistenciaAcervoBibliografico(acervosBibliograficosLinhas, importacaoArquivoId);

            var arquivoImportado = await repositorioImportacaoArquivo.ObterPorId(importacaoArquivoId);

            return ObterRetornoImportacaoAcervoBibliografico(arquivoImportado, acervosBibliograficosLinhas);
        }

        private ImportacaoArquivoRetornoDTO<AcervoBibliograficoLinhaRetornoDTO> ObterRetornoImportacaoAcervoBibliografico(ImportacaoArquivo arquivoImportado, IEnumerable<AcervoBibliograficoLinhaDTO> acervosBibliograficosLinhas)
        {
            var acervoBibliograficoRetorno = new ImportacaoArquivoRetornoDTO<AcervoBibliograficoLinhaRetornoDTO>()
            {
                Id = arquivoImportado.Id,
                Nome = arquivoImportado.Nome,
                TipoAcervo = arquivoImportado.TipoAcervo,
                DataImportacao = arquivoImportado.CriadoEm,
                Erros = acervosBibliograficosLinhas
                        .Where(w => w.PossuiErros)
                        .Select(s => new AcervoBibliograficoLinhaRetornoDTO()
                        {
                            Titulo = ObterConteudoMensagemStatus(s.Titulo),
                            SubTitulo = ObterConteudoMensagemStatus(s.SubTitulo),
                            Material = ObterConteudoMensagemStatus(s.Material),
                            Autor = ObterConteudoMensagemStatus(s.Autor),
                            CoAutor = ObterConteudoMensagemStatus(s.CoAutor),
                            TipoAutoria = ObterConteudoMensagemStatus(s.TipoAutoria),
                            Editora = ObterConteudoMensagemStatus(s.Editora),
                            Assunto = ObterConteudoMensagemStatus(s.Assunto),
                            Ano = ObterConteudoMensagemStatus(s.Ano),
                            Edicao = ObterConteudoMensagemStatus(s.Edicao),
                            NumeroPaginas = ObterConteudoMensagemStatus(s.NumeroPaginas),
                            Largura = ObterConteudoMensagemStatus(s.Largura),
                            Altura = ObterConteudoMensagemStatus(s.Altura),
                            SerieColecao = ObterConteudoMensagemStatus(s.SerieColecao),
                            Volume = ObterConteudoMensagemStatus(s.Volume),
                            Idioma = ObterConteudoMensagemStatus(s.Idioma),
                            LocalizacaoCDD = ObterConteudoMensagemStatus(s.LocalizacaoCDD),
                            LocalizacaoPHA = ObterConteudoMensagemStatus(s.LocalizacaoPHA),
                            NotasGerais = ObterConteudoMensagemStatus(s.NotasGerais),
                            Isbn = ObterConteudoMensagemStatus(s.Isbn),
                            Tombo = ObterConteudoMensagemStatus(s.Tombo),
                        }),
                Sucesso = acervosBibliograficosLinhas
                        .Where(w => !w.PossuiErros)
                        .Select(s => new AcervoBibliograficoLinhaRetornoDTO()
                        {
                            Titulo = ObterConteudoMensagemStatus(s.Titulo),
                            SubTitulo = ObterConteudoMensagemStatus(s.SubTitulo),
                            Material = ObterConteudoMensagemStatus(s.Material),
                            Autor = ObterConteudoMensagemStatus(s.Autor),
                            CoAutor = ObterConteudoMensagemStatus(s.CoAutor),
                            TipoAutoria = ObterConteudoMensagemStatus(s.TipoAutoria),
                            Editora = ObterConteudoMensagemStatus(s.Editora),
                            Assunto = ObterConteudoMensagemStatus(s.Assunto),
                            Ano = ObterConteudoMensagemStatus(s.Ano),
                            Edicao = ObterConteudoMensagemStatus(s.Edicao),
                            NumeroPaginas = ObterConteudoMensagemStatus(s.NumeroPaginas),
                            Largura = ObterConteudoMensagemStatus(s.Largura),
                            Altura = ObterConteudoMensagemStatus(s.Altura),
                            SerieColecao = ObterConteudoMensagemStatus(s.SerieColecao),
                            Volume = ObterConteudoMensagemStatus(s.Volume),
                            Idioma = ObterConteudoMensagemStatus(s.Idioma),
                            LocalizacaoCDD = ObterConteudoMensagemStatus(s.LocalizacaoCDD),
                            LocalizacaoPHA = ObterConteudoMensagemStatus(s.LocalizacaoPHA),
                            NotasGerais = ObterConteudoMensagemStatus(s.NotasGerais),
                            Isbn = ObterConteudoMensagemStatus(s.Isbn),
                            Tombo = ObterConteudoMensagemStatus(s.Tombo),
                        })
            };
            return acervoBibliograficoRetorno;
        }

        private static LinhaConteudoAjustarRetornoDTO ObterConteudoMensagemStatus(LinhaConteudoAjustarDTO linha)
        {
            return new LinhaConteudoAjustarRetornoDTO()
            {
                Conteudo = linha.Conteudo, 
                Validado = linha.PossuiErro, 
                Mensagem = linha.Mensagem
            };
        }

        public async Task PersistenciaAcervoBibliografico(IEnumerable<AcervoBibliograficoLinhaDTO> acervosBibliograficosLinhas, long importacaoArquivoId)
        {
            foreach (var acervoBibliograficoLinha in acervosBibliograficosLinhas.Where(w=> !w.PossuiErros))
            {
                try
                {
                    var acervoBibliografico = new AcervoBibliograficoCadastroDTO()
                    {
                        Titulo = acervoBibliograficoLinha.Titulo.Conteudo,

                        SubTitulo = acervoBibliograficoLinha.SubTitulo.Conteudo,

                        MaterialId =
                            Materiais.FirstOrDefault(f => f.Nome.Equals(acervoBibliograficoLinha.Material.Conteudo)).Id,

                        CreditosAutoresIds = CreditosAutores
                            .Where(f => acervoBibliograficoLinha.Autor.Conteudo.FormatarTextoEmArray().Contains(f.Nome))
                            .Select(s => s.Id).ToArray(),

                        CoAutores = ObterCoAutoresTipoAutoria(acervoBibliograficoLinha.CoAutor.Conteudo,acervoBibliograficoLinha.TipoAutoria.Conteudo),

                        EditoraId = acervoBibliograficoLinha.Editora.Conteudo.EstaPreenchido()
                            ? Editoras.FirstOrDefault(f => f.Nome.Equals(acervoBibliograficoLinha.Editora.Conteudo)).Id
                            : null,

                        AssuntosIds = Assuntos
                            .Where(f => acervoBibliograficoLinha.Assunto.Conteudo.FormatarTextoEmArray().Contains(f.Nome))
                            .Select(s => s.Id).ToArray(),

                        Ano = acervoBibliograficoLinha.Ano.Conteudo,
                        Edicao = acervoBibliograficoLinha.Edicao.Conteudo,
                        NumeroPagina = double.Parse(acervoBibliograficoLinha.NumeroPaginas.Conteudo),
                        Largura = double.Parse(acervoBibliograficoLinha.Largura.Conteudo),
                        Altura = double.Parse(acervoBibliograficoLinha.Altura.Conteudo),

                        SerieColecaoId = acervoBibliograficoLinha.SerieColecao.Conteudo.EstaPreenchido()
                            ? SeriesColecoes.FirstOrDefault(f => f.Nome.Equals(acervoBibliograficoLinha.SerieColecao.Conteudo))
                                .Id
                            : null,

                        Volume = acervoBibliograficoLinha.Volume.Conteudo,

                        IdiomaId = Idiomas.FirstOrDefault(f => f.Nome.Equals(acervoBibliograficoLinha.Idioma.Conteudo)).Id,

                        LocalizacaoCDD = acervoBibliograficoLinha.LocalizacaoCDD.Conteudo,
                        LocalizacaoPHA = acervoBibliograficoLinha.LocalizacaoPHA.Conteudo,
                        NotasGerais = acervoBibliograficoLinha.NotasGerais.Conteudo,
                        Isbn = acervoBibliograficoLinha.Isbn.Conteudo,
                        Codigo = acervoBibliograficoLinha.Tombo.Conteudo
                    };
                    await servicoAcervoBibliografico.Inserir(acervoBibliografico);

                    acervoBibliograficoLinha.Status = ImportacaoStatus.Sucesso;
                    acervoBibliograficoLinha.Mensagem = string.Empty;
                    acervoBibliograficoLinha.PossuiErros = false;
                }
                catch (Exception ex)
                {
                    acervoBibliograficoLinha.PossuiErros = true;
                    acervoBibliograficoLinha.Status = ImportacaoStatus.Erros;
                    acervoBibliograficoLinha.Mensagem = ex.Message;
                }
            }
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
                var tiposAutoriaEmTexto = tiposAutoria.FormatarTextoEmArray();
            
                tiposAutoriaEmTextoAutoNumerados = tiposAutoriaEmTexto.Select((tipoAutoria, indice) => new IdNomeDTO() { Id = indice + 1, Nome = tipoAutoria });    
            }
            
            var coAutoresCompletos = coAutoresEmTextoAutoNumerados.Select(coAutorAutoNumerado => new CoAutorDTO
            {
                CreditoAutorId = CreditosAutores.FirstOrDefault(f=> f.Nome.Equals(coAutorAutoNumerado.Nome)).Id,
                TipoAutoria = tiposAutoriaEmTextoAutoNumerados.FirstOrDefault(f => f.Id == coAutorAutoNumerado.Id)?.Nome
            }).ToArray();

            return coAutoresCompletos;
        }

        public void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoBibliograficoLinhaDTO> linhas)
        {
            foreach (var linha in linhas)
            {
                try
                {
                    ValidarPreenchimentoLimiteCaracteres(linha.Titulo, Constantes.TITULO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.SubTitulo, Constantes.SUB_TITULO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Material,Constantes.MATERIAL, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Autor,Constantes.AUTOR, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.CoAutor,Constantes.CO_AUTOR, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.TipoAutoria,Constantes.TIPO_AUTORIA, linha.NumeroLinha);
                    
                    if (linha.TipoAutoria.Conteudo.EstaPreenchido() && linha.CoAutor.Conteudo.NaoEstaPreenchido())
                        DefinirMensagemErro(linha.TipoAutoria, Constantes.CAMPO_COAUTOR_SEM_PREENCHIMENTO_E_TIPO_AUTORIA_PREENCHIDO, linha.NumeroLinha);

                    if (linha.TipoAutoria.Conteudo.SplitPipe().Count() > linha.CoAutor.Conteudo.SplitPipe().Count())
                        DefinirMensagemErro(linha.TipoAutoria, Constantes.TEMOS_MAIS_TIPO_AUTORIA_QUE_COAUTORES, linha.NumeroLinha);
                    
                    ValidarPreenchimentoLimiteCaracteres(linha.Editora,Constantes.EDITORA, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Assunto,Constantes.ASSUNTO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Ano,Constantes.ANO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Edicao,Constantes.EDICAO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.NumeroPaginas,Constantes.NUMERO_PAGINAS, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Largura,Constantes.LARGURA, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Altura,Constantes.ALTURA, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.SerieColecao,Constantes.SERIE_COLECAO, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Volume,Constantes.VOLUME, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Idioma,Constantes.IDIOMA, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.LocalizacaoCDD,Constantes.LOCALIZACAO_CDD, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.LocalizacaoPHA,Constantes.LOCALIZACAO_PHA, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.NotasGerais,Constantes.NOTAS_GERAIS, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Isbn,Constantes.ISBN, linha.NumeroLinha);
                    ValidarPreenchimentoLimiteCaracteres(linha.Tombo,Constantes.TOMBO, linha.NumeroLinha);
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

        private bool PossuiErro(AcervoBibliograficoLinhaDTO linha)
        {
            return linha.Titulo.PossuiErro 
                   || linha.SubTitulo.PossuiErro 
                   || linha.Material.PossuiErro 
                   || linha.Autor.PossuiErro 
                   || linha.CoAutor.PossuiErro 
                   || linha.TipoAutoria.PossuiErro 
                   || linha.Editora.PossuiErro 
                   || linha.Edicao.PossuiErro 
                   || linha.Assunto.PossuiErro 
                   || linha.Ano.PossuiErro 
                   || linha.NumeroPaginas.PossuiErro
                   || linha.Largura.PossuiErro 
                   || linha.Altura.PossuiErro 
                   || linha.SerieColecao.PossuiErro 
                   || linha.Volume.PossuiErro 
                   || linha.Idioma.PossuiErro
                   || linha.LocalizacaoCDD.PossuiErro
                   || linha.LocalizacaoPHA.PossuiErro
                   || linha.NotasGerais.PossuiErro
                   || linha.Isbn.PossuiErro
                   || linha.Tombo.PossuiErro;
        }

        public async Task ValidacaoObterOuInserirDominios(IEnumerable<AcervoBibliograficoLinhaDTO> linhas)
        {
            var linhasComsucesso = linhas.Where(w => !w.PossuiErros);

            try
            {
                await ValidarOuInserirMateriais(linhasComsucesso.Select(s => s.Material.Conteudo).Distinct(), TipoMaterial.BIBLIOGRAFICO);

                await ValidarOuInserirEditoras(linhasComsucesso.Select(s => s.Editora.Conteudo).Distinct());

                await ValidarOuInserirSeriesColecoes(linhasComsucesso.Select(s => s.SerieColecao.Conteudo).Distinct());

                await ValidarOuInserirIdiomas(linhasComsucesso.Select(s => s.Idioma.Conteudo).Distinct());

                await ValidarOuInserirAssuntos(linhasComsucesso.Select(s => s.Assunto.Conteudo).ToArray().UnificarPipe().SplitPipe().Distinct());

                await ValidarOuInserirCreditoAutoresCoAutores(linhasComsucesso.Select(s => s.Autor.Conteudo).ToArray().UnificarPipe().SplitPipe().Distinct(), TipoCreditoAutoria.Autoria);

                await ValidarOuInserirCreditoAutoresCoAutores(linhasComsucesso.Select(s => s.CoAutor.Conteudo).ToArray().UnificarPipe().SplitPipe().Distinct(), TipoCreditoAutoria.Autoria);
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

        private async Task<IEnumerable<AcervoBibliograficoLinhaDTO>> LerPlanilha(IFormFile file)
        {
            var linhas = new List<AcervoBibliograficoLinhaDTO>();

            var stream = file.OpenReadStream();

            using (var package = new XLWorkbook(stream))
            {
                var planilha = package.Worksheets.FirstOrDefault();

                var totalLinhas = planilha.Rows().Count();

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
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_CO_AUTOR),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200
                        },
                        TipoAutoria = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_TIPO_DE_AUTORIA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15
                        },
                        Editora = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_EDITORA),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200
                        },
                        Assunto = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_ASSUNTO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_200,
                            EhCampoObrigatorio = true
                        },
                        Ano = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_ANO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                            EhCampoObrigatorio = true
                        },
                        Edicao = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_EDICAO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                        },
                        NumeroPaginas = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_NUMERO_PAGINAS),
                            FormatoTipoDeCampo = Constantes.FORMATO_DOUBLE
                        },
                        Altura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_DIMENSAO_ALTURA),
                            FormatoTipoDeCampo = Constantes.FORMATO_DOUBLE
                        },
                        Largura = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_DIMENSAO_LARGURA),
                            FormatoTipoDeCampo = Constantes.FORMATO_DOUBLE
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
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_50,
                            EhCampoObrigatorio = true
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
                        Tombo = new LinhaConteudoAjustarDTO()
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
    }
}