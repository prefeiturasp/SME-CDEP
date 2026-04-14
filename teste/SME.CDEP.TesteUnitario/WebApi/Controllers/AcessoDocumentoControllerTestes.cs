using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Webapi.Controllers;

namespace SME.CDEP.TesteUnitario.Webapi.Controllers
{
    public class AcessoDocumentoControllerTestes
    {
        private readonly Mock<IServicoAcessoDocumento> servicoAcessoDocumentoMock;
        private readonly AcessoDocumentoController sut;

        public AcessoDocumentoControllerTestes()
        {
            var mocker = new AutoMocker();

            servicoAcessoDocumentoMock = mocker.GetMock<IServicoAcessoDocumento>();

            sut = mocker.CreateInstance<AcessoDocumentoController>();
        }

        [Fact]
        public async Task DadoNomeDtoValido_QuandoInserir_EntaoRetornaOkComId()
        {
            // Arrange
            var dto = GerarNomeDTO();
            var idGerado = new Faker().Random.Long(1, 1000);

            servicoAcessoDocumentoMock
                .Setup(s => s.Inserir(It.Is<IdNomeExcluidoDTO>(x => x.Nome == dto.Nome)))
                .ReturnsAsync(idGerado);

            // Act
            var resultado = await sut.Inserir(dto, servicoAcessoDocumentoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(idGerado);
            servicoAcessoDocumentoMock.Verify(s => s.Inserir(It.Is<IdNomeExcluidoDTO>(x => x.Nome == dto.Nome)), Times.Once);
        }

        [Fact]
        public async Task DadoIdNomeDtoValido_QuandoAlterar_EntaoRetornaOkComDtoAlterado()
        {
            // Arrange
            var dto = GerarIdNomeDTO();
            var dtoRetorno = GerarIdNomeExcluidoDTO();

            servicoAcessoDocumentoMock
                .Setup(s => s.Alterar(It.Is<IdNomeExcluidoDTO>(x => x.Id == dto.Id && x.Nome == dto.Nome)))
                .ReturnsAsync(dtoRetorno);

            // Act
            var resultado = await sut.Alterar(dto, servicoAcessoDocumentoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dtoRetorno);
            servicoAcessoDocumentoMock.Verify(s => s.Alterar(It.Is<IdNomeExcluidoDTO>(x => x.Id == dto.Id && x.Nome == dto.Nome)), Times.Once);
        }

        [Fact]
        public async Task DadoChamadaValida_QuandoObterTodos_EntaoRetornaOkComLista()
        {
            // Arrange
            var listaRetorno = GerarListaIdNomeExcluidoDTO(3);

            servicoAcessoDocumentoMock
                .Setup(s => s.ObterTodos())
                .ReturnsAsync(listaRetorno);

            // Act
            var resultado = await sut.ObterTodos(servicoAcessoDocumentoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(listaRetorno);
            servicoAcessoDocumentoMock.Verify(s => s.ObterTodos(), Times.Once);
        }

        [Fact]
        public async Task DadoIdValido_QuandoObterPorId_EntaoRetornaOkComDto()
        {
            // Arrange
            var id = new Faker().Random.Long(1, 1000);
            var dtoRetorno = GerarIdNomeExcluidoDTO();
            dtoRetorno.Id = id;

            servicoAcessoDocumentoMock
                .Setup(s => s.ObterPorId(id))
                .ReturnsAsync(dtoRetorno);

            // Act
            // O nome do método na controller está ObterTodos com sobrecarga de long id
            var resultado = await sut.ObterTodos(id, servicoAcessoDocumentoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dtoRetorno);
            servicoAcessoDocumentoMock.Verify(s => s.ObterPorId(id), Times.Once);
        }

        [Fact]
        public async Task DadoIdValido_QuandoExclusaoLogica_EntaoRetornaOkComResultadoBooleano()
        {
            // Arrange
            var id = new Faker().Random.Long(1, 1000);
            var exclusaoSucesso = true;

            servicoAcessoDocumentoMock
                .Setup(s => s.Excluir(id))
                .ReturnsAsync(exclusaoSucesso);

            // Act
            var resultado = await sut.ExclusaoLogica(id, servicoAcessoDocumentoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(exclusaoSucesso);
            servicoAcessoDocumentoMock.Verify(s => s.Excluir(id), Times.Once);
        }

        // ================= HELPER BOGUS GENERATORS ================= //

        private static NomeDTO GerarNomeDTO() => new Faker<NomeDTO>("pt_BR")
            .RuleFor(x => x.Nome, f => f.Commerce.Department())
            .Generate();

        private static IdNomeDTO GerarIdNomeDTO() => new Faker<IdNomeDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Nome, f => f.Commerce.Department())
            .Generate();

        private static IdNomeExcluidoDTO GerarIdNomeExcluidoDTO() => new Faker<IdNomeExcluidoDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Nome, f => f.Commerce.Department())
            .RuleFor(x => x.Excluido, f => f.Random.Bool())
            .Generate();

        private static List<IdNomeExcluidoDTO> GerarListaIdNomeExcluidoDTO(int quantidade) => new Faker<IdNomeExcluidoDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Nome, f => f.Commerce.Department())
            .RuleFor(x => x.Excluido, f => f.Random.Bool())
            .Generate(quantidade)
            .ToList();
    }
}