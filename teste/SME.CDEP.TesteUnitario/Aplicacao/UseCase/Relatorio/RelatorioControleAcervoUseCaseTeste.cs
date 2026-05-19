using Moq;
using Moq.Protected;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Aplicacao.UseCase;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Enumerados;
using System.Net;
using System.Text;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase.Relatorio
{
    public class RelatorioControleAcervoUseCaseTeste
    {
        private readonly Mock<IHttpClientFactory> mockHttpClientFactory;
        private readonly Mock<IServicoAcervo> mockServicoAcervo;
        private readonly Mock<IContextoAplicacao> mockContextoAplicacao;
        private readonly RelatorioControleAcervoUseCase useCase;

        public RelatorioControleAcervoUseCaseTeste()
        {
            mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockServicoAcervo = new Mock<IServicoAcervo>();
            mockContextoAplicacao = new Mock<IContextoAplicacao>();

            useCase = new RelatorioControleAcervoUseCase(
                mockHttpClientFactory.Object,
                mockServicoAcervo.Object,
                mockContextoAplicacao.Object
            );
        }

        [Fact(DisplayName = "RelatorioControleAcervo - Executar com dados válidos deve retornar stream com sucesso")]
        public async Task Executar_ComDadosValidos_DeveRetornarStreamComSucesso()
        {
            // Arrange
            var filtros = new RelatorioControleAcervoRequest
            {
                SituacaoAcervo = SituacaoAcervo.Ativo,
                TipoAcervo = Infra.Dominio.Enumerados.TipoAcervo.Bibliografico
            };

            var tiposAcervosPermitidos = new long[] { 1, 2, 3 };
            mockServicoAcervo.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(tiposAcervosPermitidos);

            mockContextoAplicacao.Setup(c => c.NomeUsuario).Returns("João Silva");
            mockContextoAplicacao.Setup(c => c.UsuarioLogado).Returns("RF123456");

            var conteudoResposta = "Conteúdo do relatório";
            var streamContent = new MemoryStream(Encoding.UTF8.GetBytes(conteudoResposta));
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(streamContent)
            };

            var mockHandler = CriarMockHttpMessageHandler(response);
            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://api.example.com")
            };
            mockHttpClientFactory.Setup(f => f.CreateClient("apiSR")).Returns(httpClient);

            // Act
            var resultado = await useCase.Executar(filtros);

            // Assert
            Assert.NotNull(resultado);
            Assert.IsType<MemoryStream>(resultado);
            mockHttpClientFactory.Verify(f => f.CreateClient("apiSR"), Times.Once);
        }

        [Fact(DisplayName = "RelatorioControleAcervo - Deve serializar filtros corretamente")]
        public async Task Executar_ComDadosValidos_DeveSerializarFiltrosCorretamente()
        {
            // Arrange
            var filtros = new RelatorioControleAcervoRequest
            {
                SituacaoAcervo = SituacaoAcervo.Ativo,
                TipoAcervo = Infra.Dominio.Enumerados.TipoAcervo.Bibliografico
            };

            var tiposAcervosPermitidos = new long[] { 1, 2, 3 };
            mockServicoAcervo.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(tiposAcervosPermitidos);

            mockContextoAplicacao.Setup(c => c.NomeUsuario).Returns("João Silva");
            mockContextoAplicacao.Setup(c => c.UsuarioLogado).Returns("RF123456");

            var conteudoResposta = "Conteúdo do relatório";
            var streamContent = new MemoryStream(Encoding.UTF8.GetBytes(conteudoResposta));
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(streamContent)
            };

            var mockHandler = CriarMockHttpMessageHandler(response);
            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://api.example.com")
            };
            mockHttpClientFactory.Setup(f => f.CreateClient("apiSR")).Returns(httpClient);

            // Act
            await useCase.Executar(filtros);

            // Assert
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri!.ToString().EndsWith("v1/cdep/controle-acervo")),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact(DisplayName = "RelatorioControleAcervo - Com status code NoContent deve retornar null")]
        public async Task Executar_ComStatusCodeNoContent_DeveRetornarNull()
        {
            // Arrange
            var filtros = new RelatorioControleAcervoRequest
            {
                SituacaoAcervo = SituacaoAcervo.Ativo,
                TipoAcervo = Infra.Dominio.Enumerados.TipoAcervo.Bibliografico
            };

            var tiposAcervosPermitidos = new long[] { 1, 2, 3 };
            mockServicoAcervo.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(tiposAcervosPermitidos);

            mockContextoAplicacao.Setup(c => c.NomeUsuario).Returns("João Silva");
            mockContextoAplicacao.Setup(c => c.UsuarioLogado).Returns("RF123456");

            var response = new HttpResponseMessage(HttpStatusCode.NoContent);

            var mockHandler = CriarMockHttpMessageHandler(response);
            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://api.example.com")
            };
            mockHttpClientFactory.Setup(f => f.CreateClient("apiSR")).Returns(httpClient);

            // Act
            var resultado = await useCase.Executar(filtros);

            // Assert
            Assert.Null(resultado);
        }

        [Fact(DisplayName = "RelatorioControleAcervo - Com status code BadRequest deve retornar null")]
        public async Task Executar_ComStatusCodeBadRequest_DeveRetornarNull()
        {
            // Arrange
            var filtros = new RelatorioControleAcervoRequest
            {
                SituacaoAcervo = SituacaoAcervo.Ativo,
                TipoAcervo = Infra.Dominio.Enumerados.TipoAcervo.Bibliografico
            };

            var tiposAcervosPermitidos = new long[] { 1, 2, 3 };
            mockServicoAcervo.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(tiposAcervosPermitidos);

            mockContextoAplicacao.Setup(c => c.NomeUsuario).Returns("João Silva");
            mockContextoAplicacao.Setup(c => c.UsuarioLogado).Returns("RF123456");

            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var mockHandler = CriarMockHttpMessageHandler(response);
            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://api.example.com")
            };
            mockHttpClientFactory.Setup(f => f.CreateClient("apiSR")).Returns(httpClient);

            // Act
            var resultado = await useCase.Executar(filtros);

            // Assert
            Assert.Null(resultado);
        }

        [Fact(DisplayName = "RelatorioControleAcervo - Com status code Unauthorized deve retornar null")]
        public async Task Executar_ComStatusCodeUnauthorized_DeveRetornarNull()
        {
            // Arrange
            var filtros = new RelatorioControleAcervoRequest
            {
                SituacaoAcervo = SituacaoAcervo.Ativo,
                TipoAcervo = Infra.Dominio.Enumerados.TipoAcervo.Bibliografico
            };

            var tiposAcervosPermitidos = new long[] { 1, 2, 3 };
            mockServicoAcervo.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(tiposAcervosPermitidos);

            mockContextoAplicacao.Setup(c => c.NomeUsuario).Returns("João Silva");
            mockContextoAplicacao.Setup(c => c.UsuarioLogado).Returns("RF123456");

            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

            var mockHandler = CriarMockHttpMessageHandler(response);
            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://api.example.com")
            };
            mockHttpClientFactory.Setup(f => f.CreateClient("apiSR")).Returns(httpClient);

            // Act
            var resultado = await useCase.Executar(filtros);

            // Assert
            Assert.Null(resultado);
        }

        [Fact(DisplayName = "RelatorioControleAcervo - Deve usar nome usuário do contexto")]
        public async Task Executar_DeveUsarNomeUsuarioDoContexto()
        {
            // Arrange
            var filtros = new RelatorioControleAcervoRequest();
            var nomeUsuario = "Maria Santos";

            mockServicoAcervo.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(new long[] { 1 });

            mockContextoAplicacao.Setup(c => c.NomeUsuario).Returns(nomeUsuario);
            mockContextoAplicacao.Setup(c => c.UsuarioLogado).Returns("RF789");

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(new MemoryStream())
            };

            var mockHandler = CriarMockHttpMessageHandler(response);
            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://api.example.com")
            };
            mockHttpClientFactory.Setup(f => f.CreateClient("apiSR")).Returns(httpClient);

            // Act
            await useCase.Executar(filtros);

            // Assert
            mockContextoAplicacao.Verify(c => c.NomeUsuario, Times.Once);
        }

        [Fact(DisplayName = "RelatorioControleAcervo - Deve usar usuário logado do contexto")]
        public async Task Executar_DeveUsarUsuarioLogadoDoContexto()
        {
            // Arrange
            var filtros = new RelatorioControleAcervoRequest();
            var usuarioRF = "RF456";

            mockServicoAcervo.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(new long[] { 1 });

            mockContextoAplicacao.Setup(c => c.NomeUsuario).Returns("João");
            mockContextoAplicacao.Setup(c => c.UsuarioLogado).Returns(usuarioRF);

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(new MemoryStream())
            };

            var mockHandler = CriarMockHttpMessageHandler(response);
            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://api.example.com")
            };
            mockHttpClientFactory.Setup(f => f.CreateClient("apiSR")).Returns(httpClient);

            // Act
            await useCase.Executar(filtros);

            // Assert
            mockContextoAplicacao.Verify(c => c.UsuarioLogado, Times.Once);
        }

        [Fact(DisplayName = "RelatorioControleAcervo - Deve obter tipos acervos permitidos do perfil")]
        public async Task Executar_DeveObterTiposAcervosPermitidosDoPerfil()
        {
            // Arrange
            var filtros = new RelatorioControleAcervoRequest();
            var tiposPermitidos = new long[] { 10, 20, 30 };

            mockServicoAcervo.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(tiposPermitidos);

            mockContextoAplicacao.Setup(c => c.NomeUsuario).Returns("João");
            mockContextoAplicacao.Setup(c => c.UsuarioLogado).Returns("RF123");

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(new MemoryStream())
            };

            var mockHandler = CriarMockHttpMessageHandler(response);
            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://api.example.com")
            };
            mockHttpClientFactory.Setup(f => f.CreateClient("apiSR")).Returns(httpClient);

            // Act
            await useCase.Executar(filtros);

            // Assert
            mockServicoAcervo.Verify(s => s.ObterTiposAcervosPermitidosDoPerfilLogado(), Times.Once);
        }

        [Fact(DisplayName = "RelatorioControleAcervo - Deve realizar post na rota correta")]
        public async Task Executar_DeveRealizarPostNaRotaCorreta()
        {
            // Arrange
            var filtros = new RelatorioControleAcervoRequest();

            mockServicoAcervo.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(new long[] { 1 });

            mockContextoAplicacao.Setup(c => c.NomeUsuario).Returns("João");
            mockContextoAplicacao.Setup(c => c.UsuarioLogado).Returns("RF123");

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(new MemoryStream())
            };

            var mockHandler = CriarMockHttpMessageHandler(response);
            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://api.example.com")
            };
            mockHttpClientFactory.Setup(f => f.CreateClient("apiSR")).Returns(httpClient);

            // Act
            await useCase.Executar(filtros);

            // Assert
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri!.ToString().EndsWith("v1/cdep/controle-acervo")),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact(DisplayName = "RelatorioControleAcervo - Deve usar cliente com nome apiSR")]
        public async Task Executar_DeveUsarClienteComNomeApiSR()
        {
            // Arrange
            var filtros = new RelatorioControleAcervoRequest();

            mockServicoAcervo.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(new long[] { 1 });

            mockContextoAplicacao.Setup(c => c.NomeUsuario).Returns("João");
            mockContextoAplicacao.Setup(c => c.UsuarioLogado).Returns("RF123");

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(new MemoryStream())
            };

            var mockHandler = CriarMockHttpMessageHandler(response);
            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://api.example.com")
            };
            mockHttpClientFactory.Setup(f => f.CreateClient("apiSR")).Returns(httpClient);

            // Act
            await useCase.Executar(filtros);

            // Assert
            mockHttpClientFactory.Verify(f => f.CreateClient("apiSR"), Times.Once);
        }

        [Fact(DisplayName = "RelatorioControleAcervo - Deve ler conteúdo do stream da resposta")]
        public async Task Executar_DeveLerConteudoDoStreamDaResposta()
        {
            // Arrange
            var filtros = new RelatorioControleAcervoRequest();
            var conteudoEsperado = "Conteúdo do relatório PDF";
            var streamContent = new MemoryStream(Encoding.UTF8.GetBytes(conteudoEsperado));

            mockServicoAcervo.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(new long[] { 1 });

            mockContextoAplicacao.Setup(c => c.NomeUsuario).Returns("João");
            mockContextoAplicacao.Setup(c => c.UsuarioLogado).Returns("RF123");

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(streamContent)
            };

            var mockHandler = CriarMockHttpMessageHandler(response);
            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://api.example.com")
            };
            mockHttpClientFactory.Setup(f => f.CreateClient("apiSR")).Returns(httpClient);

            // Act
            var resultado = await useCase.Executar(filtros);

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact(DisplayName = "RelatorioControleAcervo - Construtor deve inicializar com dependências injetadas")]
        public void Construtor_DeveInicializarComDependenciasInjetadas()
        {
            // Assert
            Assert.NotNull(useCase);
        }

        private static Mock<HttpMessageHandler> CriarMockHttpMessageHandler(HttpResponseMessage response)
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            return mockHandler;
        }
    }
}