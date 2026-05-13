using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Aplicacao.UseCase;
using SME.CDEP.Dominio.Contexto;
using System.Net;
using System.Text;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase.Relatorio
{
    public class RelatorioControleAcervoAutorUseCaseTeste
    {
        private readonly Mock<IHttpClientFactory> mockHttpClientFactory;
        private readonly Mock<IServicoAcervo> mockServicoAcervo;
        private readonly Mock<IContextoAplicacao> mockContextoAplicacao;
        private readonly RelatorioControleAcervoAutorUseCase useCase;

        public RelatorioControleAcervoAutorUseCaseTeste()
        {
            mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockServicoAcervo = new Mock<IServicoAcervo>();
            mockContextoAplicacao = new Mock<IContextoAplicacao>();

            useCase = new RelatorioControleAcervoAutorUseCase(
                mockHttpClientFactory.Object,
                mockServicoAcervo.Object,
                mockContextoAplicacao.Object
            );
        }

        [Fact(DisplayName = "RelatorioControleAcervoAutor - Executar com dados válidos deve retornar stream com sucesso")]
        public async Task Executar_ComDadosValidos_DeveRetornarStreamComSucesso()
        {
            var filtros = new RelatorioControleAcervoAutorRequest
            {
                Autores = new List<int> { 1 },
                TipoAcervo = Infra.Dominio.Enumerados.TipoAcervo.Bibliografico
            };

            var tiposAcervosPermitidos = new long[] { 1, 2, 3 };
            mockServicoAcervo.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(tiposAcervosPermitidos);

            mockContextoAplicacao.Setup(c => c.NomeUsuario).Returns("João Silva");
            mockContextoAplicacao.Setup(c => c.UsuarioLogado).Returns("RF123456");

            var conteudoResposta = "Conteúdo do relatório de autores";
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

            var resultado = await useCase.Executar(filtros);

            Assert.NotNull(resultado);
            Assert.IsType<MemoryStream>(resultado);
            mockHttpClientFactory.Verify(f => f.CreateClient("apiSR"), Times.Once);
        }

        [Fact(DisplayName = "RelatorioControleAcervoAutor - Com status code NoContent deve retornar null")]
        public async Task Executar_ComStatusCodeNoContent_DeveRetornarNull()
        {
            var filtros = new RelatorioControleAcervoAutorRequest
            {
                Autores = new List<int> { 1 },
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

            var resultado = await useCase.Executar(filtros);

            Assert.Null(resultado);
        }

        [Fact(DisplayName = "RelatorioControleAcervoAutor - Com status code BadRequest deve retornar null")]
        public async Task Executar_ComStatusCodeBadRequest_DeveRetornarNull()
        {
            var filtros = new RelatorioControleAcervoAutorRequest
            {
                Autores = new List<int> { 1 },
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

            var resultado = await useCase.Executar(filtros);

            Assert.Null(resultado);
        }

        [Fact(DisplayName = "RelatorioControleAcervoAutor - Com status code Unauthorized deve retornar null")]
        public async Task Executar_ComStatusCodeUnauthorized_DeveRetornarNull()
        {
            var filtros = new RelatorioControleAcervoAutorRequest
            {
                Autores = new List<int> { 1 },
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

            var resultado = await useCase.Executar(filtros);

            Assert.Null(resultado);
        }

        [Fact(DisplayName = "RelatorioControleAcervoAutor - Deve serializar filtros corretamente")]
        public async Task Executar_ComDadosValidos_DeveSerializarFiltrosCorretamente()
        {
            var autoresEsperados = new List<int> { 1 };
            var tipoAcervoEsperado = Infra.Dominio.Enumerados.TipoAcervo.Bibliografico;
            var filtros = new RelatorioControleAcervoAutorRequest
            {
                Autores = autoresEsperados,
                TipoAcervo = tipoAcervoEsperado
            };

            var tiposAcervosPermitidos = new long[] { 5, 10, 15 };
            mockServicoAcervo.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(tiposAcervosPermitidos);

            mockContextoAplicacao.Setup(c => c.NomeUsuario).Returns("Usuário Teste");
            mockContextoAplicacao.Setup(c => c.UsuarioLogado).Returns("RF999");

            var conteudoResposta = "PDF do relatório";
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

            await useCase.Executar(filtros);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri!.ToString().EndsWith("v1/cdep/controle-acervo-autor")),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact(DisplayName = "RelatorioControleAcervoAutor - Deve usar nome usuário do contexto")]
        public async Task Executar_DeveUsarNomeUsuarioDoContexto()
        {
            var filtros = new RelatorioControleAcervoAutorRequest
            {
                Autores = new List<int> { 1 },
                TipoAcervo = Infra.Dominio.Enumerados.TipoAcervo.Bibliografico
            };
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

            await useCase.Executar(filtros);

            mockContextoAplicacao.Verify(c => c.NomeUsuario, Times.Once);
        }

        [Fact(DisplayName = "RelatorioControleAcervoAutor - Deve usar usuário logado do contexto")]
        public async Task Executar_DeveUsarUsuarioLogadoDoContexto()
        {
            var filtros = new RelatorioControleAcervoAutorRequest
            {
                Autores = new List<int> { 1 },
                TipoAcervo = Infra.Dominio.Enumerados.TipoAcervo.Bibliografico
            };
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

            await useCase.Executar(filtros);

            mockContextoAplicacao.Verify(c => c.UsuarioLogado, Times.Once);
        }

        [Fact(DisplayName = "RelatorioControleAcervoAutor - Deve obter tipos acervos permitidos do perfil")]
        public async Task Executar_DeveObterTiposAcervosPermitidosDoPerfil()
        {
            var filtros = new RelatorioControleAcervoAutorRequest
            {
                Autores = new List<int> { 1 },
                TipoAcervo = Infra.Dominio.Enumerados.TipoAcervo.Bibliografico
            };
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

            await useCase.Executar(filtros);

            mockServicoAcervo.Verify(s => s.ObterTiposAcervosPermitidosDoPerfilLogado(), Times.Once);
        }

        [Fact(DisplayName = "RelatorioControleAcervoAutor - Deve realizar post na rota correta")]
        public async Task Executar_DeveRealizarPostNaRotaCorreta()
        {
            var filtros = new RelatorioControleAcervoAutorRequest
            {
                Autores = new List<int> { 1 },
                TipoAcervo = Infra.Dominio.Enumerados.TipoAcervo.Bibliografico
            };

            mockServicoAcervo.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(new long[] { 1, 2 });

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

            await useCase.Executar(filtros);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri!.ToString().EndsWith("v1/cdep/controle-acervo-autor")),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact(DisplayName = "RelatorioControleAcervoAutor - Deve usar cliente com nome apiSR")]
        public async Task Executar_DeveUsarClienteComNomeApiSR()
        {
            var filtros = new RelatorioControleAcervoAutorRequest
            {
                Autores = new List<int> { 1 },
                TipoAcervo = Infra.Dominio.Enumerados.TipoAcervo.Bibliografico
            };

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

            await useCase.Executar(filtros);

            mockHttpClientFactory.Verify(f => f.CreateClient("apiSR"), Times.Once);
        }

        [Fact(DisplayName = "RelatorioControleAcervoAutor - Deve ler conteúdo do stream da resposta")]
        public async Task Executar_DeveLerConteudoDoStreamDaResposta()
        {
            var filtros = new RelatorioControleAcervoAutorRequest
            {
                Autores = new List<int> { 1 },
                TipoAcervo = Infra.Dominio.Enumerados.TipoAcervo.Bibliografico
            };
            var conteudoEsperado = "Conteúdo do relatório PDF de autores";
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

            var resultado = await useCase.Executar(filtros);

            Assert.NotNull(resultado);
        }

        [Fact(DisplayName = "RelatorioControleAcervoAutor - Construtor deve inicializar com dependências injetadas")]
        public void Construtor_DeveInicializarComDependenciasInjetadas()
        {
            Assert.NotNull(useCase);
        }

        [Fact(DisplayName = "RelatorioControleAcervoAutor - Deve serializar objeto mensagem com Mensagem wrapper")]
        public async Task Executar_DeveSerializarComMensagemWrapper()
        {
            var filtros = new RelatorioControleAcervoAutorRequest
            {
                Autores = new List<int> { 1 },
                TipoAcervo = Infra.Dominio.Enumerados.TipoAcervo.Bibliografico
            };

            mockServicoAcervo.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(new long[] { 1 });

            mockContextoAplicacao.Setup(c => c.NomeUsuario).Returns("João");
            mockContextoAplicacao.Setup(c => c.UsuarioLogado).Returns("RF123");

            var conteudoCapturado = string.Empty;
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Callback<HttpRequestMessage, CancellationToken>(async (request, ct) =>
                {
                    if (request.Content is StringContent stringContent)
                    {
                        conteudoCapturado = await stringContent.ReadAsStringAsync(ct);
                    }
                })
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(new MemoryStream())
                });

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://api.example.com")
            };
            mockHttpClientFactory.Setup(f => f.CreateClient("apiSR")).Returns(httpClient);

            await useCase.Executar(filtros);

            Assert.NotEmpty(conteudoCapturado);
            var objetoSerializado = JsonConvert.DeserializeObject<dynamic>(conteudoCapturado)!;
            Assert.NotNull(objetoSerializado["Mensagem"]);
        }

        [Fact(DisplayName = "RelatorioControleAcervoAutor - Com filtros null em autores deve processar corretamente")]
        public async Task Executar_ComFiltrosNulosEmAutores_DeveProcessarCorretamente()
        {
            var filtros = new RelatorioControleAcervoAutorRequest
            {
                Autores = null,
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

            var resultado = await useCase.Executar(filtros);

            Assert.NotNull(resultado);
        }

        [Fact(DisplayName = "RelatorioControleAcervoAutor - Com múltiplos autores deve passar todos")]
        public async Task Executar_ComMultiplosAutores_DevePassarTodos()
        {
            var autores = new List<int> { 1, 2, 3, 4 };
            var filtros = new RelatorioControleAcervoAutorRequest
            {
                Autores = autores,
                TipoAcervo = Infra.Dominio.Enumerados.TipoAcervo.Bibliografico
            };

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

            var resultado = await useCase.Executar(filtros);

            Assert.NotNull(resultado);
            mockServicoAcervo.Verify(s => s.ObterTiposAcervosPermitidosDoPerfilLogado(), Times.Once);
        }

        [Fact(DisplayName = "RelatorioControleAcervoAutor - Com diversos tipos de acervo permitidos")]
        public async Task Executar_ComDiversosTiposDeAcervoPermitidos_DevePassarTodos()
        {
            var filtros = new RelatorioControleAcervoAutorRequest
            {
                Autores = new List<int> { 1 },
                TipoAcervo = Infra.Dominio.Enumerados.TipoAcervo.Bibliografico
            };

            var tiposPermitidos = new long[] { 1, 5, 10, 15, 20, 25 };
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
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
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