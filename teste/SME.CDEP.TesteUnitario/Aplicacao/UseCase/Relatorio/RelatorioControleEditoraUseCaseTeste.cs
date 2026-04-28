using Moq;
using Moq.Protected;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Aplicacao.UseCase;
using SME.CDEP.Dominio.Contexto;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase.Relatorio
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
            var json = JObject.Parse(corpoRequisicao);
            var mensagem = json["Mensagem"] as JObject;
            Assert.NotNull(mensagem);
            Assert.Equal(99, mensagem["EditoraId"]?[0]?.Value<int>());
            Assert.Equal("Usuário Teste", mensagem["Usuario"]?.Value<string>());
            Assert.Equal("123456", mensagem["UsuarioRF"]?.Value<string>());
            Assert.Equal(new[] { 1L, 2L, 3L }, mensagem["TiposAcervosPermitidos"]?.Values<long>());
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Servico_Acervo_Para_Obter_Tipos_Permitidos()
        {
            var httpClient = CriarHttpClient(HttpStatusCode.OK, "conteudo");
            httpClientFactoryMock.Setup(x => x.CreateClient("apiSR")).Returns(httpClient);

            var useCase = new RelatorioControleEditoraUseCase(httpClientFactoryMock.Object, servicoAcervoMock.Object, contextoAplicacaoMock.Object);

            var request = new RelatorioControleEditoraRequest();

            await useCase.Executar(request);

            servicoAcervoMock.Verify(x => x.ObterTiposAcervosPermitidosDoPerfilLogado(), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Chamar_HttpClientFactory_Com_Nome_apiSR()
        {
            var httpClient = CriarHttpClient(HttpStatusCode.OK, "conteudo");
            httpClientFactoryMock.Setup(x => x.CreateClient("apiSR")).Returns(httpClient);

            var useCase = new RelatorioControleEditoraUseCase(httpClientFactoryMock.Object, servicoAcervoMock.Object, contextoAplicacaoMock.Object);

            var request = new RelatorioControleEditoraRequest();

            await useCase.Executar(request);

            httpClientFactoryMock.Verify(x => x.CreateClient("apiSR"), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Fazer_Post_Para_Rota_v1_cdep_controle_editora()
        {
            string? uriRequisitada = null;
            string? metodoHttpUsado = null;

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Callback<HttpRequestMessage, CancellationToken>((req, _) =>
                {
                    uriRequisitada = req.RequestUri?.ToString();
                    metodoHttpUsado = req.Method.ToString();
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

            var useCase = new RelatorioControleEditoraUseCase(httpClientFactoryMock.Object, servicoAcervoMock.Object, contextoAplicacaoMock.Object);

            await useCase.Executar(new RelatorioControleEditoraRequest());

            Assert.NotNull(uriRequisitada);
            Assert.Contains("v1/cdep/controle-editora", uriRequisitada);
            Assert.Equal("POST", metodoHttpUsado);
        }

        [Fact]
        public async Task Executar_Deve_Incluir_EditoraId_No_Json_Quando_Fornecido()
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

            var useCase = new RelatorioControleEditoraUseCase(httpClientFactoryMock.Object, servicoAcervoMock.Object, contextoAplicacaoMock.Object);

            var editoraIds = new List<int> { 5, 10, 15 };
            var request = new RelatorioControleEditoraRequest { EditoraId = editoraIds };

            await useCase.Executar(request);

            Assert.NotNull(corpoRequisicao);
            var json = JObject.Parse(corpoRequisicao);
            var mensagem = json["Mensagem"];
            Assert.NotNull(mensagem);
            var editoraIdArray = mensagem["EditoraId"]?.Values<int>().ToList();
            Assert.NotNull(editoraIdArray);
            Assert.Equal(new[] { 5, 10, 15 }, editoraIdArray);
        }

        [Fact]
        public async Task Executar_Deve_Incluir_Usuario_No_Json()
        {
            string? corpoRequisicao = null;
            var nomeUsuarioEsperado = "João da Silva";

            contextoAplicacaoMock.SetupGet(x => x.NomeUsuario).Returns(nomeUsuarioEsperado);

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

            var useCase = new RelatorioControleEditoraUseCase(httpClientFactoryMock.Object, servicoAcervoMock.Object, contextoAplicacaoMock.Object);

            await useCase.Executar(new RelatorioControleEditoraRequest());

            Assert.NotNull(corpoRequisicao);
            var json = JObject.Parse(corpoRequisicao);
            var usuario = json["Mensagem"]?["Usuario"]?.Value<string>();
            Assert.Equal(nomeUsuarioEsperado, usuario);
        }

        [Fact]
        public async Task Executar_Deve_Incluir_UsuarioRF_No_Json()
        {
            string? corpoRequisicao = null;
            var usuarioRFEsperado = "9999999";

            contextoAplicacaoMock.SetupGet(x => x.UsuarioLogado).Returns(usuarioRFEsperado);

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

            var useCase = new RelatorioControleEditoraUseCase(httpClientFactoryMock.Object, servicoAcervoMock.Object, contextoAplicacaoMock.Object);

            await useCase.Executar(new RelatorioControleEditoraRequest());

            Assert.NotNull(corpoRequisicao);
            var json = JObject.Parse(corpoRequisicao);
            var usuarioRF = json["Mensagem"]?["UsuarioRF"]?.Value<string>();
            Assert.Equal(usuarioRFEsperado, usuarioRF);
        }

        [Fact]
        public async Task Executar_Deve_Incluir_TiposAcervosPermitidos_No_Json()
        {
            string? corpoRequisicao = null;
            var tiposEsperados = new long[] { 10, 20, 30, 40 };

            servicoAcervoMock.Setup(x => x.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(tiposEsperados);

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

            var useCase = new RelatorioControleEditoraUseCase(httpClientFactoryMock.Object, servicoAcervoMock.Object, contextoAplicacaoMock.Object);

            await useCase.Executar(new RelatorioControleEditoraRequest());

            Assert.NotNull(corpoRequisicao);
            var json = JObject.Parse(corpoRequisicao);
            var tipos = json["Mensagem"]?["TiposAcervosPermitidos"]?.Values<long>().ToList();
            Assert.NotNull(tipos);
            Assert.Equal(tiposEsperados, tipos);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Null_Quando_Status_Code_For_NoContent_Mesmo_Com_IsSuccessStatusCode_True()
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
                    StatusCode = HttpStatusCode.NoContent,
                    Content = new StringContent("", Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };
            httpClientFactoryMock.Setup(x => x.CreateClient("apiSR")).Returns(httpClient);

            var useCase = new RelatorioControleEditoraUseCase(httpClientFactoryMock.Object, servicoAcervoMock.Object, contextoAplicacaoMock.Object);

            var result = await useCase.Executar(new RelatorioControleEditoraRequest());

            Assert.Null(result);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Null_Para_Diferentes_Codigos_Erro_Http()
        {
            var codigosErro = new[]
            {
                HttpStatusCode.BadRequest,
                HttpStatusCode.Unauthorized,
                HttpStatusCode.Forbidden,
                HttpStatusCode.NotFound,
                HttpStatusCode.InternalServerError
            };

            foreach (var codigoErro in codigosErro)
            {
                var httpClient = CriarHttpClient(codigoErro);
                httpClientFactoryMock.Setup(x => x.CreateClient("apiSR")).Returns(httpClient);

                var useCase = new RelatorioControleEditoraUseCase(httpClientFactoryMock.Object, servicoAcervoMock.Object, contextoAplicacaoMock.Object);

                var result = await useCase.Executar(new RelatorioControleEditoraRequest());

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Stream_Para_Diferentes_Codigos_Sucesso_Http()
        {
            var codigosSucesso = new[]
            {
                HttpStatusCode.OK,
                HttpStatusCode.Created,
                HttpStatusCode.Accepted
            };

            foreach (var codigoSucesso in codigosSucesso)
            {
                var httpClient = CriarHttpClient(codigoSucesso, "conteudo relatorio");
                httpClientFactoryMock.Setup(x => x.CreateClient("apiSR")).Returns(httpClient);

                var useCase = new RelatorioControleEditoraUseCase(httpClientFactoryMock.Object, servicoAcervoMock.Object, contextoAplicacaoMock.Object);

                var result = await useCase.Executar(new RelatorioControleEditoraRequest());

                Assert.NotNull(result);
                using var reader = new StreamReader(result);
                var texto = await reader.ReadToEndAsync();
                Assert.Equal("conteudo relatorio", texto);
            }
        }

        [Fact]
        public async Task Executar_Deve_Encapsular_Filtros_Em_Propriedade_Mensagem()
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

            var useCase = new RelatorioControleEditoraUseCase(httpClientFactoryMock.Object, servicoAcervoMock.Object, contextoAplicacaoMock.Object);

            await useCase.Executar(new RelatorioControleEditoraRequest());

            Assert.NotNull(corpoRequisicao);
            var json = JObject.Parse(corpoRequisicao);
            Assert.NotNull(json["Mensagem"]);
            Assert.True(json["Mensagem"] is JObject);
        }
    }
}
