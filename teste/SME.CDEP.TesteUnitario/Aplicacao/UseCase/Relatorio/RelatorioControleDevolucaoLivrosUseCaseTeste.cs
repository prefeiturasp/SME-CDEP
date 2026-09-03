using Bogus;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Moq.Protected;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Aplicacao.UseCase;
using SME.CDEP.Dominio.Contexto;
using System.Net;
using System.Text;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase.Relatorio
{
    public class RelatorioControleDevolucaoLivrosUseCaseTeste
    {
        private const string BaseAddress = "https://api.example.com/";
        private const string ApiEndpoint = "v1/cdep/controle-devolucao-livros";
        private readonly AutoMocker _mocker;
        private readonly RelatorioControleDevolucaoLivrosUseCase _useCase;

        public RelatorioControleDevolucaoLivrosUseCaseTeste()
        {
            _mocker = new AutoMocker();
            _useCase = _mocker.CreateInstance<RelatorioControleDevolucaoLivrosUseCase>();
        }

        [Fact]
        public async Task Executar_ComDadosValidos_DeveRetornarStream()
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

            var resultado = await _useCase.Executar(filtros);

            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<MemoryStream>();
            httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task Executar_ComStatusCodeNoContent_DeveRetornarNull()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.NoContent);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.Executar(filtros);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task Executar_ComStatusCodeErro_DeveRetornarNull()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.InternalServerError);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.Executar(filtros);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task Executar_DeveEnviarFiltrosCorretos()
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

            await _useCase.Executar(filtros);

            capturedRequest.Should().NotBeNull();
            capturedRequest.Method.Should().Be(System.Net.Http.HttpMethod.Post);
            capturedRequest.RequestUri!.ToString().Should().Contain(ApiEndpoint);
            contextoMock.Verify(x => x.NomeUsuario, Times.Once);
            contextoMock.Verify(x => x.UsuarioLogado, Times.Once);
        }

        [Fact]
        public async Task Executar_DeveSerializarPayloadCorretamente()
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

            var resultado = await _useCase.Executar(filtros);

            resultado.Should().NotBeNull();
            capturedRequest.Content.Should().NotBeNull();
            capturedRequest.Content.Headers.ContentType!.MediaType.Should().Be("application/json");
        }

        [Fact]
        public async Task Executar_ComFiltrosComTodosOsCampos_DeveProcessarCorretamente()
        {
            var solicitante = "Maria Silva";
            var somenteEmAtraso = true;

            var filtros = new RelatorioControleDevolucaoLivrosRequest
            {
                Solicitante = solicitante,
                SomenteEmAtraso = somenteEmAtraso
            };

            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, "dados completos");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.Executar(filtros);

            resultado.Should().NotBeNull();
        }

        [Fact]
        public async Task Executar_ComFiltrosComCamposNulos_DeveProcessarCorretamente()
        {
            var filtros = new RelatorioControleDevolucaoLivrosRequest
            {
                Solicitante = null,
                SomenteEmAtraso = false
            };

            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, "dados");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.Executar(filtros);

            resultado.Should().NotBeNull();
        }

        [Fact]
        public async Task Executar_ComStatusCodeCreated_DeveRetornarStream()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.Created, "novo recurso");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.Executar(filtros);

            resultado.Should().NotBeNull();
        }

        [Fact]
        public async Task Executar_ComStatusCodeAccepted_DeveRetornarStream()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.Accepted, "aceito");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.Executar(filtros);

            resultado.Should().NotBeNull();
        }

        [Fact]
        public async Task Executar_ComStatusCodeForbidden_DeveRetornarNull()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.Forbidden);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.Executar(filtros);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task Executar_DeveObtermTiposAcervosPermitidosDoPerfilLogado()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();
            var tiposAcervos = new long[] { 1 };

            var servicoAcervoMock = _mocker.GetMock<IServicoAcervo>();
            servicoAcervoMock.Setup(x => x.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(tiposAcervos);

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, "dados");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.Executar(filtros);

            resultado.Should().NotBeNull();
            servicoAcervoMock.Verify(x => x.ObterTiposAcervosPermitidosDoPerfilLogado(), Times.Once);
        }

        [Fact]
        public async Task Executar_DeveTransferirFiltrosSolicitanteAoDTOCorretamente()
        {
            var filtros = new RelatorioControleDevolucaoLivrosRequest
            {
                Solicitante = "Carlos Mendes",
                SomenteEmAtraso = false
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

            await _useCase.Executar(filtros);

            capturedRequest.Should().NotBeNull();
            capturedRequest.Content.Should().NotBeNull();
        }

        [Fact]
        public async Task Executar_DeveTransferirFiltrosSomenteEmAtrasoAoDTOCorretamente()
        {
            var filtros = new RelatorioControleDevolucaoLivrosRequest
            {
                Solicitante = null,
                SomenteEmAtraso = true
            };

            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, "dados");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.Executar(filtros);

            resultado.Should().NotBeNull();
        }

        [Fact]
        public async Task Executar_DeveInserirContextoAplicacaoNomeUsuarioNoDTOCorretamente()
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

            var resultado = await _useCase.Executar(filtros);

            resultado.Should().NotBeNull();
            contextoMock.Verify(x => x.NomeUsuario, Times.Once);
        }

        [Fact]
        public async Task Executar_DeveInserirContextoAplicacaoUsuarioRFNoDTOCorretamente()
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

            var resultado = await _useCase.Executar(filtros);

            resultado.Should().NotBeNull();
            contextoMock.Verify(x => x.UsuarioLogado, Times.Once);
        }

        [Fact]
        public async Task Executar_ComFiltrosNulosCompletos_DeveProcessarSemErro()
        {
            var filtros = new RelatorioControleDevolucaoLivrosRequest();
            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, "dados");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.Executar(filtros);

            resultado.Should().NotBeNull();
        }

        [Fact]
        public async Task Executar_ComStatusCodeForbiddenENoContent_DeveRetornarNull()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var resposta = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Forbidden,
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(resposta);

            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };
            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.Executar(filtros);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task Executar_DeveUsarClienteHTTPApiSR()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, "dados");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            var httpClientFactoryMock = _mocker.GetMock<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient)
                .Verifiable();

            var resultado = await _useCase.Executar(filtros);

            resultado.Should().NotBeNull();
            httpClientFactoryMock.Verify(x => x.CreateClient("apiSR"), Times.Once);
        }

        [Fact]
        public async Task Executar_DeveDisposeHttpClientAoFinalizar()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, "dados");
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.Executar(filtros);

            resultado.Should().NotBeNull();
        }

        [Fact]
        public async Task Executar_DeveRetornarStreamAoInvesDeReadAsStringAsync()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();
            ConfigurarServicoAcervo();

            var conteudoBinario = new byte[] { 0x1F, 0x8B, 0x08, 0x00 }; // Exemplo de conteúdo gzip
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(conteudoBinario)
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };
            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.Executar(filtros);

            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<MemoryStream>();
        }

        [Fact]
        public async Task Executar_DeveEnviarPostRequestAoEndpointCorreto()
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
                    Content = new StringContent("dados", Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };
            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            await _useCase.Executar(filtros);

            capturedRequest.Should().NotBeNull();
            capturedRequest.Method.Should().Be(HttpMethod.Post);
            capturedRequest.RequestUri!.ToString().Should().Contain(ApiEndpoint);
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
                .Returns(new long[] { 1 });
        }

        private static RelatorioControleDevolucaoLivrosRequest GerarFiltrosValidos() =>
            new Faker<RelatorioControleDevolucaoLivrosRequest>()
                .CustomInstantiator(f => new RelatorioControleDevolucaoLivrosRequest
                {
                    Solicitante = f.Person.FullName,
                    SomenteEmAtraso = f.Random.Bool()
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
