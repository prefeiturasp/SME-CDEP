using Bogus;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Moq.Protected;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.UseCase;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Infra.Dominio.Enumerados;
using System.Net;
using System.Text;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase.Relatorio
{
    public class RelatorioControleDownloadAcervoUseCaseTeste
    {
        private readonly Mock<IHttpClientFactory> httpClientFactoryMock;
        private readonly Mock<IContextoAplicacao> contextoAplicacaoMock;
        private readonly RelatorioControleDownloadAcervoUseCase sut;
        private const string BaseUrlMock = "http://localhost/";

        public RelatorioControleDownloadAcervoUseCaseTeste()
        {
            var mocker = new AutoMocker();

            httpClientFactoryMock = mocker.GetMock<IHttpClientFactory>();
            contextoAplicacaoMock = mocker.GetMock<IContextoAplicacao>();

            sut = mocker.CreateInstance<RelatorioControleDownloadAcervoUseCase>();
        }

        #region Testes de Sucesso

        [Fact]
        public async Task DadoFiltrosValidos_QuandoExecutarAsync_EntaoRetornaStreamComDados()
        {
            // Arrange
            var faker = new Faker("pt_BR");
            var filtros = new RelatorioControleDownloadAcervoRequest
            {
                TipoAcervo = TipoAcervo.Bibliografico,
                Titulo = faker.Commerce.ProductName()
            };

            var nomeUsuario = faker.Name.FullName();
            var usuarioRF = faker.Random.Replace("########");

            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns(nomeUsuario);
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns(usuarioRF);

            var bytesArquivo = Encoding.UTF8.GetBytes("dados do relatório em formato xlsx");

            MockarRespostaHttpClientComSucesso(HttpStatusCode.OK, bytesArquivo);

            // Act
            var resultado = await sut.ExecutarAsync(filtros);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<MemoryStream>();
            resultado.Length.Should().Be(bytesArquivo.Length);

            VerificarChamadaHttpPost();
        }

        [Fact]
        public async Task DadoRetornoComStatusOk_QuandoExecutarAsync_EntaoRetornaStreamValido()
        {
            var filtros = new Faker<RelatorioControleDownloadAcervoRequest>().Generate();
            var nomeUsuario = "Usuario Teste";
            var usuarioRF = "1234567";

            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns(nomeUsuario);
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns(usuarioRF);

            var bytesRelatorio = Encoding.UTF8.GetBytes("PK\x03\x04");
            MockarRespostaHttpClientComSucesso(HttpStatusCode.OK, bytesRelatorio);

            var resultado = await sut.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<MemoryStream>();
            resultado.Seek(0, SeekOrigin.Begin);
            var bytesLidos = new byte[resultado.Length];
            await resultado.ReadExactlyAsync(bytesLidos);
            bytesLidos.Should().BeEquivalentTo(bytesRelatorio);
        }

        [Fact]
        public async Task DadoFiltrosComTipoAcervo_QuandoExecutarAsync_EntaoEnviaRequestComTipoAcervo()
        {
            var tipoAcervo = TipoAcervo.Fotografico;
            var filtros = new RelatorioControleDownloadAcervoRequest
            {
                TipoAcervo = tipoAcervo,
                Titulo = "Título de Teste"
            };

            var nomeUsuario = "João Silva";
            var usuarioRF = "9876543";

            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns(nomeUsuario);
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns(usuarioRF);

            MockarRespostaHttpClientComSucesso(HttpStatusCode.OK, new byte[] { 1, 2, 3 });

            await sut.ExecutarAsync(filtros);

            VerificarChamadaHttpPost();
        }

        [Fact]
        public async Task DadoFiltrosComTitulo_QuandoExecutarAsync_EntaoEnviaRequestComTitulo()
        {
            var titulo = "Livro Importante";
            var filtros = new RelatorioControleDownloadAcervoRequest
            {
                TipoAcervo = TipoAcervo.Bibliografico,
                Titulo = titulo
            };

            var nomeUsuario = "Maria Santos";
            var usuarioRF = "5555555";

            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns(nomeUsuario);
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns(usuarioRF);

            MockarRespostaHttpClientComSucesso(HttpStatusCode.OK, new byte[] { 4, 5, 6 });

            await sut.ExecutarAsync(filtros);

            VerificarChamadaHttpPost();
        }

        [Fact]
        public async Task DadoFiltrosSemTitulo_QuandoExecutarAsync_EntaoEnviaRequestComTituloNulo()
        {
            var filtros = new RelatorioControleDownloadAcervoRequest
            {
                TipoAcervo = TipoAcervo.Bibliografico,
                Titulo = null
            };

            var nomeUsuario = "Pedro Oliveira";
            var usuarioRF = "1111111";

            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns(nomeUsuario);
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns(usuarioRF);

            MockarRespostaHttpClientComSucesso(HttpStatusCode.OK, new byte[] { 7, 8, 9 });

            await sut.ExecutarAsync(filtros);

            VerificarChamadaHttpPost();
        }

        [Fact]
        public async Task DadoFiltrosSemTipoAcervo_QuandoExecutarAsync_EntaoEnviaRequestComTipoAcervoNulo()
        {
            var filtros = new RelatorioControleDownloadAcervoRequest
            {
                TipoAcervo = null,
                Titulo = "Algum Título"
            };

            var nomeUsuario = "Ana Costa";
            var usuarioRF = "2222222";

            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns(nomeUsuario);
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns(usuarioRF);

            MockarRespostaHttpClientComSucesso(HttpStatusCode.OK, new byte[] { 10, 11, 12 });

            await sut.ExecutarAsync(filtros);

            VerificarChamadaHttpPost();
        }

        [Fact]
        public async Task DadoContextoComNomeUsuarioVazio_QuandoExecutarAsync_EntaoEnviaRequestComNomeUsuarioVazio()
        {
            var filtros = new Faker<RelatorioControleDownloadAcervoRequest>().Generate();
            var nomeUsuario = string.Empty;
            var usuarioRF = "3333333";

            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns(nomeUsuario);
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns(usuarioRF);

            MockarRespostaHttpClientComSucesso(HttpStatusCode.OK, new byte[] { 13, 14, 15 });

            await sut.ExecutarAsync(filtros);

            VerificarChamadaHttpPost();
        }

        [Fact]
        public async Task DadoArquivoComTamanhoPequeno_QuandoExecutarAsync_EntaoRetornaStreamComDadosCorretos()
        {
            var filtros = new Faker<RelatorioControleDownloadAcervoRequest>().Generate();
            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns("User");
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns("1234567");

            var bytesArquivo = new byte[] { 1 };
            MockarRespostaHttpClientComSucesso(HttpStatusCode.OK, bytesArquivo);

            var resultado = await sut.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
            resultado.Length.Should().Be(1);
        }

        [Fact]
        public async Task DadoArquivoComTamanhoGrande_QuandoExecutarAsync_EntaoRetornaStreamCompleto()
        {
            var filtros = new Faker<RelatorioControleDownloadAcervoRequest>().Generate();
            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns("User");
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns("1234567");

            var bytesArquivo = new byte[10000];
            new Random().NextBytes(bytesArquivo);
            MockarRespostaHttpClientComSucesso(HttpStatusCode.OK, bytesArquivo);

            var resultado = await sut.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
            resultado.Length.Should().Be(10000);
        }

        #endregion

        #region Testes de Falha - Status Code Não Sucesso

        [Fact]
        public async Task DadoStatusCodeNotFound_QuandoExecutarAsync_EntaoRetornaNulo()
        {
            var filtros = new Faker<RelatorioControleDownloadAcervoRequest>().Generate();
            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns("User");
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns("1234567");

            MockarRespostaHttpClientComFalha(HttpStatusCode.NotFound, new byte[] { });

            var resultado = await sut.ExecutarAsync(filtros);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DadoStatusCodeBadRequest_QuandoExecutarAsync_EntaoRetornaNulo()
        {
            var filtros = new Faker<RelatorioControleDownloadAcervoRequest>().Generate();
            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns("User");
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns("1234567");

            MockarRespostaHttpClientComFalha(HttpStatusCode.BadRequest, new byte[] { });

            var resultado = await sut.ExecutarAsync(filtros);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DadoStatusCodeServerError_QuandoExecutarAsync_EntaoRetornaNulo()
        {
            var filtros = new Faker<RelatorioControleDownloadAcervoRequest>().Generate();
            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns("User");
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns("1234567");

            MockarRespostaHttpClientComFalha(HttpStatusCode.InternalServerError, new byte[] { });

            var resultado = await sut.ExecutarAsync(filtros);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DadoStatusCodeUnauthorized_QuandoExecutarAsync_EntaoRetornaNulo()
        {
            var filtros = new Faker<RelatorioControleDownloadAcervoRequest>().Generate();
            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns("User");
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns("1234567");

            MockarRespostaHttpClientComFalha(HttpStatusCode.Unauthorized, new byte[] { });

            var resultado = await sut.ExecutarAsync(filtros);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DadoStatusCodeForbidden_QuandoExecutarAsync_EntaoRetornaNulo()
        {
            var filtros = new Faker<RelatorioControleDownloadAcervoRequest>().Generate();
            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns("User");
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns("1234567");

            MockarRespostaHttpClientComFalha(HttpStatusCode.Forbidden, new byte[] { });

            var resultado = await sut.ExecutarAsync(filtros);

            resultado.Should().BeNull();
        }

        #endregion

        #region Testes de Falha - NoContent

        [Fact]
        public async Task DadoStatusCodeNoContent_QuandoExecutarAsync_EntaoRetornaNulo()
        {
            var filtros = new Faker<RelatorioControleDownloadAcervoRequest>().Generate();
            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns("User");
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns("1234567");

            MockarRespostaHttpClientComSucesso(HttpStatusCode.NoContent, new byte[] { });

            var resultado = await sut.ExecutarAsync(filtros);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DadoStatusCodeNoContentComDados_QuandoExecutarAsync_EntaoRetornaNuloIgnorandoDados()
        {
            var filtros = new Faker<RelatorioControleDownloadAcervoRequest>().Generate();
            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns("User");
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns("1234567");

            MockarRespostaHttpClientComSucesso(HttpStatusCode.NoContent, new byte[] { 1, 2, 3 });

            var resultado = await sut.ExecutarAsync(filtros);

            resultado.Should().BeNull();
        }

        #endregion

        #region Testes de Integração - Serialização JSON

        [Fact]
        public async Task DadoFiltrosCompletos_QuandoExecutarAsync_EntaoSerializaJsonCorretamente()
        {
            var tipoAcervo = TipoAcervo.Bibliografico;
            var titulo = "Teste de Serializacao";
            var nomeUsuario = "Usuario Teste";
            var usuarioRF = "9999999";

            var filtros = new RelatorioControleDownloadAcervoRequest
            {
                TipoAcervo = tipoAcervo,
                Titulo = titulo
            };

            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns(nomeUsuario);
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns(usuarioRF);

            var jsonEnviado = string.Empty;
            MockarRespostaHttpClientComSerializacao(HttpStatusCode.OK, new byte[] { 1, 2, 3 }, json => jsonEnviado = json);

            await sut.ExecutarAsync(filtros);

            jsonEnviado.Should().NotBeNullOrEmpty();
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonEnviado)!;
            ((string)jsonObject.Mensagem.Usuario).Should().Be(nomeUsuario);
            ((string)jsonObject.Mensagem.UsuarioRF).Should().Be(usuarioRF);
            ((string)jsonObject.Mensagem.Titulo).Should().Be(titulo);
        }

        [Fact]
        public async Task DadoHttpClientFactoryCriandoClienteComNome_QuandoExecutarAsync_EntaoChamaCreateClientComApiSr()
        {
            var filtros = new Faker<RelatorioControleDownloadAcervoRequest>().Generate();
            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns("User");
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns("1234567");

            MockarRespostaHttpClientComSucesso(HttpStatusCode.OK, new byte[] { 1 });

            await sut.ExecutarAsync(filtros);

            httpClientFactoryMock.Verify(f => f.CreateClient("apiSR"), Times.Once);
        }

        [Fact]
        public async Task DadoPostAsyncChamado_QuandoExecutarAsync_EntaoChamaComUrlCorreta()
        {
            var filtros = new Faker<RelatorioControleDownloadAcervoRequest>().Generate();
            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns("User");
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns("1234567");

            var urlEnviada = string.Empty;
            MockarRespostaHttpClientComCapturacaoDeUrl(HttpStatusCode.OK, new byte[] { 1 }, url => urlEnviada = url);

            await sut.ExecutarAsync(filtros);

            urlEnviada.Should().Contain("v1/cdep/controle-download-acervo");
        }

        [Fact]
        public async Task DadoContentTypeDaRequisicao_QuandoExecutarAsync_EntaoChamaComApplicationJsonUtf8()
        {
            var filtros = new Faker<RelatorioControleDownloadAcervoRequest>().Generate();
            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns("User");
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns("1234567");

            var contentTypeEnviado = string.Empty;
            MockarRespostaHttpClientComCapturacaoDeContentType(HttpStatusCode.OK, new byte[] { 1 }, ct => contentTypeEnviado = ct);

            await sut.ExecutarAsync(filtros);

            contentTypeEnviado.Should().Be("application/json");
        }

        #endregion

        #region Testes de Status Codes Adicionais

        [Fact]
        public async Task DadoStatusCodeCreated_QuandoExecutarAsync_EntaoRetornaStream()
        {
            var filtros = new Faker<RelatorioControleDownloadAcervoRequest>().Generate();
            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns("User");
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns("1234567");

            MockarRespostaHttpClientComSucesso(HttpStatusCode.Created, new byte[] { 1, 2, 3 });

            var resultado = await sut.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
        }

        [Fact]
        public async Task DadoStatusCodeAccepted_QuandoExecutarAsync_EntaoRetornaStream()
        {
            var filtros = new Faker<RelatorioControleDownloadAcervoRequest>().Generate();
            contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns("User");
            contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns("1234567");

            MockarRespostaHttpClientComSucesso(HttpStatusCode.Accepted, new byte[] { 4, 5, 6 });

            var resultado = await sut.ExecutarAsync(filtros);

            resultado.Should().NotBeNull();
        }

        #endregion

        #region Métodos Auxiliares

        private void MockarRespostaHttpClientComSucesso(HttpStatusCode statusCode, byte[] conteudoRetorno)
        {
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new ByteArrayContent(conteudoRetorno)
                });

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri(BaseUrlMock)
            };

            httpClientFactoryMock
                .Setup(f => f.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);
        }

        private void MockarRespostaHttpClientComFalha(HttpStatusCode statusCode, byte[] conteudoRetorno)
        {            
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(() =>
                {
                    var response = new HttpResponseMessage
                    {
                        StatusCode = statusCode,
                        Content = new ByteArrayContent(conteudoRetorno)
                    };
                    // Adiciona um header customizado para diferenciar do método de sucesso
                    response.Headers.Add("X-Mock-Failure", "true");
                    return response;
                });

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri(BaseUrlMock)
            };

            httpClientFactoryMock
                .Setup(f => f.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);
        }

        private void MockarRespostaHttpClientComSerializacao(HttpStatusCode statusCode, byte[] conteudoRetorno, Action<string> capturaJson)
        {
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Callback<HttpRequestMessage, CancellationToken>(async (request, token) =>
                {
                    if (request.Content is StringContent stringContent)
                    {
                        var jsonContent = await stringContent.ReadAsStringAsync(token);
                        capturaJson(jsonContent);
                    }
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new ByteArrayContent(conteudoRetorno)
                });

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri(BaseUrlMock)
            };

            httpClientFactoryMock
                .Setup(f => f.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);
        }

        private void MockarRespostaHttpClientComCapturacaoDeUrl(HttpStatusCode statusCode, byte[] conteudoRetorno, Action<string> capturaUrl)
        {
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Callback<HttpRequestMessage, CancellationToken>((request, token) =>
                {
                    capturaUrl(request.RequestUri?.ToString() ?? string.Empty);
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new ByteArrayContent(conteudoRetorno)
                });

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri(BaseUrlMock)
            };

            httpClientFactoryMock
                .Setup(f => f.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);
        }

        private void MockarRespostaHttpClientComCapturacaoDeContentType(HttpStatusCode statusCode, byte[] conteudoRetorno, Action<string> capturaContentType)
        {
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Callback<HttpRequestMessage, CancellationToken>((request, token) =>
                {
                    if (request.Content?.Headers.ContentType != null)
                    {
                        capturaContentType(request.Content.Headers.ContentType.MediaType!);
                    }
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new ByteArrayContent(conteudoRetorno)
                });

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri(BaseUrlMock)
            };

            httpClientFactoryMock
                .Setup(f => f.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);
        }

        private void VerificarChamadaHttpPost()
        {
            httpClientFactoryMock.Verify(f => f.CreateClient("apiSR"), Times.Once);

            contextoAplicacaoMock.Verify(c => c.NomeUsuario, Times.AtLeastOnce);
            contextoAplicacaoMock.Verify(c => c.UsuarioLogado, Times.AtLeastOnce);
        }

        #endregion
    }
}
