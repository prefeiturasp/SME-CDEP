using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Controllers;

namespace SME.CDEP.TesteUnitario.WebApi.Controllers
{
    public class AcervoControllerTestes
    {
        private readonly Mock<IServicoAcervo> _servicoAcervoMock;
        private readonly AcervoController _controller;
        private readonly Faker _faker;

        public AcervoControllerTestes()
        {
            _servicoAcervoMock = new Mock<IServicoAcervo>();
            _controller = new AcervoController();
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public void ObterTiposDeAcervos_QuandoChamado_DeveRetornarOkComOsTipos()
        {
            // Arrange
            var tiposDeAcervo = new List<IdNomeDTO>
            {
                new IdNomeDTO { Id = 1, Nome = "Bibliográfico" },
                new IdNomeDTO { Id = 2, Nome = "Documentação Textual" }
            };
            _servicoAcervoMock.Setup(s => s.ObterTodosTipos()).Returns(tiposDeAcervo);

            // Act
            var resultado = _controller.ObterTiposDeAcervos(_servicoAcervoMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            okResult.StatusCode.Should().Be(200);

            var valorRetornado = Assert.IsAssignableFrom<IEnumerable<IdNomeDTO>>(okResult.Value);
            valorRetornado.Should().BeEquivalentTo(tiposDeAcervo);

            _servicoAcervoMock.Verify(s => s.ObterTodosTipos(), Times.Once);
        }

        [Fact]
        public async Task ObterPorFiltro_QuandoChamado_DeveRetornarOkComResultadoPaginado()
        {
            // Arrange
            var filtro = new FiltroTipoTituloCreditoAutoriaCodigoAcervoDTO { Titulo = "História" };
            var paginacaoResultado = new PaginacaoResultadoDTO<AcervoTableRowDTO>
            {
                Items = _faker.Make(3, () => new AcervoTableRowDTO { Titulo = _faker.Commerce.ProductName() }),
                TotalRegistros = 3,
                TotalPaginas = 1
            };

            _servicoAcervoMock.Setup(s => s.ObterPorFiltro(filtro.TipoAcervo, filtro.Titulo, filtro.CreditoAutorId, filtro.Codigo, filtro.IdEditora))
                              .ReturnsAsync(paginacaoResultado);

            // Act
            var resultado = await _controller.ObterTodosOuPorTipoTituloCreditoAutoriaTomboECodigo(filtro, _servicoAcervoMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var valorRetornado = Assert.IsType<PaginacaoResultadoDTO<AcervoTableRowDTO>>(okResult.Value);
            valorRetornado.Should().BeEquivalentTo(paginacaoResultado);

            _servicoAcervoMock.Verify(s => s.ObterPorFiltro(filtro.TipoAcervo, filtro.Titulo, filtro.CreditoAutorId, filtro.Codigo, filtro.IdEditora), Times.Once);
        }

        [Fact]
        public async Task ObterPorTextoLivre_QuandoChamado_DeveRetornarOkComResultadoPaginado()
        {
            // Arrange
            var filtro = new FiltroTextoLivreTipoAcervoDTO { TextoLivre = "Guerra" };
            var paginacaoResultado = new PaginacaoResultadoDTO<PesquisaAcervoDTO>
            {
                Items = _faker.Make(2, () => new PesquisaAcervoDTO { Titulo = _faker.Lorem.Sentence() }),
                TotalRegistros = 2,
                TotalPaginas = 1
            };

            _servicoAcervoMock.Setup(s => s.ObterPorTextoLivreETipoAcervo(filtro))
                              .ReturnsAsync(paginacaoResultado);

            // Act
            var resultado = await _controller.ObterPorTextoLivreETipoAcervo(filtro, _servicoAcervoMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var valorRetornado = Assert.IsType<PaginacaoResultadoDTO<PesquisaAcervoDTO>>(okResult.Value);
            valorRetornado.Should().BeEquivalentTo(paginacaoResultado);

            _servicoAcervoMock.Verify(s => s.ObterPorTextoLivreETipoAcervo(filtro), Times.Once);
        }

        [Fact]
        public async Task ObterDetalhamentoAcervo_QuandoChamado_DeveRetornarOkComDetalhes()
        {
            // Arrange
            var filtro = new FiltroDetalharAcervoDTO { Codigo = "TMB-001", Tipo = TipoAcervo.Bibliografico };
            var detalheDto = new AcervoDetalheDTO { Titulo = "O Pequeno Príncipe", Codigo = filtro.Codigo };

            _servicoAcervoMock.Setup(s => s.ObterDetalhamentoPorTipoAcervoECodigo(filtro))
                              .ReturnsAsync(detalheDto);

            // Act
            var resultado = await _controller.ObterDetalhamentoAcervo(filtro, _servicoAcervoMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var valorRetornado = Assert.IsType<AcervoDetalheDTO>(okResult.Value);
            valorRetornado.Titulo.Should().Be(detalheDto.Titulo);

            _servicoAcervoMock.Verify(s => s.ObterDetalhamentoPorTipoAcervoECodigo(filtro), Times.Once);
        }

        [Fact]
        public async Task ObterTermoDeCompromisso_QuandoChamado_DeveRetornarOkComStringDoTermo()
        {
            // Arrange
            var termo = "Eu, [NOME_SOLICITANTE], comprometo-me a cuidar do item...";
            _servicoAcervoMock.Setup(s => s.ObterTermoDeCompromisso()).ReturnsAsync(termo);

            // Act
            var resultado = await _controller.ObterTermoDeCompromisso(_servicoAcervoMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            okResult.Value.Should().Be(termo);

            _servicoAcervoMock.Verify(s => s.ObterTermoDeCompromisso(), Times.Once);
        }

        [Fact]
        public async Task PesquisarAcervoPorCodigoTombo_QuandoChamado_DeveRetornarOkComResultado()
        {
            // Arrange
            var filtro = new FiltroCodigoTomboDTO { CodigoTombo = "TMB-001" };
            var resultadoDto = new IdNomeCodigoTipoParaEmprestimoDTO
            {
                Id = 1,
                Nome = "Livro para Empréstimo",
                Codigo = filtro.CodigoTombo,
                Tipo = (int)TipoAcervo.Bibliografico
            };

            _servicoAcervoMock.Setup(s => s.PesquisarAcervoPorCodigoTombo(filtro)).ReturnsAsync(resultadoDto);

            // Act
            var resultado = await _controller.PesquisarAcervoPorCodigoTombo(filtro, _servicoAcervoMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var valorRetornado = Assert.IsType<IdNomeCodigoTipoParaEmprestimoDTO>(okResult.Value);
            valorRetornado.Should().BeEquivalentTo(resultadoDto);

            _servicoAcervoMock.Verify(s => s.PesquisarAcervoPorCodigoTombo(filtro), Times.Once);
        }
    }
}
