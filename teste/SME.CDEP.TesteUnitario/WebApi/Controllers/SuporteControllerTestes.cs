using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Controllers;

namespace SME.CDEP.TesteUnitario.Webapi.Controllers
{
    public class SuporteControllerTestes
    {
        private readonly Mock<IServicoSuporte> servicoSuporteMock;
        private readonly SuporteController sut;

        public SuporteControllerTestes()
        {
            var mocker = new AutoMocker();

            servicoSuporteMock = mocker.GetMock<IServicoSuporte>();

            sut = mocker.CreateInstance<SuporteController>();
        }

        [Fact]
        public async Task DadoNomeTipoDtoValido_QuandoInserir_EntaoRetornaOkComIdGerado()
        {
            // Arrange
            var dto = GerarNomeTipoDTO();
            var idGerado = new Faker().Random.Long(1, 1000);

            servicoSuporteMock
                .Setup(s => s.Inserir(It.Is<IdNomeTipoExcluidoDTO>(x => x.Nome == dto.Nome && x.Tipo == dto.Tipo)))
                .ReturnsAsync(idGerado);

            // Act
            var resultado = await sut.Inserir(dto, servicoSuporteMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(idGerado);
            servicoSuporteMock.Verify(s => s.Inserir(It.Is<IdNomeTipoExcluidoDTO>(x => x.Nome == dto.Nome && x.Tipo == dto.Tipo)), Times.Once);
        }

        [Fact]
        public async Task DadoIdNomeTipoDtoValido_QuandoAlterar_EntaoRetornaOkComDtoAlterado()
        {
            // Arrange
            var dto = GerarIdNomeTipoDTO();
            var dtoRetorno = GerarIdNomeTipoExcluidoDTO();

            servicoSuporteMock
                .Setup(s => s.Alterar(It.Is<IdNomeTipoExcluidoDTO>(x => x.Id == dto.Id && x.Nome == dto.Nome && x.Tipo == dto.Tipo)))
                .ReturnsAsync(dtoRetorno);

            // Act
            var resultado = await sut.Alterar(dto, servicoSuporteMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dtoRetorno);
            servicoSuporteMock.Verify(s => s.Alterar(It.Is<IdNomeTipoExcluidoDTO>(x => x.Id == dto.Id && x.Nome == dto.Nome && x.Tipo == dto.Tipo)), Times.Once);
        }

        [Fact]
        public async Task DadoTipoSuporteNaoDefinido_QuandoObterTodos_EntaoRetornaListaCompletaSemFiltro()
        {
            // Arrange
            var tipoSuporte = (TipoSuporte)0; // Equivalente a TipoSuporte.NAO_DEFINIDO
            var listaCompleta = GerarListaIdNomeTipoExcluidoDTO(5);

            servicoSuporteMock
                .Setup(s => s.ObterTodos())
                .ReturnsAsync(listaCompleta);

            // Act
            var resultado = await sut.ObterTodos(tipoSuporte, servicoSuporteMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            var valorRetorno = okResult.Value.Should().BeAssignableTo<IEnumerable<IdNomeTipoExcluidoDTO>>().Subject;
            valorRetorno.Should().HaveCount(5);
            servicoSuporteMock.Verify(s => s.ObterTodos(), Times.Once);
        }

        [Fact]
        public async Task DadoTipoSuporteDefinido_QuandoObterTodos_EntaoRetornaListaFiltradaPorTipo()
        {
            // Arrange
            var tipoSuporteFiltrado = 1;
            var tipoSuporteEnum = (TipoSuporte)tipoSuporteFiltrado;
            var listaCompleta = GerarListaIdNomeTipoExcluidoDTO(5);

            listaCompleta[0].Tipo = tipoSuporteFiltrado;
            listaCompleta[1].Tipo = tipoSuporteFiltrado;
            listaCompleta[2].Tipo = 99; // Outro tipo qualquer

            var listaFiltradaEsperada = listaCompleta.Where(w => w.Tipo == tipoSuporteFiltrado).ToList();

            servicoSuporteMock
                .Setup(s => s.ObterTodos())
                .ReturnsAsync(listaCompleta);

            // Act
            var resultado = await sut.ObterTodos(tipoSuporteEnum, servicoSuporteMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            var valorRetorno = okResult.Value.Should().BeAssignableTo<IEnumerable<IdNomeTipoExcluidoDTO>>().Subject;
            valorRetorno.Should().HaveCount(listaFiltradaEsperada.Count);
            valorRetorno.Should().BeEquivalentTo(listaFiltradaEsperada);
            servicoSuporteMock.Verify(s => s.ObterTodos(), Times.Once);
        }

        [Fact]
        public async Task DadoIdValido_QuandoObterPorId_EntaoRetornaOkComDto()
        {
            // Arrange
            var id = new Faker().Random.Long(1, 1000);
            var dtoRetorno = GerarIdNomeTipoExcluidoDTO();
            dtoRetorno.Id = id;

            servicoSuporteMock
                .Setup(s => s.ObterPorId(id))
                .ReturnsAsync(dtoRetorno);

            // Act
            var resultado = await sut.ObterTodos(id, servicoSuporteMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dtoRetorno);
            servicoSuporteMock.Verify(s => s.ObterPorId(id), Times.Once);
        }

        [Fact]
        public async Task DadoIdValido_QuandoExclusaoLogica_EntaoRetornaOkComResultadoBooleano()
        {
            // Arrange
            var id = new Faker().Random.Long(1, 1000);
            var exclusaoSucesso = true;

            servicoSuporteMock
                .Setup(s => s.Excluir(id))
                .ReturnsAsync(exclusaoSucesso);

            // Act
            var resultado = await sut.ExclusaoLogica(id, servicoSuporteMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(exclusaoSucesso);
            servicoSuporteMock.Verify(s => s.Excluir(id), Times.Once);
        }

        // ================= HELPER BOGUS GENERATORS ================= //

        private static NomeTipoDTO GerarNomeTipoDTO() => new Faker<NomeTipoDTO>("pt_BR")
            .RuleFor(x => x.Nome, f => f.Commerce.ProductName())
            .RuleFor(x => x.Tipo, f => f.Random.Int(1, 5))
            .Generate();

        private static IdNomeTipoDTO GerarIdNomeTipoDTO() => new Faker<IdNomeTipoDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Nome, f => f.Commerce.ProductName())
            .RuleFor(x => x.Tipo, f => f.Random.Int(1, 5))
            .Generate();

        private static IdNomeTipoExcluidoDTO GerarIdNomeTipoExcluidoDTO() => new Faker<IdNomeTipoExcluidoDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Nome, f => f.Commerce.ProductName())
            .RuleFor(x => x.Tipo, f => f.Random.Int(1, 5))
            .RuleFor(x => x.Excluido, f => f.Random.Bool())
            .Generate();

        private static List<IdNomeTipoExcluidoDTO> GerarListaIdNomeTipoExcluidoDTO(int quantidade) => new Faker<IdNomeTipoExcluidoDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Nome, f => f.Commerce.ProductName())
            .RuleFor(x => x.Tipo, f => f.Random.Int(1, 5))
            .RuleFor(x => x.Excluido, f => f.Random.Bool())
            .Generate(quantidade)
            .ToList();
    }
}