using Moq;
using Moq.Protected;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Aplicacao.UseCase;
using SME.CDEP.Dominio.Contexto;
using System.Net;
using System.Text;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase
{
    public class RelatorioControleEditoraUseCaseTeste
    {
        private readonly Mock<IHttpClientFactory> httpClientFactoryMock;
        private readonly Mock<IServicoAcervo> servicoAcervoMock;
        private readonly Mock<IContextoAplicacao> contextoAplicacaoMock;

        public RelatorioControleEditoraUseCaseTeste()
        {
            httpClientFactoryMock = new Mock<IHttpClientFactory>();
            servicoAcervoMock = new Mock<IServicoAcervo>();
            contextoAplicacaoMock = new Mock<IContextoAplicacao>();

            contextoAplicacaoMock.SetupGet(x => x.NomeUsuario).Returns("Usuário Teste");
            contextoAplicacaoMock.SetupGet(x => x.UsuarioLogado).Returns("123456");
            servicoAcervoMock.Setup(x => x.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(new long[] { 1, 2, 3 });
        }

        private HttpClient CriarHttpClient(HttpStatusCode statusCode, string content = "arquivo fake")
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content, Encoding.UTF8, "application/json")
                });

            return new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Stream_Quando_Resposta_For_Sucesso()
        {
            var httpClient = CriarHttpClient(HttpStatusCode.OK, "conteudo do relatorio");
            httpClientFactoryMock.Setup(x => x.CreateClient("apiSR")).Returns(httpClient);

            var useCase = new RelatorioControleEditoraUseCase(httpClientFactoryMock.Object, servicoAcervoMock.Object, contextoAplicacaoMock.Object);

            var request = new RelatorioControleEditoraRequest { EditoraId = new List<int> { 10 } };

            var result = await useCase.Executar(request);

            Assert.NotNull(result);
            using var reader = new StreamReader(result);
            var texto = await reader.ReadToEndAsync();
            Assert.Contains("conteudo do relatorio", texto);
        }

        [Fact]
        public async Task Executar_Deve_RetornarNull_Quando_Resposta_For_No_Content()
        {
            var httpClient = CriarHttpClient(HttpStatusCode.NoContent);
            httpClientFactoryMock.Setup(x => x.CreateClient("apiSR")).Returns(httpClient);

            var useCase = new RelatorioControleEditoraUseCase(httpClientFactoryMock.Object, servicoAcervoMock.Object, contextoAplicacaoMock.Object);

            var request = new RelatorioControleEditoraRequest();

            var result = await useCase.Executar(request);

            Assert.Null(result);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Null_Quando_Resposta_Nao_For_Sucesso()
        {
            var httpClient = CriarHttpClient(HttpStatusCode.BadRequest);
            httpClientFactoryMock.Setup(x => x.CreateClient("apiSR")).Returns(httpClient);

            var useCase = new RelatorioControleEditoraUseCase(httpClientFactoryMock.Object, servicoAcervoMock.Object, contextoAplicacaoMock.Object);

            var request = new RelatorioControleEditoraRequest();

            var result = await useCase.Executar(request);

            Assert.Null(result);
        }

        [Fact]
        public async Task Executar_Deve_Montar_Json_Com_Filtros_Corretos()
        {
            string? corpoRequisicao = null;

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Callback<HttpRequestMessage, CancellationToken>(async (req, _) =>
                {
                    corpoRequisicao = await req.Content.ReadAsStringAsync();
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{}", Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };
            httpClientFactoryMock.Setup(x => x.CreateClient("apiSR")).Returns(httpClient);

            var useCase = new RelatorioControleEditoraUseCase(
                httpClientFactoryMock.Object,
                servicoAcervoMock.Object,
                contextoAplicacaoMock.Object
            );

            var request = new RelatorioControleEditoraRequest { EditoraId = new List<int> { 99 } };

            await useCase.Executar(request);

            Assert.NotNull(corpoRequisicao);
            Assert.Contains("\"EditoraId\":[99]", corpoRequisicao);
            Assert.Contains("\"Usuario\":\"Usuário Teste\"", corpoRequisicao);
            Assert.Contains("\"UsuarioRF\":\"123456\"", corpoRequisicao);
            Assert.Contains("\"TiposAcervosPermitidos\":[1,2,3]", corpoRequisicao);
        }
    }
}
