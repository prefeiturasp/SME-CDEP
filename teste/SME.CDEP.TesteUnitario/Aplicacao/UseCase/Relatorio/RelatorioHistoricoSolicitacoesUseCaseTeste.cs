using Bogus;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Moq.Protected;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.UseCase;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Infra.Dominio.Enumerados;
using System.Net;
using System.Text;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase.Relatorio
{
    public class RelatorioHistoricoSolicitacoesUseCaseTeste
    {
        private const string BaseAddress = "https://api.example.com/";
        private const string ApiEndpoint = "v1/cdep/historico-solicitacao-acervo";
        private readonly AutoMocker _mocker;
        private readonly RelatorioHistoricoSolicitacoesUseCase _useCase;

        public RelatorioHistoricoSolicitacoesUseCaseTeste()
        {
            _mocker = new AutoMocker();
            _useCase = _mocker.CreateInstance<RelatorioHistoricoSolicitacoesUseCase>();
        }

        [Fact]
        public async Task ExecutarAsync_ComDadosValidos_DeveRetornarStream()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();

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
            capturedRequest.Method.Should().Be(HttpMethod.Post);
            capturedRequest.RequestUri!.ToString().Should().Contain(ApiEndpoint);
            contextoMock.Verify(x => x.NomeUsuario, Times.Once);
            contextoMock.Verify(x => x.UsuarioLogado, Times.Once);
        }

        [Fact]
        public async Task ExecutarAsync_DeveSerializarPayloadCorretamente()
        {
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();

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
            var dataInicio = new DateTime(2024, 01, 01);
            var dataFim = new DateTime(2024, 12, 31);
            var solicitante = "Maria Silva";
            var tiposAcervo = new List<TipoAcervo> { TipoAcervo.Bibliografico, TipoAcervo.Audiovisual };
            var situacoes = new List<SituacaoSolicitacaoItem> 
            { 
                SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO, 
                SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE 
            };

            var filtros = new RelatorioHistoricoSolicitacoesRequest(
                solicitante,
                dataInicio,
                dataFim,
                tiposAcervo,
                situacoes);

            ConfigurarContextoUsuario();

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
            var filtros = new RelatorioHistoricoSolicitacoesRequest(
                null,
                DateTime.Now.AddDays(-30),
                DateTime.Now,
                null,
                null);

            ConfigurarContextoUsuario();

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

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.Forbidden);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(BaseAddress) };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().BeNull();
        }

        private void ConfigurarContextoUsuario()
        {
            var contextoMock = _mocker.GetMock<IContextoAplicacao>();
            contextoMock.Setup(c => c.NomeUsuario).Returns("João da Silva");
            contextoMock.Setup(c => c.UsuarioLogado).Returns("d123456");
        }

        private static RelatorioHistoricoSolicitacoesRequest GerarFiltrosValidos() =>
            new Faker<RelatorioHistoricoSolicitacoesRequest>()
                .CustomInstantiator(f => new RelatorioHistoricoSolicitacoesRequest(
                    f.Person.FullName,
                    f.Date.Past(),
                    f.Date.Future(),
                    [TipoAcervo.Bibliografico],
                    [SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO]
                ))
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
