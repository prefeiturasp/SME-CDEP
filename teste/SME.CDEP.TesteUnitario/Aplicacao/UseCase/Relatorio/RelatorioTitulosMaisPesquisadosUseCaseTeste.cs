using FluentAssertions;
using Moq;
using Moq.Protected;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.UseCase;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Infra.Dominio.Enumerados;
using System.Net;
using System.Text;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase.Relatorio
{
    public class RelatorioTitulosMaisPesquisadosUseCaseTeste
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IContextoAplicacao> _contextoAplicacaoMock;
        private readonly RelatorioTitulosMaisPesquisadosUseCase _useCase;
        private readonly List<TipoAcervo> _tipoAcervos = [TipoAcervo.Bibliografico];
        private const string BaseAddress = "https://api.example.com/";
        private const string ApiEndpoint = "v1/cdep/titulos-mais-pesquisados";

        public RelatorioTitulosMaisPesquisadosUseCaseTeste()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _contextoAplicacaoMock = new Mock<IContextoAplicacao>();
            _useCase = new RelatorioTitulosMaisPesquisadosUseCase(_httpClientFactoryMock.Object, _contextoAplicacaoMock.Object);
        }

        [Fact]
        public async Task ExecutarAsync_ComDadosValidos_DeveRetornarStream()
        {
            var dataInicio = DateTime.Now.AddDays(-30);
            var dataFim = DateTime.Now;
            var nomeUsuario = "João Silva";
            var usuarioRF = "123456";

            var filtros = new RelatorioTitulosMaisPesquisadosRequest
            {
                DataInicio = dataInicio,
                DataFim = dataFim,
                TipoAcervos = _tipoAcervos
            };

            _contextoAplicacaoMock.Setup(x => x.NomeUsuario).Returns(nomeUsuario);
            _contextoAplicacaoMock.Setup(x => x.UsuarioLogado).Returns(usuarioRF);

            var handlerMock = new Mock<HttpMessageHandler>();
            var streamContent = new MemoryStream(Encoding.UTF8.GetBytes("dados do relatório"));

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(streamContent)
                });

            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri(BaseAddress) };
            _httpClientFactoryMock.Setup(x => x.CreateClient("apiSR")).Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<MemoryStream>();
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task ExecutarAsync_ComStatusCodeErro_DeveRetornarNull()
        {
            var dataInicio = DateTime.Now.AddDays(-30);
            var dataFim = DateTime.Now;
            var nomeUsuario = "Maria Santos";
            var usuarioRF = "654321";

            var filtros = new RelatorioTitulosMaisPesquisadosRequest
            {
                DataInicio = dataInicio,
                DataFim = dataFim,
                TipoAcervos = _tipoAcervos
            };

            _contextoAplicacaoMock.Setup(x => x.NomeUsuario).Returns(nomeUsuario);
            _contextoAplicacaoMock.Setup(x => x.UsuarioLogado).Returns(usuarioRF);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));

            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri(BaseAddress) };
            _httpClientFactoryMock.Setup(x => x.CreateClient("apiSR")).Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task ExecutarAsync_ComStatusCodeNoContent_DeveRetornarNull()
        {
            var dataInicio = DateTime.Now.AddDays(-30);
            var dataFim = DateTime.Now;
            var nomeUsuario = "Pedro Oliveira";
            var usuarioRF = "789012";

            var filtros = new RelatorioTitulosMaisPesquisadosRequest
            {
                DataInicio = dataInicio,
                DataFim = dataFim,
                TipoAcervos = _tipoAcervos
            };

            _contextoAplicacaoMock.Setup(x => x.NomeUsuario).Returns(nomeUsuario);
            _contextoAplicacaoMock.Setup(x => x.UsuarioLogado).Returns(usuarioRF);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NoContent));

            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri(BaseAddress) };
            _httpClientFactoryMock.Setup(x => x.CreateClient("apiSR")).Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task ExecutarAsync_DeveEnviarFiltrosCorretos()
        {
            var dataInicio = new DateTime(2024, 01, 01);
            var dataFim = new DateTime(2024, 12, 31);
            var nomeUsuario = "Ana Costa";
            var usuarioRF = "345678";

            var filtros = new RelatorioTitulosMaisPesquisadosRequest
            {
                DataInicio = dataInicio,
                DataFim = dataFim,
                TipoAcervos = _tipoAcervos
            };

            _contextoAplicacaoMock.Setup(x => x.NomeUsuario).Returns(nomeUsuario);
            _contextoAplicacaoMock.Setup(x => x.UsuarioLogado).Returns(usuarioRF);

            var handlerMock = new Mock<HttpMessageHandler>();
            var streamContent = new MemoryStream(Encoding.UTF8.GetBytes("dados"));

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(streamContent)
                });

            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri(BaseAddress) };
            _httpClientFactoryMock.Setup(x => x.CreateClient("apiSR")).Returns(httpClient);

            await _useCase.ExecutarAsync(filtros);

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri!.ToString().Contains(ApiEndpoint)),
                ItExpr.IsAny<CancellationToken>());

            _contextoAplicacaoMock.Verify(x => x.NomeUsuario, Times.Once);
            _contextoAplicacaoMock.Verify(x => x.UsuarioLogado, Times.Once);
        }

        [Fact]
        public async Task ExecutarAsync_DeveSerializarMensagemCorretamente()
        {
            var dataInicio = new DateTime(2024, 06, 15);
            var dataFim = new DateTime(2024, 06, 20);
            var nomeUsuario = "Carlos Mendes";
            var usuarioRF = "901234";

            var filtros = new RelatorioTitulosMaisPesquisadosRequest
            {
                DataInicio = dataInicio,
                DataFim = dataFim,
                TipoAcervos = _tipoAcervos
            };

            _contextoAplicacaoMock.Setup(x => x.NomeUsuario).Returns(nomeUsuario);
            _contextoAplicacaoMock.Setup(x => x.UsuarioLogado).Returns(usuarioRF);

            var handlerMock = new Mock<HttpMessageHandler>();
            var streamContent = new MemoryStream(Encoding.UTF8.GetBytes("relatório"));
            HttpRequestMessage capturedRequest = null!;

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((request, token) =>
                {
                    capturedRequest = request;
                })
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StreamContent(streamContent)
                });

            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri(BaseAddress) };
            _httpClientFactoryMock.Setup(x => x.CreateClient("apiSR")).Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
            capturedRequest.Should().NotBeNull();
            capturedRequest.Content.Should().NotBeNull();
            capturedRequest.Content.Headers.ContentType!.MediaType.Should().Be("application/json");
        }

        [Fact]
        public async Task ExecutarAsync_ComMultiplosTiposAcervos_DeveProcessarCorretamente()
        {
            var dataInicio = DateTime.Now.AddDays(-60);
            var dataFim = DateTime.Now;
            var nomeUsuario = "Fernanda Souza";
            var usuarioRF = "222222";
            var multiplosTipos = new List<TipoAcervo> { TipoAcervo.Bibliografico, TipoAcervo.Audiovisual };

            var filtros = new RelatorioTitulosMaisPesquisadosRequest
            {
                DataInicio = dataInicio,
                DataFim = dataFim,
                TipoAcervos = multiplosTipos
            };

            _contextoAplicacaoMock.Setup(x => x.NomeUsuario).Returns(nomeUsuario);
            _contextoAplicacaoMock.Setup(x => x.UsuarioLogado).Returns(usuarioRF);

            var handlerMock = new Mock<HttpMessageHandler>();
            var streamContent = new MemoryStream(Encoding.UTF8.GetBytes("relatório múltiplo"));

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(streamContent)
                });

            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri(BaseAddress) };
            _httpClientFactoryMock.Setup(x => x.CreateClient("apiSR")).Returns(httpClient);

            var resultado = await _useCase.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}
