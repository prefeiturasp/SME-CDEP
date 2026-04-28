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
    public class MaterialControllerTestes
    {
        private readonly Mock<IServicoMaterial> servicoMaterialMock;
        private readonly MaterialController sut;

        public MaterialControllerTestes()
        {
            var mocker = new AutoMocker();

            servicoMaterialMock = mocker.GetMock<IServicoMaterial>();

            sut = mocker.CreateInstance<MaterialController>();
        }

        [Fact]
        public async Task DadoNomeTipoDtoValido_QuandoInserir_EntaoRetornaOkComIdGerado()
        {
            // Arrange
            var dto = GerarNomeTipoDTO();
            var idGerado = new Faker().Random.Long(1, 1000);

            servicoMaterialMock
                .Setup(s => s.Inserir(It.Is<IdNomeTipoExcluidoDTO>(x => x.Nome == dto.Nome && x.Tipo == dto.Tipo)))
                .ReturnsAsync(idGerado);

            // Act
            var resultado = await sut.Inserir(dto, servicoMaterialMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(idGerado);
            servicoMaterialMock.Verify(s => s.Inserir(It.Is<IdNomeTipoExcluidoDTO>(x => x.Nome == dto.Nome && x.Tipo == dto.Tipo)), Times.Once);
        }

        [Fact]
        public async Task DadoIdNomeTipoDtoValido_QuandoAlterar_EntaoRetornaOkComDtoAlterado()
        {
            // Arrange
            var dto = GerarIdNomeTipoDTO();
            var dtoRetorno = GerarIdNomeTipoExcluidoDTO();

            servicoMaterialMock
                .Setup(s => s.Alterar(It.Is<IdNomeTipoExcluidoDTO>(x => x.Id == dto.Id && x.Nome == dto.Nome && x.Tipo == dto.Tipo)))
                .ReturnsAsync(dtoRetorno);

            // Act
            var resultado = await sut.Alterar(dto, servicoMaterialMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dtoRetorno);
            servicoMaterialMock.Verify(s => s.Alterar(It.Is<IdNomeTipoExcluidoDTO>(x => x.Id == dto.Id && x.Nome == dto.Nome && x.Tipo == dto.Tipo)), Times.Once);
        }

        [Fact]
        public async Task DadoTipoMaterialNaoDefinido_QuandoObterTodos_EntaoRetornaListaCompletaSemFiltro()
        {
            // Arrange
            var tipoMaterial = TipoMaterial.NAO_DEFINIDO;
            var listaCompleta = GerarListaIdNomeTipoExcluidoDTO(5);

            servicoMaterialMock
                .Setup(s => s.ObterTodos())
                .ReturnsAsync(listaCompleta);

            // Act
            var resultado = await sut.ObterTodos(tipoMaterial, servicoMaterialMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            var valorRetorno = okResult.Value.Should().BeAssignableTo<IEnumerable<IdNomeTipoExcluidoDTO>>().Subject;
            valorRetorno.Should().HaveCount(5);
            servicoMaterialMock.Verify(s => s.ObterTodos(), Times.Once);
        }

        [Fact]
        public async Task DadoTipoMaterialDefinido_QuandoObterTodos_EntaoRetornaListaFiltradaPorTipo()
        {
            // Arrange
            var tipoMaterial = TipoMaterial.DOCUMENTAL;
            var listaCompleta = GerarListaIdNomeTipoExcluidoDTO(5);

            // Garantindo que teremos itens com o tipo filtrado e outros com tipos diferentes
            listaCompleta[0].Tipo = (int)TipoMaterial.DOCUMENTAL;
            listaCompleta[1].Tipo = (int)TipoMaterial.DOCUMENTAL;
            listaCompleta[2].Tipo = (int)TipoMaterial.BIBLIOGRAFICO;

            var listaFiltradaEsperada = listaCompleta.Where(w => w.Tipo == (int)tipoMaterial).ToList();

            servicoMaterialMock
                .Setup(s => s.ObterTodos())
                .ReturnsAsync(listaCompleta);

            // Act
            var resultado = await sut.ObterTodos(tipoMaterial, servicoMaterialMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            var valorRetorno = okResult.Value.Should().BeAssignableTo<IEnumerable<IdNomeTipoExcluidoDTO>>().Subject;
            valorRetorno.Should().HaveCount(listaFiltradaEsperada.Count);
            valorRetorno.Should().BeEquivalentTo(listaFiltradaEsperada);
            servicoMaterialMock.Verify(s => s.ObterTodos(), Times.Once);
        }

        [Fact]
        public async Task DadoIdValido_QuandoObterPorId_EntaoRetornaOkComDto()
        {
            // Arrange
            var id = new Faker().Random.Long(1, 1000);
            var dtoRetorno = GerarIdNomeTipoExcluidoDTO();
            dtoRetorno.Id = id;

            servicoMaterialMock
                .Setup(s => s.ObterPorId(id))
                .ReturnsAsync(dtoRetorno);

            // Act
            var resultado = await sut.ObterTodos(id, servicoMaterialMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dtoRetorno);
            servicoMaterialMock.Verify(s => s.ObterPorId(id), Times.Once);
        }

        [Fact]
        public async Task DadoIdValido_QuandoExclusaoLogica_EntaoRetornaOkComResultadoBooleano()
        {
            // Arrange
            var id = new Faker().Random.Long(1, 1000);
            var exclusaoSucesso = true;

            servicoMaterialMock
                .Setup(s => s.Excluir(id))
                .ReturnsAsync(exclusaoSucesso);

            // Act
            var resultado = await sut.ExclusaoLogica(id, servicoMaterialMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(exclusaoSucesso);
            servicoMaterialMock.Verify(s => s.Excluir(id), Times.Once);
        }

        // ================= HELPER BOGUS GENERATORS ================= //

        private static NomeTipoDTO GerarNomeTipoDTO() => new Faker<NomeTipoDTO>("pt_BR")
            .RuleFor(x => x.Nome, f => f.Commerce.ProductName())
            .RuleFor(x => x.Tipo, f => (int)f.PickRandom<TipoMaterial>())
            .Generate();

        private static IdNomeTipoDTO GerarIdNomeTipoDTO() => new Faker<IdNomeTipoDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Nome, f => f.Commerce.ProductName())
            .RuleFor(x => x.Tipo, f => (int)f.PickRandom<TipoMaterial>())
            .Generate();

        private static IdNomeTipoExcluidoDTO GerarIdNomeTipoExcluidoDTO() => new Faker<IdNomeTipoExcluidoDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Nome, f => f.Commerce.ProductName())
            .RuleFor(x => x.Tipo, f => (int)f.PickRandom<TipoMaterial>())
            .RuleFor(x => x.Excluido, f => f.Random.Bool())
            .Generate();

        private static List<IdNomeTipoExcluidoDTO> GerarListaIdNomeTipoExcluidoDTO(int quantidade) => new Faker<IdNomeTipoExcluidoDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Nome, f => f.Commerce.ProductName())
            .RuleFor(x => x.Tipo, f => (int)f.PickRandom<TipoMaterial>())
            .RuleFor(x => x.Excluido, f => f.Random.Bool())
            .Generate(quantidade)
            .ToList();
    }
}