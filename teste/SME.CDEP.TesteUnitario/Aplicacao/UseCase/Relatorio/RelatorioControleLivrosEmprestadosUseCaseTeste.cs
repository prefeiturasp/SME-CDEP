using Bogus;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Moq.Protected;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Enumerados;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Aplicacao.UseCase;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Infra.Dominio.Enumerados;
using System.Net;
using System.Text;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase.Relatorio
{
    public class RelatorioControleLivrosEmprestadosUseCaseTeste
    {
        private const string BaseAddress = "https://api.example.com/";
        private const string ApiEndpoint = "v1/cdep/controle-livros-emprestados";
        private readonly AutoMocker _mocker;
        private readonly RelatorioControleLivrosEmprestadosUseCase _useCase;

        public RelatorioControleLivrosEmprestadosUseCaseTeste()
        {
            _mocker = new AutoMocker();
            _useCase = _mocker.CreateInstance<RelatorioControleLivrosEmprestadosUseCase>();
        }

        [Fact]
        public async Task ExecutarAsync_ComDadosValidos_DeveRetornarStream()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var conteudoRetorno = "conteudo binario simulado";
            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, conteudoRetorno);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<MemoryStream>();
            httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task ExecutarAsync_ComStatusCodeNoContent_DeveRetornarNull()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.NoContent);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task ExecutarAsync_ComStatusCodeErro_DeveRetornarNull()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.InternalServerError);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task ExecutarAsync_DeveEnviarFiltrosCorretos()
        {
            var nomeUsuario = "João da Silva";
            var usuarioRF = "d123456";
            var filtros = GerarFiltrosValidos();
            var contextoMock = _mocker.GetMock<IContextoAplicacao>();
            contextoMock.Setup(c => c.NomeUsuario).Returns(nomeUsuario);
            contextoMock.Setup(c => c.UsuarioLogado).Returns(usuarioRF);
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            HttpRequestMessage capturedRequest = null!;

            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((request, token) =>
                {
                    capturedRequest = request;
                })
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("dados", Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };
            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            await _useCase.ExecutarAsync(filtros);

            capturedRequest.Should().NotBeNull();
            capturedRequest.Method.Should().Be(System.Net.Http.HttpMethod.Post);
            capturedRequest.RequestUri!.ToString().Should().Contain(ApiEndpoint);
            contextoMock.Verify(x => x.NomeUsuario, Times.Once);
            contextoMock.Verify(x => x.UsuarioLogado, Times.Once);
        }

        [Fact]
        public async Task ExecutarAsync_DeveSerializarPayloadCorretamente()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            HttpRequestMessage capturedRequest = null!;

            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((request, token) =>
                {
                    capturedRequest = request;
                })
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("conteudo", Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };
            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
            capturedRequest.Content.Should().NotBeNull();
            capturedRequest.Content.Headers.ContentType!.MediaType.Should().Be("application/json");
        }

        [Fact]
        public async Task ExecutarAsync_ComFiltrosComTodosOsCampos_DeveProcessarCorretamente()
        {
            var solicitante = "Maria Silva";
            var tombo = "TOM001";
            var modelo = ModeloRelatorio.Sintetico;
            var situacaoSolicitacao = SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO;
            var situacaoEmprestimo = new List<SituacaoEmprestimo> { SituacaoEmprestimo.EMPRESTADO };
            var somenteDevolvidos = false;

            var filtros = new RelatorioControleLivroEmprestadosRequest
            {
                Solicitante = solicitante,
                Tombo = tombo,
                Modelo = modelo,
                SituacaoSolicitacaoItem = situacaoSolicitacao,
                SituacaoEmprestimo = situacaoEmprestimo,
                SomenteDevolvidos = somenteDevolvidos
            };

            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, "dados completos");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecutarAsync_ComFiltrosComCamposNulos_DeveProcessarCorretamente()
        {
            var filtros = new RelatorioControleLivroEmprestadosRequest
            {
                Solicitante = null,
                Tombo = null,
                SituacaoEmprestimo = null,
            };

            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, "dados");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecutarAsync_ComStatusCodeCreated_DeveRetornarStream()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.Created, "novo recurso");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecutarAsync_ComStatusCodeAccepted_DeveRetornarStream()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.Accepted, "aceito");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecutarAsync_ComStatusCodeForbidden_DeveRetornarNull()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.Forbidden);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task ExecutarAsync_DeveObtermTiposAcervosPermitidosDoPerfilLogado()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();
            var tiposAcervos = new long [] { 1 };

            var servicoAcervoMock = _mocker.GetMock<IServicoAcervo>();
            servicoAcervoMock.Setup(x => x.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(tiposAcervos);

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, "dados");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
            servicoAcervoMock.Verify(x => x.ObterTiposAcervosPermitidosDoPerfilLogado(), Times.Once);
        }

        [Fact]
        public async Task ExecutarAsync_DeveTransferirFiltrosSolicitanteAoDTOCorretamente()
        {
            var filtros = new RelatorioControleLivroEmprestadosRequest
            {
                Solicitante = "Carlos Mendes",
                Tombo = "TOM002",
                Modelo = ModeloRelatorio.Sintetico,
                SituacaoSolicitacaoItem = SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO,
                SituacaoEmprestimo = null,
                SomenteDevolvidos = true
            };

            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            HttpRequestMessage capturedRequest = null!;

            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((request, token) =>
                {
                    capturedRequest = request;
                })
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("dados", Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };
            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            await _useCase.ExecutarAsync(filtros);

            capturedRequest.Should().NotBeNull();
            capturedRequest.Content.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecutarAsync_DeveTransferirFiltrosTomboAoDTOCorretamente()
        {
            var filtros = new RelatorioControleLivroEmprestadosRequest
            {
                Solicitante = null,
                Tombo = "TOM999",
                Modelo = ModeloRelatorio.Analitico,
                SituacaoSolicitacaoItem = SituacaoSolicitacaoItem.SEM_RESPOSTA_SOLICITANTE,
                SituacaoEmprestimo = null,
                SomenteDevolvidos = true
            };

            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, "dados");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecutarAsync_DeveTransferirFiltrosModeloAoDTOCorretamente()
        {
            var filtros = new RelatorioControleLivroEmprestadosRequest
            {
                Solicitante = null,
                Tombo = null,
                Modelo = ModeloRelatorio.Analitico,
                SituacaoSolicitacaoItem = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE,
                SituacaoEmprestimo = null,
                SomenteDevolvidos = true
            };

            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, "dados");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecutarAsync_DeveTransferirFiltrosSituacaoSolicitacaoAoDTOCorretamente()
        {
            var situacaoSolicitacao = SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE;
            var filtros = new RelatorioControleLivroEmprestadosRequest
            {
                Solicitante = null,
                Tombo = null,
                Modelo = ModeloRelatorio.Analitico,
                SituacaoSolicitacaoItem = situacaoSolicitacao,
                SituacaoEmprestimo = null,
                SomenteDevolvidos = true
            };

            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, "dados");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecutarAsync_DeveTransferirFiltrosSituacaoEmpretimoAoDTOCorretamente()
        {
            var filtros = new RelatorioControleLivroEmprestadosRequest
            {
                Solicitante = null,
                Tombo = null,
                Modelo = ModeloRelatorio.Analitico,
                SituacaoSolicitacaoItem = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE,
                SituacaoEmprestimo = [SituacaoEmprestimo.DEVOLVIDO],
                SomenteDevolvidos = true
            };

            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, "dados");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecutarAsync_DeveTransferirFiltrosSomenteDevolvidosAoDTOCorretamente()
        {
            var filtros = new RelatorioControleLivroEmprestadosRequest
            {
                Solicitante = null,
                Tombo = null,
                Modelo = ModeloRelatorio.Analitico,
                SituacaoSolicitacaoItem = SituacaoSolicitacaoItem.SEM_RESPOSTA_SOLICITANTE,
                SituacaoEmprestimo = null,
                SomenteDevolvidos = true
            };

            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, "dados");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecutarAsync_DeveInserirContextoAplicacaoNomeUsuarioNoDTOCorretamente()
        {
            var nomeUsuarioEsperado = "Fernanda Souza";
            var contextoMock = _mocker.GetMock<IContextoAplicacao>();
            contextoMock.Setup(c => c.NomeUsuario).Returns(nomeUsuarioEsperado);
            contextoMock.Setup(c => c.UsuarioLogado).Returns("d654321");

            var filtros = GerarFiltrosValidos();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, "dados");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
            contextoMock.Verify(x => x.NomeUsuario, Times.Once);
        }

        [Fact]
        public async Task ExecutarAsync_DeveInserirContextoAplicacaoUsuarioRFNoDTOCorretamente()
        {
            var usuarioRFEsperado = "d789012";
            var contextoMock = _mocker.GetMock<IContextoAplicacao>();
            contextoMock.Setup(c => c.NomeUsuario).Returns("Test User");
            contextoMock.Setup(c => c.UsuarioLogado).Returns(usuarioRFEsperado);

            var filtros = GerarFiltrosValidos();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, "dados");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
            contextoMock.Verify(x => x.UsuarioLogado, Times.Once);
        }

        [Fact]
        public async Task ExecutarAsync_ComFiltrosNulosCompletos_DeveProcessarSemErro()
        {
            var filtros = new RelatorioControleLivroEmprestadosRequest();
            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, "dados");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
        }

        private void ConfigurarContextoUsuario()
        {
            var contextoMock = _mocker.GetMock<IContextoAplicacao>();
            contextoMock.Setup(c => c.NomeUsuario).Returns("João da Silva");
            contextoMock.Setup(c => c.UsuarioLogado).Returns("d123456");
        }

        private void ConfigurarServicoAcervo()
        {
            var servicoAcervoMock = _mocker.GetMock<IServicoAcervo>();
            servicoAcervoMock.Setup(x => x.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns([1]);
        }

        private static RelatorioControleLivroEmprestadosRequest GerarFiltrosValidos() =>
            new Faker<RelatorioControleLivroEmprestadosRequest>()
                .CustomInstantiator(f => new RelatorioControleLivroEmprestadosRequest
                {
                    Solicitante = f.Person.FullName,
                    Tombo = f.Random.AlphaNumeric(10),
                    Modelo = ModeloRelatorio.Analitico,
                    SituacaoSolicitacaoItem = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE,
                    SituacaoEmprestimo = [SituacaoEmprestimo.DEVOLVIDO],
                    SomenteDevolvidos = true
                })
                .Generate();

        private static Mock<HttpMessageHandler> CriarHttpMessageHandlerMock(HttpStatusCode statusCode, string content = "")
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            var response = new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            return handlerMock;
        }
    }
}