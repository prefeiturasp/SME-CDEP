using Bogus;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.AutoMock;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoImportacaoArquivoAcervoBibliograficoTestes
    {
        private readonly AutoMocker _mocker;
        private readonly ServicoImportacaoArquivoAcervoBibliografico _servico;
        private readonly Faker _faker;

        public ServicoImportacaoArquivoAcervoBibliograficoTestes()
        {
            _mocker = new AutoMocker();
            _faker = new Faker("pt_BR");
            _servico = _mocker.CreateInstance<ServicoImportacaoArquivoAcervoBibliografico>();

            // Configuração padrão para permitir a leitura do limite de linhas em todos os testes
            _mocker.GetMock<IRepositorioParametroSistema>()
               .Setup(x => x.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
               .ReturnsAsync(new ParametroSistema { Valor = "1000" });
        }

        [Fact]
        public async Task DadoQueArquivoEhValido_QuandoImportarArquivo_EntaoDeveProcessarEPublicarMensagem()
        {
            // Arrange
            var arquivoExcel = GerarArquivoExcelValido();

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.Salvar(It.IsAny<ImportacaoArquivo>()))
                .ReturnsAsync(123);

            // Act
            var resultado = await _servico.ImportarArquivo(arquivoExcel);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(123, resultado.Id);
            Assert.Equal(ImportacaoStatus.Pendente, resultado.Status);

            // Verifica se salvou no banco
            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Verify(x => x.Salvar(It.Is<ImportacaoArquivo>(i =>
                    i.Nome == Constantes.PLANILHA_ACERVO_BIBLIOGRAFICO &&
                    i.TipoAcervo == TipoAcervo.Bibliografico &&
                    i.Conteudo.Contains("Dom Casmurro") // Verifica se leu o dado da planilha
                )), Times.Once);

            // Verifica se publicou na fila
            _mocker.GetMock<IServicoMensageria>()
                .Verify(x => x.Publicar(
                    RotasRabbit.ExecutarImportacaoArquivoAcervoBibliografico,
                    (long)123,
                    It.IsAny<Guid?>(), It.IsAny<Usuario?>(), It.IsAny<bool>(), It.IsAny<string?>()
                ), Times.Once);
        }

        [Fact]
        public async Task DadoQueArquivoPossuiCabecalhoInvalido_QuandoImportarArquivo_EntaoDeveLancarNegocioException()
        {
            // Arrange
            // Gera arquivo e força um erro no título da coluna 1
            var arquivoExcel = GerarArquivoExcelValido(alterarCabecalho: true);

            // Act & Assert
            // A mensagem exata depende da formatação da string, validamos o tipo da exceção
            await Assert.ThrowsAsync<NegocioException>(() => _servico.ImportarArquivo(arquivoExcel));
        }

        [Fact]
        public async Task DadoQueArquivoExcedeLimiteDeLinhas_QuandoImportarArquivo_EntaoDeveLancarNegocioException()
        {
            // Arrange
            // Configura limite para 1, e geramos 2 linhas de dados
            _mocker.GetMock<IRepositorioParametroSistema>()
               .Setup(x => x.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
               .ReturnsAsync(new ParametroSistema { Valor = "1" });

            var arquivoExcel = GerarArquivoExcelValido(quantidadeLinhas: 2);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.ImportarArquivo(arquivoExcel));
        }

        [Fact]
        public void DadoQueArquivoTemExtensaoInvalida_QuandoValidarArquivo_EntaoDeveLancarExcecao()
        {
            // Arrange
            var arquivoMock = new Mock<IFormFile>();
            arquivoMock.Setup(x => x.FileName).Returns("arquivo.txt");
            arquivoMock.Setup(x => x.ContentType).Returns("text/plain");
            arquivoMock.Setup(x => x.Length).Returns(100);

            // Act & Assert
            Assert.Throws<NegocioException>(() => ServicoImportacaoArquivoBase.ValidarArquivo(arquivoMock.Object));
        }

        [Fact]
        public async Task DadoQueExistemCoAutoresETiposAutoriaValidosQuandoObterCoAutoresTipoAutoriaEntaoDeveRetornarDTOCorreto()
        {
            // Arrange
            var coAutoresBase = new List<IdNomeTipoDTO>
            {
                new() { Id = 1, Nome = "Autor Um" },
                new() { Id = 2, Nome = "Autor Dois" }
            };
            _servico.DefinirCoAutores(coAutoresBase);

            var stringCoAutores = "Autor Um|Autor Dois|Autor Dois";
            var stringTipos = "Ilustrador|Tradutor|Tradutor";

            // Act
            var resultado = _servico.ObterCoAutoresTipoAutoria(stringCoAutores, stringTipos);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Length);
            Assert.Equal(1, resultado[0].CreditoAutorId);
            Assert.Equal("Ilustrador", resultado[0].TipoAutoria);
            Assert.Equal(2, resultado[1].CreditoAutorId);
            Assert.Equal("Tradutor", resultado[1].TipoAutoria);
        }

        [Fact]
        public async Task DadoQueLinhaUnicaEhRemovida_QuandoRemoverLinhaDoArquivo_EntaoDeveLancarErro()
        {
            // Arrange
            var importacao = new ImportacaoArquivo
            {
                Id = 1,
                TipoAcervo = TipoAcervo.Bibliografico,
                Conteudo = JsonConvert.SerializeObject(new List<AcervoBibliograficoLinhaDTO> {
                    new AcervoBibliograficoLinhaDTO { NumeroLinha = 1 }
                })
            };

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.ObterPorId(1))
                .ReturnsAsync(importacao);

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<NegocioException>(() =>
                _servico.RemoverLinhaDoArquivo(1, new LinhaDTO { NumeroLinha = 1 }));

            Assert.Equal(Constantes.NAO_EH_POSSIVEL_EXCLUIR_A_UNICA_LINHA_DO_ARQUIVO, excecao.Message);
        }

        [Fact]
        public async Task DadoQueLinhaPossuiErro_QuandoAtualizarLinhaParaSucesso_EntaoDeveAtualizarJsonEStatusParaSucesso()
        {
            // Arrange
            long idImportacao = 10;
            int numeroLinhaAlvo = 5;

            // 1. Prepara o conteúdo simulado do banco (uma linha com erro)
            var listaLinhas = new List<AcervoBibliograficoLinhaDTO>
            {
                new AcervoBibliograficoLinhaDTO
                {
                    NumeroLinha = numeroLinhaAlvo,
                    PossuiErros = true, // Estado atual: Erro
                    Status = ImportacaoStatus.Erros,
                    // Inicializamos os objetos internos para garantir a serialização correta
                    Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Livro Teste", PossuiErro = true },
                    SubTitulo = new LinhaConteudoAjustarDTO(),
                    Material = new LinhaConteudoAjustarDTO(),
                    Autor = new LinhaConteudoAjustarDTO(),
                    CoAutor = new LinhaConteudoAjustarDTO(),
                    TipoAutoria = new LinhaConteudoAjustarDTO(),
                    Editora = new LinhaConteudoAjustarDTO(),
                    Assunto = new LinhaConteudoAjustarDTO(),
                    Ano = new LinhaConteudoAjustarDTO(),
                    Edicao = new LinhaConteudoAjustarDTO(),
                    NumeroPaginas = new LinhaConteudoAjustarDTO(),
                    Altura = new LinhaConteudoAjustarDTO(),
                    Largura = new LinhaConteudoAjustarDTO(),
                    SerieColecao = new LinhaConteudoAjustarDTO(),
                    Volume = new LinhaConteudoAjustarDTO(),
                    Idioma = new LinhaConteudoAjustarDTO(),
                    LocalizacaoCDD = new LinhaConteudoAjustarDTO(),
                    LocalizacaoPHA = new LinhaConteudoAjustarDTO(),
                    NotasGerais = new LinhaConteudoAjustarDTO(),
                    Isbn = new LinhaConteudoAjustarDTO(),
                    Codigo = new LinhaConteudoAjustarDTO()
                }
            };

            var conteudoJson = JsonConvert.SerializeObject(listaLinhas);

            var arquivoNoBanco = new ImportacaoArquivo
            {
                Id = idImportacao,
                TipoAcervo = TipoAcervo.Bibliografico, // Deve corresponder ao esperado no método
                Status = ImportacaoStatus.Erros,
                Conteudo = conteudoJson
            };

            // 2. Mock do Repositório para retornar esse arquivo
            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.ObterPorId(idImportacao))
                .ReturnsAsync(arquivoNoBanco);

            // 3. Mock do Salvar
            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.Salvar(It.IsAny<ImportacaoArquivo>()))
                .ReturnsAsync(idImportacao);

            var linhaDto = new LinhaDTO { NumeroLinha = numeroLinhaAlvo };

            // Act
            var resultado = await _servico.AtualizarLinhaParaSucesso(idImportacao, linhaDto);

            // Assert
            Assert.True(resultado);

            // Verifica se o método Salvar foi chamado com o objeto modificado corretamente
            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Verify(x => x.Salvar(It.Is<ImportacaoArquivo>(imp =>
                    imp.Id == idImportacao &&
                    imp.Status == ImportacaoStatus.Sucesso && // Como era a única linha e foi arrumada, o status geral vira Sucesso
                    imp.Conteudo.Contains($"\"NumeroLinha\":{numeroLinhaAlvo}") && // Garante que é a linha certa
                    imp.Conteudo.Contains("\"PossuiErros\":false") && // Verifica se o flag de erro foi removido no JSON
                    imp.Conteudo.Contains($"\"Status\":{(int)ImportacaoStatus.Sucesso}") // Verifica se o status interno da linha mudou
                )), Times.Once);
        }
        [Fact]
        public async Task DadoQueNaoExisteImportacaoPendente_QuandoObterImportacaoPendente_EntaoDeveRetornarNulo()
        {
            // Act
            var resultado = await _servico.ObterImportacaoPendente();

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public async Task DadoQueExisteImportacaoPendente_QuandoObterImportacaoPendente_EntaoDeveRetornarDTOPreenchidoCorretamente()
        {
            // Arrange
            // 1. Prepara linhas: Uma com sucesso, uma com erro
            var linhaSucesso = GerarLinhaFake(1, false);
            var linhaErro = GerarLinhaFake(2, true);
            var listaLinhas = new List<AcervoBibliograficoLinhaDTO> { linhaSucesso, linhaErro };

            var conteudoJson = JsonConvert.SerializeObject(listaLinhas);
            var dataImportacao = DateTime.Now;

            var importacaoArquivo = new ImportacaoArquivo
            {
                Id = 10,
                Nome = "planilha_bibliografia.xlsx",
                TipoAcervo = TipoAcervo.Bibliografico,
                Status = ImportacaoStatus.Pendente,
                Conteudo = conteudoJson,
                CriadoEm = dataImportacao
            };

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.ObterUltimaImportacao(TipoAcervo.Bibliografico))
                .ReturnsAsync(importacaoArquivo);

            // Act
            var resultado = await _servico.ObterImportacaoPendente();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(importacaoArquivo.Id, resultado.Id);
            Assert.Equal(importacaoArquivo.Nome, resultado.Nome);
            Assert.Equal(importacaoArquivo.Status, resultado.Status);
            Assert.Equal(dataImportacao, resultado.DataImportacao);

            // Valida separação de Sucesso
            Assert.Single(resultado.Sucesso);
            Assert.Equal(linhaSucesso.NumeroLinha, resultado.Sucesso.First().NumeroLinha);
            Assert.Equal(linhaSucesso.Titulo.Conteudo, resultado.Sucesso.First().Titulo);

            // Valida separação de Erros
            Assert.Single(resultado.Erros);
            Assert.Equal(linhaErro.NumeroLinha, resultado.Erros.First().NumeroLinha);
            Assert.Equal(linhaErro.Titulo.Conteudo, resultado.Erros.First().Titulo);
        }

        [Fact]
        public async Task DadoQueNaoExisteImportacaoPorId_QuandoObterImportacaoPorId_EntaoDeveRetornarNulo()
        {
            // Arrange
            long idPesquisa = 99;

            // Act
            var resultado = await _servico.ObterImportacaoPorId(idPesquisa);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public async Task DadoQueExisteImportacaoPorId_QuandoObterImportacaoPorId_EntaoDeveRetornarDTOComTodasAsLinhas()
        {
            // Arrange
            long idPesquisa = 50;

            // Gera 3 linhas de sucesso
            var linhas = new List<AcervoBibliograficoLinhaDTO>
            {
                GerarLinhaFake(1, false),
                GerarLinhaFake(2, false),
                GerarLinhaFake(3, false)
            };

            var conteudoJson = JsonConvert.SerializeObject(linhas);

            var importacaoArquivo = new ImportacaoArquivo
            {
                Id = idPesquisa,
                Nome = "arquivo_antigo.xlsx",
                TipoAcervo = TipoAcervo.Bibliografico,
                Status = ImportacaoStatus.Sucesso,
                Conteudo = conteudoJson
            };

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.ObterImportacaoPorId(idPesquisa))
                .ReturnsAsync(importacaoArquivo);

            // Act
            var resultado = await _servico.ObterImportacaoPorId(idPesquisa);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(idPesquisa, resultado.Id);
            Assert.Equal(3, resultado.Sucesso.Count());
            Assert.Empty(resultado.Erros);
        }
        [Fact]
        public async Task DadoQueLinhaPossuiErrosEmTodosOsCampos_QuandoObterImportacaoPendente_EntaoDeveRetornarTodasAsMensagensDeErroMapeadas()
        {
            // Arrange
            // Cria uma linha onde TODOS os campos monitorados pelo método ObterMensagemErroLinha possuem erro
            var linhaCaos = new AcervoBibliograficoLinhaDTO
            {
                NumeroLinha = 1,
                PossuiErros = true,
                Titulo = CriarCampoComErro("Erro no Título"),
                Codigo = CriarCampoComErro("Erro no Código"),
                SubTitulo = CriarCampoComErro("Erro no Subtítulo"),
                Material = CriarCampoComErro("Erro no Material"),
                Autor = CriarCampoComErro("Erro no Autor"),
                CoAutor = CriarCampoComErro("Erro no CoAutor"),
                TipoAutoria = CriarCampoComErro("Erro no Tipo de Autoria"),
                Editora = CriarCampoComErro("Erro na Editora"),
                Edicao = CriarCampoComErro("Erro na Edição"),
                Assunto = CriarCampoComErro("Erro no Assunto"),
                Ano = CriarCampoComErro("Erro no Ano"),
                NumeroPaginas = CriarCampoComErro("Erro no Número de Páginas"),
                Largura = CriarCampoComErro("Erro na Largura"),
                Altura = CriarCampoComErro("Erro na Altura"),
                SerieColecao = CriarCampoComErro("Erro na Série"),
                Volume = CriarCampoComErro("Erro no Volume"),
                Idioma = CriarCampoComErro("Erro no Idioma"),
                LocalizacaoCDD = CriarCampoComErro("Erro no CDD"),
                LocalizacaoPHA = CriarCampoComErro("Erro no PHA"),
                NotasGerais = CriarCampoComErro("Erro nas Notas"),
                Isbn = CriarCampoComErro("Erro no ISBN")
            };

            var conteudoJson = JsonConvert.SerializeObject(new List<AcervoBibliograficoLinhaDTO> { linhaCaos });

            var importacaoArquivo = new ImportacaoArquivo
            {
                Id = 100,
                Conteudo = conteudoJson,
                TipoAcervo = TipoAcervo.Bibliografico,
                Status = ImportacaoStatus.Erros
            };

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.ObterUltimaImportacao(TipoAcervo.Bibliografico))
                .ReturnsAsync(importacaoArquivo);

            // Act
            var resultado = await _servico.ObterImportacaoPendente();

            // Assert
            Assert.NotNull(resultado);
            var linhaErroRetornada = resultado.Erros.FirstOrDefault();
            Assert.NotNull(linhaErroRetornada);

            var mensagensDeErro = linhaErroRetornada.RetornoErro.ErrosCampos;

            // Validamos a quantidade exata de campos validados (21 campos no método original)
            Assert.Equal(21, mensagensDeErro.Length);

            // Verificação por amostragem das mensagens para garantir que o mapeamento está correto
            Assert.Contains("Erro no Título", mensagensDeErro);
            Assert.Contains("Erro no Código", mensagensDeErro);
            Assert.Contains("Erro no ISBN", mensagensDeErro);
            Assert.Contains("Erro nas Notas", mensagensDeErro);
        }

        [Fact]
        public async Task DadoQueLinhaPossuiErroGenericoMasCamposEstaoValidos_QuandoObterImportacaoPendente_EntaoDeveRetornarArrayDeErrosVazio()
        {
            // Arrange
            // Linha marcada com erro (ex: duplicação), mas os campos individuais estão OK
            var linhaErroGenerico = new AcervoBibliograficoLinhaDTO
            {
                NumeroLinha = 2,
                PossuiErros = true,
                Mensagem = "Erro genérico de linha",
                // Inicializa campos sem erro (PossuiErro = false)
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Ok" },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = "Ok" },
                SubTitulo = new LinhaConteudoAjustarDTO(),
                Material = new LinhaConteudoAjustarDTO(),
                Autor = new LinhaConteudoAjustarDTO(),
                CoAutor = new LinhaConteudoAjustarDTO(),
                TipoAutoria = new LinhaConteudoAjustarDTO(),
                Editora = new LinhaConteudoAjustarDTO(),
                Edicao = new LinhaConteudoAjustarDTO(),
                Assunto = new LinhaConteudoAjustarDTO(),
                Ano = new LinhaConteudoAjustarDTO(),
                NumeroPaginas = new LinhaConteudoAjustarDTO(),
                Largura = new LinhaConteudoAjustarDTO(),
                Altura = new LinhaConteudoAjustarDTO(),
                SerieColecao = new LinhaConteudoAjustarDTO(),
                Volume = new LinhaConteudoAjustarDTO(),
                Idioma = new LinhaConteudoAjustarDTO(),
                LocalizacaoCDD = new LinhaConteudoAjustarDTO(),
                LocalizacaoPHA = new LinhaConteudoAjustarDTO(),
                NotasGerais = new LinhaConteudoAjustarDTO(),
                Isbn = new LinhaConteudoAjustarDTO()
            };

            var conteudoJson = JsonConvert.SerializeObject(new List<AcervoBibliograficoLinhaDTO> { linhaErroGenerico });

            var importacaoArquivo = new ImportacaoArquivo
            {
                Id = 101,
                Conteudo = conteudoJson,
                TipoAcervo = TipoAcervo.Bibliografico
            };

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.ObterUltimaImportacao(TipoAcervo.Bibliografico))
                .ReturnsAsync(importacaoArquivo);

            // Act
            var resultado = await _servico.ObterImportacaoPendente();

            // Assert
            var linhaErroRetornada = resultado.Erros.FirstOrDefault();

            // Deve existir a linha de erro (pois PossuiErros = true)
            Assert.NotNull(linhaErroRetornada);

            // Mas a lista de ErrosCampos deve estar vazia, pois nenhum if foi satisfeito
            Assert.Empty(linhaErroRetornada.RetornoErro.ErrosCampos);
        }

        #region Helpers

        private static LinhaConteudoAjustarDTO CriarCampoComErro(string mensagem)
        {
            return new LinhaConteudoAjustarDTO
            {
                Conteudo = "Valor Inválido",
                PossuiErro = true,
                Mensagem = mensagem
            };
        }
        private AcervoBibliograficoLinhaDTO GerarLinhaFake(int numeroLinha, bool erro)
        {
            return new AcervoBibliograficoLinhaDTO
            {
                NumeroLinha = numeroLinha,
                PossuiErros = erro,
                Mensagem = erro ? "Erro simulado" : null,
                // Inicializa propriedades para evitar NullReference na projeção do DTO
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = _faker.Lorem.Sentence() },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = _faker.Random.AlphaNumeric(10) },
                SubTitulo = new LinhaConteudoAjustarDTO(),
                Material = new LinhaConteudoAjustarDTO(),
                Autor = new LinhaConteudoAjustarDTO(),
                CoAutor = new LinhaConteudoAjustarDTO(),
                TipoAutoria = new LinhaConteudoAjustarDTO(),
                Editora = new LinhaConteudoAjustarDTO(),
                Assunto = new LinhaConteudoAjustarDTO(),
                Ano = new LinhaConteudoAjustarDTO(),
                Edicao = new LinhaConteudoAjustarDTO(),
                NumeroPaginas = new LinhaConteudoAjustarDTO(),
                Altura = new LinhaConteudoAjustarDTO(),
                Largura = new LinhaConteudoAjustarDTO(),
                SerieColecao = new LinhaConteudoAjustarDTO(),
                Volume = new LinhaConteudoAjustarDTO(),
                Idioma = new LinhaConteudoAjustarDTO(),
                LocalizacaoCDD = new LinhaConteudoAjustarDTO(),
                LocalizacaoPHA = new LinhaConteudoAjustarDTO(),
                NotasGerais = new LinhaConteudoAjustarDTO(),
                Isbn = new LinhaConteudoAjustarDTO()
            };
        }

        #endregion

        #region Helpers para Geração de Excel

        private IFormFile GerarArquivoExcelValido(bool alterarCabecalho = false, int quantidadeLinhas = 1)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Planilha1");

            // Configurar Cabeçalhos baseados nas Constantes
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_TITULO, alterarCabecalho ? "TITULO_ERRADO" : Constantes.NOME_DA_COLUNA_TITULO);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_SUB_TITULO, Constantes.NOME_DA_COLUNA_SUBTITULO);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_MATERIAL, Constantes.NOME_DA_COLUNA_MATERIAL);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_AUTOR, Constantes.NOME_DA_COLUNA_AUTOR);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_COAUTOR, Constantes.NOME_DA_COLUNA_COAUTOR);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_TIPO_DE_AUTORIA, Constantes.NOME_DA_COLUNA_TIPO_DE_AUTORIA);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_EDITORA, Constantes.NOME_DA_COLUNA_EDITORA);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_ASSUNTO, Constantes.NOME_DA_COLUNA_ASSUNTO);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_ANO, Constantes.NOME_DA_COLUNA_ANO);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_EDICAO, Constantes.NOME_DA_COLUNA_EDICAO);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_NUMERO_PAGINAS, Constantes.NOME_DA_COLUNA_NUMERO_PAGINAS);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_DIMENSAO_ALTURA, Constantes.NOME_DA_COLUNA_DIMENSAO_ALTURA);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_DIMENSAO_LARGURA, Constantes.NOME_DA_COLUNA_DIMENSAO_LARGURA);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_SERIE_COLECAO, Constantes.NOME_DA_COLUNA_SERIE_COLECAO);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_VOLUME, Constantes.NOME_DA_COLUNA_VOLUME);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_IDIOMA, Constantes.NOME_DA_COLUNA_IDIOMA);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_LOCALIZACAO_CDD, Constantes.NOME_DA_COLUNA_LOCALIZACAO_CDD);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_LOCALIZACAO_PHA, Constantes.NOME_DA_COLUNA_LOCALIZACAO_PHA);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_NOTAS_GERAIS, Constantes.NOME_DA_COLUNA_NOTAS_GERAIS);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_ISBN, Constantes.NOME_DA_COLUNA_ISBN);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_TOMBO, Constantes.NOME_DA_COLUNA_TOMBO);

            // Adicionar Dados
            for (int i = 0; i < quantidadeLinhas; i++)
            {
                int linha = Constantes.INICIO_LINHA_DADOS + i;
                worksheet.Cell(linha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_TITULO).Value = "Dom Casmurro";
                worksheet.Cell(linha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_MATERIAL).Value = "Livro";
                worksheet.Cell(linha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_AUTOR).Value = "Machado de Assis";
                worksheet.Cell(linha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_ASSUNTO).Value = "Literatura";
                worksheet.Cell(linha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_ANO).Value = "1899";
                worksheet.Cell(linha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_IDIOMA).Value = "Português";
                worksheet.Cell(linha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_LOCALIZACAO_CDD).Value = "869.3";
                worksheet.Cell(linha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_TOMBO).Value = "TOMBO123";
                worksheet.Cell(linha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_DIMENSAO_ALTURA).Value = "20,50"; // Formato esperado pelo regex
                worksheet.Cell(linha, Constantes.ACERVO_BIBLIOGRAFICO_CAMPO_DIMENSAO_LARGURA).Value = "14,00";
            }

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.OpenReadStream()).Returns(stream);
            fileMock.Setup(_ => _.FileName).Returns(Constantes.PLANILHA_ACERVO_BIBLIOGRAFICO);
            fileMock.Setup(_ => _.Length).Returns(stream.Length);
            fileMock.Setup(_ => _.ContentType).Returns(Constantes.CONTENT_TYPE_EXCEL);

            return fileMock.Object;
        }

        private void AdicionarCabecalho(IXLWorksheet worksheet, int coluna, string valor)
        {
            worksheet.Cell(Constantes.INICIO_LINHA_TITULO, coluna).Value = valor;
        }

        #endregion
    }
}
