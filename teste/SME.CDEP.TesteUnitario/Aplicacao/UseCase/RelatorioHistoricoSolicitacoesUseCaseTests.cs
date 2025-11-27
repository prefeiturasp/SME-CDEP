using Bogus;
using Moq;
using Moq.AutoMock;
using Moq.Protected;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.UseCase;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Infra.Dominio.Enumerados;
using System.Net;
using System.Text;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase
{
    public class RelatorioHistoricoSolicitacoesUseCaseTests
    {
        private readonly AutoMocker _mocker;
        private readonly RelatorioHistoricoSolicitacoesUseCase _useCase;

        public RelatorioHistoricoSolicitacoesUseCaseTests()
        {
            _mocker = new AutoMocker();
            _useCase = _mocker.CreateInstance<RelatorioHistoricoSolicitacoesUseCase>();
        }

        [Fact]
        public async Task DadoDadosValidos_QuandoApiExternaRetornarSucesso_EntaoDeveRetornarStream()
        {
            // Arrange
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();

            var conteudoRetorno = "conteudo binario simulado";
            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.OK, conteudoRetorno);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri("http://api-simulada") };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            // Act
            var resultado = await _useCase.ExecutarAsync(filtros);

            // Assert
            Assert.NotNull(resultado);
            Assert.IsType<Stream>(resultado, exactMatch: false);
        }

        [Fact]
        public async Task DadoDadosValidos_QuandoApiExternaRetornarNoContent_EntaoDeveRetornarNulo()
        {
            // Arrange
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();

            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.NoContent);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri("http://api-simulada") };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            // Act
            var resultado = await _useCase.ExecutarAsync(filtros);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public async Task DadoDadosValidos_QuandoApiExternaRetornarErro_EntaoDeveRetornarNulo()
        {
            // Arrange
            var filtros = GerarFiltrosValidos();
            ConfigurarContextoUsuario();

            // Simula um erro 500
            var httpMessageHandlerMock = CriarHttpMessageHandlerMock(HttpStatusCode.InternalServerError);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri("http://api-simulada") };

            _mocker.GetMock<IHttpClientFactory>()
                .Setup(x => x.CreateClient("apiSR"))
                .Returns(httpClient);

            // Act
            var resultado = await _useCase.ExecutarAsync(filtros);

            // Assert
            Assert.Null(resultado);
        }

        // Métodos Auxiliares

        private void ConfigurarContextoUsuario()
        {
            var contextoMock = _mocker.GetMock<IContextoAplicacao>();
            contextoMock.Setup(c => c.NomeUsuario).Returns("João da Silva");
            contextoMock.Setup(c => c.UsuarioLogado).Returns("d123456");
        }

        private RelatorioHistoricoSolicitacoesRequest GerarFiltrosValidos() =>
            // O record não tem construtor vazio, usando instantiator
            new Faker<RelatorioHistoricoSolicitacoesRequest>()
                .CustomInstantiator(f => new RelatorioHistoricoSolicitacoesRequest(
                    f.Person.FullName,
                    f.Date.Past(),
                    f.Date.Future(),
                    [TipoAcervo.Bibliografico],
                    [SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO]
                ))
                .Generate();

        private Mock<HttpMessageHandler> CriarHttpMessageHandlerMock(HttpStatusCode statusCode, string content = "")
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
