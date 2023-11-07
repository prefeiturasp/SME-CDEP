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
        private readonly IServicoAcervoBibliografico servicoAcervoBibliografico;
        private readonly IMapper mapper;
        
        public ServicoImportacaoArquivoAcervoDocumental(IRepositorioImportacaoArquivo repositorioImportacaoArquivo, IMapper mapper, IServicoMaterial servicoMaterial,
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

        public async Task<ImportacaoArquivoRetornoDTO<AcervoDocumentalLinhaRetornoDTO>> ObterImportacaoPendente()
        {
            var arquivoImportado = await repositorioImportacaoArquivo.ObterUltimaImportacao(TipoAcervo.DocumentacaoHistorica);

            return ObterRetornoImportacaoAcervoDocumental(arquivoImportado, JsonConvert.DeserializeObject<IEnumerable<AcervoDocumentalLinhaDTO>>(arquivoImportado.Conteudo));
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
            
            await PersistenciaAcervoDocumental(acervosDocumentalLinhas, importacaoArquivoId);
            await AtualizarImportacao(importacaoArquivoId, JsonConvert.SerializeObject(acervosDocumentalLinhas), acervosDocumentalLinhas.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);

            var arquivoImportado = await repositorioImportacaoArquivo.ObterPorId(importacaoArquivoId);

            return ObterRetornoImportacaoAcervoDocumental(arquivoImportado, acervosDocumentalLinhas);
        }

        private ImportacaoArquivoRetornoDTO<AcervoDocumentalLinhaRetornoDTO> ObterRetornoImportacaoAcervoDocumental(ImportacaoArquivo arquivoImportado, IEnumerable<AcervoDocumentalLinhaDTO> acervosDocumentalLinhas)
        {
            var acervoDocumentalRetorno = new ImportacaoArquivoRetornoDTO<AcervoDocumentalLinhaRetornoDTO>()
            {
                Id = arquivoImportado.Id,
                Nome = arquivoImportado.Nome,
                TipoAcervo = arquivoImportado.TipoAcervo,
                DataImportacao = arquivoImportado.CriadoEm,
                Erros = acervosDocumentalLinhas
                        .Where(w => w.PossuiErros)
                        .Select(s => new AcervoDocumentalLinhaRetornoDTO()
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
                        }),
                Sucesso = acervosDocumentalLinhas
                        .Where(w => !w.PossuiErros)
                        .Select(s => new AcervoDocumentalLinhaRetornoDTO()
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
                        })
            };
            return acervoDocumentalRetorno;
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

        public async Task PersistenciaAcervoDocumental(IEnumerable<AcervoDocumentalLinhaDTO> acervosDocumentalLinhas, long importacaoArquivoId)
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
                        MaterialId = Materiais.FirstOrDefault(f => f.Nome.Equals(acervoDocumentalLinha.Material.Conteudo)).Id,
                        IdiomaId = Idiomas.FirstOrDefault(f => f.Nome.Equals(acervoDocumentalLinha.Idioma.Conteudo)).Id,
                        CreditosAutoresIds = CreditosAutores
                            .Where(f => acervoDocumentalLinha.Autor.Conteudo.FormatarTextoEmArray().Contains(f.Nome))
                            .Select(s => s.Id).ToArray(),
                        Ano = acervoDocumentalLinha.Ano.Conteudo,
                        NumeroPagina = acervoDocumentalLinha.NumeroPaginas.Conteudo,
                        Volume = acervoDocumentalLinha.Volume.Conteudo,
                        Descricao = acervoDocumentalLinha.Descricao.Conteudo,
                        TipoAnexo = acervoDocumentalLinha.TipoAnexo.Conteudo,
                        Largura = double.Parse(acervoDocumentalLinha.Largura.Conteudo),
                        Altura = double.Parse(acervoDocumentalLinha.Altura.Conteudo),
                        TamanhoArquivo = acervoDocumentalLinha.TamanhoArquivo.Conteudo,
                        AcessoDocumentosIds = CreditosAutores
                            .Where(f => acervoDocumentalLinha.AcessoDocumento.Conteudo.FormatarTextoEmArray().Contains(f.Nome))
                            .Select(s => s.Id).ToArray(),
                        Localizacao = acervoDocumentalLinha.Localizacao.Conteudo,
                        CopiaDigital = acervoDocumentalLinha.CopiaDigital.Conteudo,
                        ConservacaoId = Conservacoes.FirstOrDefault(f => f.Nome.Equals(acervoDocumentalLinha.EstadoConservacao.Conteudo)).Id
                    };
                    await servicoAcervoBibliografico.Inserir(acervoDocumental);

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

        public void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoDocumentalLinhaDTO> linhas)
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

        private bool PossuiErro(AcervoDocumentalLinhaDTO linha)
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

        public async Task ValidacaoObterOuInserirDominios(IEnumerable<AcervoDocumentalLinhaDTO> linhas)
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
                            EhCampoObrigatorio = true
                        },
                        CodigoNovo = new LinhaConteudoAjustarDTO()
                        {
                            Conteudo = planilha.ObterValorDaCelula(numeroLinha, Constantes.ACERVO_DOCUMENTAL_CAMPO_CODIGO_NOVO),
                            LimiteCaracteres = Constantes.CARACTERES_PERMITIDOS_15,
                            EhCampoObrigatorio = true
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