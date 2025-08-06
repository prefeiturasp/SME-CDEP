using AutoMapper;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Dtos;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoAcervoAuditavelTestes
    {
        private readonly Mock<IRepositorioAcervo> _repositorioAcervoMock;
        private readonly Mock<IRepositorioAcervoCreditoAutor> _repositorioAcervoCreditoAutorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IContextoAplicacao> _contextoAplicacaoMock;
        private readonly Mock<IRepositorioArquivo> _repositorioArquivoMock;
        private readonly Mock<IRepositorioAcervoBibliografico> _repositorioAcervoBibliograficoMock;
        private readonly Mock<IRepositorioParametroSistema> _repositorioParametroSistemaMock;
        private readonly Mock<IOptions<ConfiguracaoArmazenamentoOptions>> _optionsMock;

        private readonly ServicoAcervoAuditavel _servico;

        public ServicoAcervoAuditavelTestes()
        {
            // Inicialização dos Mocks
            _repositorioAcervoMock = new Mock<IRepositorioAcervo>();
            _repositorioAcervoCreditoAutorMock = new Mock<IRepositorioAcervoCreditoAutor>();
            _mapperMock = new Mock<IMapper>();
            _contextoAplicacaoMock = new Mock<IContextoAplicacao>();
            _repositorioArquivoMock = new Mock<IRepositorioArquivo>();
            _repositorioAcervoBibliograficoMock = new Mock<IRepositorioAcervoBibliografico>();
            _repositorioParametroSistemaMock = new Mock<IRepositorioParametroSistema>();

            var configOptions = new ConfiguracaoArmazenamentoOptions { EndPoint = "localhost/arquivos", TipoRequisicao = "http" };
            _optionsMock = new Mock<IOptions<ConfiguracaoArmazenamentoOptions>>();
            _optionsMock.Setup(o => o.Value).Returns(configOptions);

            // Mock do contexto da aplicação (usuário logado)
            _contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns("usuario.teste");
            _contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns("Usuário de Teste");

            _servico = new ServicoAcervoAuditavel(
                _repositorioAcervoMock.Object,
                _mapperMock.Object,
                _contextoAplicacaoMock.Object,
                _repositorioAcervoCreditoAutorMock.Object,
                _repositorioArquivoMock.Object,
                _repositorioAcervoBibliograficoMock.Object,
                Mock.Of<IRepositorioAcervoDocumental>(),
                Mock.Of<IRepositorioAcervoArteGrafica>(),
                Mock.Of<IRepositorioAcervoAudiovisual>(),
                Mock.Of<IRepositorioAcervoFotografico>(),
                Mock.Of<IRepositorioAcervoTridimensional>(),
                _repositorioParametroSistemaMock.Object,
                _optionsMock.Object
            );
        }

        [Fact]
        public async Task Inserir_ComAcervoValido_DeveSalvarRepositorioEAutores()
        {
            // Arrange
            var acervo = new Acervo
            {
                Codigo = "TMB-001",
                Ano = "2024",
                TipoAcervoId = (long)TipoAcervo.Bibliografico,
                CreditosAutoresIds = new long[] { 1, 2 },
                CoAutores = new List<CoAutor> { new CoAutor { CreditoAutorId = 3, TipoAutoria = "Ilustrador" } }
            };

            _repositorioAcervoMock.Setup(r => r.ExisteCodigo(acervo.Codigo, 0, It.IsAny<TipoAcervo>())).ReturnsAsync(false);
            _repositorioAcervoMock.Setup(r => r.Inserir(acervo)).ReturnsAsync(99);

            // Act
            var resultadoId = await _servico.Inserir(acervo);

            // Assert
            resultadoId.Should().Be(99);
            _repositorioAcervoMock.Verify(r => r.Inserir(It.Is<Acervo>(a => a.CriadoLogin == "usuario.teste")), Times.Once);
            _repositorioAcervoCreditoAutorMock.Verify(r => r.Inserir(It.IsAny<AcervoCreditoAutor>()), Times.Exactly(3));
        }

        [Fact]
        public async Task Inserir_ComCodigoDuplicado_DeveLancarNegocioException()
        {
            // Arrange
            var acervo = new Acervo { Codigo = "TMB-001", Ano = "2024" };
            _repositorioAcervoMock.Setup(r => r.ExisteCodigo(acervo.Codigo, 0, It.IsAny<TipoAcervo>())).ReturnsAsync(true);

            // Act
            Func<Task> acao = async () => await _servico.Inserir(acervo);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>().WithMessage("Registro 'Tombo' duplicado");
        }

        [Fact]
        public async Task Inserir_ComAnoFuturo_DeveLancarNegocioException()
        {
            // Arrange
            var acervo = new Acervo { Codigo = "TMB-001", Ano = (DateTime.Now.Year + 1).ToString() };

            // Act
            Func<Task> acao = async () => await _servico.Inserir(acervo);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>().WithMessage("O campo ano não admite anos futuros. Apenas anos atuais e anteriores são permitidos.");
        }

        [Fact]
        public async Task Alterar_ComDadosValidos_DeveAtualizarAcervoEAutores()
        {
            // Arrange
            var acervo = new Acervo
            {
                Id = 1,
                Codigo = "TMB-001",
                Ano = "2023",
                TipoAcervoId = (long)TipoAcervo.Bibliografico,
                CreditosAutoresIds = new long[] { 2, 3 }, // Manter o 2, remover o 1, adicionar o 3
                CoAutores = new List<CoAutor>()
            };
            var acervoDto = new AcervoDTO
            {
                Id = acervo.Id,
                Codigo = acervo.Codigo,
                Ano = acervo.Ano,
                TipoAcervoId = acervo.TipoAcervoId,
                CreditosAutoresIds = acervo.CreditosAutoresIds
            };
            var autoresAtuais = new List<AcervoCreditoAutor>
            {
                new AcervoCreditoAutor { AcervoId = 1, CreditoAutorId = 1 },
                new AcervoCreditoAutor { AcervoId = 1, CreditoAutorId = 2 }
            };

            _repositorioAcervoMock.Setup(r => r.ExisteCodigo(acervo.Codigo, acervo.Id, It.IsAny<TipoAcervo>())).ReturnsAsync(false);
            _repositorioAcervoCreditoAutorMock.Setup(r => r.ObterPorAcervoId(acervo.Id, false)).ReturnsAsync(autoresAtuais);
            _repositorioAcervoCreditoAutorMock.Setup(r => r.ObterPorAcervoId(acervo.Id, true)).ReturnsAsync(new List<AcervoCreditoAutor>());
            _repositorioAcervoMock.Setup(r => r.Atualizar(acervo)).ReturnsAsync(acervo);
            _mapperMock.Setup(m => m.Map<AcervoDTO>(It.IsAny<Acervo>())).Returns(acervoDto);

            // Act
            await _servico.Alterar(acervo);

            // Assert
            _repositorioAcervoMock.Verify(r => r.Atualizar(It.Is<Acervo>(a => a.AlteradoLogin == "usuario.teste")), Times.Once);
            _repositorioAcervoCreditoAutorMock.Verify(r => r.Inserir(It.Is<AcervoCreditoAutor>(a => a.CreditoAutorId == 3)), Times.Once);
            _repositorioAcervoCreditoAutorMock.Verify(r => r.Excluir(It.Is<long[]>(ids => ids.Contains(1L)), acervo.Id), Times.Once);
        }

        [Fact]
        public async Task ObterDetalhamento_ParaTipoBibliografico_DeveChamarRepositorioCorreto()
        {
            // Arrange
            var filtro = new FiltroDetalharAcervoDTO { Codigo = "COD-BIB", Tipo = TipoAcervo.Bibliografico };
            var detalhe = new AcervoBibliograficoDetalhe { Titulo = "Livro Teste" };
            var detalheDTO = new AcervoBibliograficoDetalheDTO
            {
                Titulo = detalhe.Titulo,
            };

            _repositorioAcervoBibliograficoMock.Setup(r => r.ObterDetalhamentoPorCodigo(It.IsAny<string>())).ReturnsAsync(detalhe);
            _repositorioArquivoMock.Setup(r => r.ObterArquivoPorNomeTipoArquivo(It.IsAny<string>(), TipoArquivo.Sistema))
                                      .ReturnsAsync(new Arquivo { Codigo = Guid.NewGuid(), Nome = "padrao.jpg" });
            _mapperMock.Setup(m => m.Map<AcervoBibliograficoDetalheDTO>(It.IsAny<AcervoBibliograficoDetalhe>())).Returns(detalheDTO);

            // Act
            var resultado = await _servico.ObterDetalhamentoPorTipoAcervoECodigo(filtro);

            // Assert
            resultado.Should().BeOfType<AcervoBibliograficoDetalheDTO>();
            resultado.Should().BeEquivalentTo(detalheDTO);
            _repositorioAcervoBibliograficoMock.Verify(r => r.ObterDetalhamentoPorCodigo(filtro.Codigo), Times.Once);
        }

        [Fact]
        public async Task ObterDetalhamento_QuandoAcervoNaoEncontrado_DeveLancarNegocioException()
        {
            // Arrange
            var filtro = new FiltroDetalharAcervoDTO { Codigo = "COD-INEXISTENTE", Tipo = TipoAcervo.Bibliografico };
            _repositorioAcervoBibliograficoMock.Setup(r => r.ObterDetalhamentoPorCodigo(filtro.Codigo)).ReturnsAsync((AcervoBibliograficoDetalhe)null);

            // Act
            Func<Task> acao = async () => await _servico.ObterDetalhamentoPorTipoAcervoECodigo(filtro);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>().WithMessage("Acervo não encontrado.");
        }

        [Fact]
        public void ObterTiposPermitidos_ParaPerfilAdminGeral_DeveRetornarTodosOsTipos()
        {
            // Arrange
            _contextoAplicacaoMock.Setup(c => c.PerfilUsuario).Returns("D3766FB4-D753-4398-BFB0-C357724BB0A2");
            var totalTipos = Enum.GetValues(typeof(TipoAcervo)).Length;

            // Act
            var resultado = _servico.ObterTiposAcervosPermitidosDoPerfilLogado();

            // Assert
            resultado.Should().HaveCount(totalTipos);
        }

        [Fact]
        public void ObterTiposPermitidos_ParaPerfilAdminBiblioteca_DeveRetornarApenasBibliografico()
        {
            // Arrange
            _contextoAplicacaoMock.Setup(c => c.PerfilUsuario).Returns("B82673B9-52B9-4E01-9157-E19339B7211A");

            // Act
            var resultado = _servico.ObterTiposAcervosPermitidosDoPerfilLogado();

            // Assert
            resultado.Should().HaveCount(1);
            resultado.First().Should().Be((long)TipoAcervo.Bibliografico);
        }


        [Fact]
        public async Task ObterPorFiltro_QuandoNaoEncontraRegistros_DeveRetornarPaginacaoVazia()
        {
            // Arrange
            var filtro = new AcervoFiltroDto(null, "", null, "");
            _repositorioAcervoMock.Setup(r => r.ContarPorFiltro(It.IsAny<AcervoFiltroDto>()))
                                  .ReturnsAsync(0);

            // Act
            var resultado = await _servico.ObterPorFiltro(filtro.TipoAcervo, filtro.Titulo, filtro.CreditoAutorId, filtro.Codigo);

            // Assert
            resultado.Should().NotBeNull();
            resultado.TotalRegistros.Should().Be(0);
            resultado.TotalPaginas.Should().Be(0);
            resultado.Items.Should().NotBeNull().And.BeEmpty();

            _repositorioAcervoMock.Verify(r => r.PesquisarPorFiltroPaginado(It.IsAny<AcervoFiltroDto>(), It.IsAny<PaginacaoDto>()), Times.Never);
        }

        [Fact]
        public async Task ObterPorFiltro_QuandoEncontraRegistros_DeveRetornarPaginacaoComItensFormatados()
        {
            // Arrange
            var totalRegistros = 2;
            var paginacao = new Paginacao(1, 10, 0);
            var paginacaoDto = new PaginacaoDto { Pagina = 1, QuantidadeRegistros = 10 };

            var autores = new Faker<CreditoAutor>("pt_BR").Generate(2);

            var acervosDoRepo = new List<Acervo>
            {
                new Faker<Acervo>("pt_BR")
                    .RuleFor(a => a.CreditosAutores, f => autores)
                    .RuleFor(a => a.TipoAcervoId, (long)TipoAcervo.Bibliografico)
                    .RuleFor(a => a.Id, f => f.Random.Int())
                    .Generate(),
                new Faker<Acervo>("pt_BR")
                    .RuleFor(a => a.CreditosAutores, f => null) // Acervo sem autores para testar o 'else'
                    .RuleFor(a => a.TipoAcervoId, (long)TipoAcervo.Fotografico)
                    .RuleFor(a => a.Id, f => f.Random.Int())
                    .Generate(),
            };

            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroPagina")).Returns(paginacao.Pagina.ToString());
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroRegistros")).Returns(paginacao.QuantidadeRegistros.ToString());
            _mapperMock.Setup(m => m.Map<PaginacaoDto>(It.IsAny<Paginacao>())).Returns(paginacaoDto);

            _repositorioAcervoMock.Setup(r => r.ContarPorFiltro(It.IsAny<AcervoFiltroDto>()))
                                  .ReturnsAsync(totalRegistros);
            _repositorioAcervoMock.Setup(r => r.PesquisarPorFiltroPaginado(It.IsAny<AcervoFiltroDto>(), It.IsAny<PaginacaoDto>()))
                                  .ReturnsAsync(acervosDoRepo);

            // Act
            var resultado = await _servico.ObterPorFiltro(null, "", null, "");

            // Assert
            resultado.Should().NotBeNull();
            resultado.TotalRegistros.Should().Be(totalRegistros);
            resultado.Items.Should().HaveCount((int)totalRegistros);

            // Valida a formatação do acervo com autores
            var itemComAutor = resultado.Items.First(i => i.AcervoId == acervosDoRepo[0].Id);
            var autoresEsperados = string.Join(", ", autores.Select(a => a.Nome));
            itemComAutor.CreditoAutoria.Should().Be(autoresEsperados);

            // Valida a formatação do acervo sem autores
            var itemSemAutor = resultado.Items.First(i => i.AcervoId == acervosDoRepo[1].Id);
            itemSemAutor.CreditoAutoria.Should().BeEmpty();
        }

        [Theory(DisplayName = "Deve mapear corretamente a descrição do TipoAcervo")]
        [InlineData(TipoAcervo.Bibliografico, "Bibliográfico")]
        [InlineData(TipoAcervo.DocumentacaoTextual, "Documentação textual")]
        [InlineData(TipoAcervo.ArtesGraficas, "Artes gráficas")]
        [InlineData(TipoAcervo.Audiovisual, "Audiovisual")]
        [InlineData(TipoAcervo.Fotografico, "Fotográfico")]
        [InlineData(TipoAcervo.Tridimensional, "Tridimensional")]
        public async Task ObterPorFiltro_DeveMapearCorretamenteDescricaoDoTipoAcervo(TipoAcervo tipoAcervo, string descricaoEsperada)
        {
            // Arrange
            var acervoDoRepo = new List<Acervo>
        {
            new Faker<Acervo>().RuleFor(a => a.TipoAcervoId, (long)tipoAcervo).Generate()
        };

            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>(It.IsAny<string>())).Returns("1");
            _mapperMock.Setup(m => m.Map<PaginacaoDto>(It.IsAny<Paginacao>())).Returns(new PaginacaoDto());
            _repositorioAcervoMock.Setup(r => r.ContarPorFiltro(It.IsAny<AcervoFiltroDto>())).ReturnsAsync(1);
            _repositorioAcervoMock.Setup(r => r.PesquisarPorFiltroPaginado(It.IsAny<AcervoFiltroDto>(), It.IsAny<PaginacaoDto>()))
                                  .ReturnsAsync(acervoDoRepo);

            // Act
            var resultado = await _servico.ObterPorFiltro(null, "", null, "");

            // Assert
            resultado.Items.Should().HaveCount(1);
            var item = resultado.Items.First();
            item.TipoAcervoId.Should().Be(tipoAcervo);
            item.TipoAcervo.Should().Be(descricaoEsperada);
        }

        [Theory(DisplayName = "Deve calcular corretamente o total de páginas")]
        [InlineData(19, 10, 2)] // 19 itens, 10 por página -> 2 páginas
        [InlineData(20, 10, 2)] // 20 itens, 10 por página -> 2 páginas
        [InlineData(21, 10, 3)] // 21 itens, 10 por página -> 3 páginas
        [InlineData(5, 10, 1)]  // 5 itens, 10 por página -> 1 página
        [InlineData(0, 10, 0)]   // 0 itens, 10 por página -> 0 páginas
        public async Task ObterPorFiltro_DeveCalcularCorretamenteTotalDePaginas(int totalRegistros, int registrosPorPagina, int paginasEsperadas)
        {
            // Arrange
            var acervosDoRepo = new Faker<Acervo>().Generate(1); // Apenas para o fluxo não retornar vazio

            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroPagina")).Returns("1");
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroRegistros")).Returns(registrosPorPagina.ToString());
            _mapperMock.Setup(m => m.Map<PaginacaoDto>(It.IsAny<Paginacao>()))
                       .Returns(new PaginacaoDto { QuantidadeRegistros = registrosPorPagina });

            _repositorioAcervoMock.Setup(r => r.ContarPorFiltro(It.IsAny<AcervoFiltroDto>()))
                                  .ReturnsAsync(totalRegistros);
            _repositorioAcervoMock.Setup(r => r.PesquisarPorFiltroPaginado(It.IsAny<AcervoFiltroDto>(), It.IsAny<PaginacaoDto>()))
                                  .ReturnsAsync(acervosDoRepo);

            // Act
            var resultado = await _servico.ObterPorFiltro(null, "", null, "");

            // Assert
            resultado.TotalPaginas.Should().Be(paginasEsperadas);
        }

    }
}
