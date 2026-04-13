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
    public class AssuntoControllerTestes
    {
        private readonly Mock<IServicoAssunto> servicoAssuntoMock;
        private readonly AssuntoController sut;

        public AssuntoControllerTestes()
        {
            var mocker = new AutoMocker();

            servicoAssuntoMock = mocker.GetMock<IServicoAssunto>();

            sut = mocker.CreateInstance<AssuntoController>();
        }

        [Fact]
        public async Task DadoNomeDtoValido_QuandoInserir_EntaoRetornaOkComId()
        {
            // Arrange
            var dto = GerarNomeDTO();
            var idGerado = new Faker().Random.Long(1, 1000);

            servicoAssuntoMock
                .Setup(s => s.Inserir(It.Is<IdNomeExcluidoAuditavelDTO>(x => x.Nome == dto.Nome)))
                .ReturnsAsync(idGerado);

            // Act
            var resultado = await sut.Inserir(dto, servicoAssuntoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(idGerado);
            servicoAssuntoMock.Verify(s => s.Inserir(It.Is<IdNomeExcluidoAuditavelDTO>(x => x.Nome == dto.Nome)), Times.Once);
        }

        [Fact]
        public async Task DadoIdNomeDtoValido_QuandoAlterar_EntaoRetornaOkComDtoAlterado()
        {
            // Arrange
            var dto = GerarIdNomeDTO();
            var dtoAlterado = GerarIdNomeExcluidoAuditavelDTO().Generate();

            servicoAssuntoMock
                .Setup(s => s.Alterar(It.Is<IdNomeExcluidoAuditavelDTO>(x => x.Id == dto.Id && x.Nome == dto.Nome)))
                .ReturnsAsync(dtoAlterado);

            // Act
            var resultado = await sut.Alterar(dto, servicoAssuntoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dtoAlterado);
            servicoAssuntoMock.Verify(s => s.Alterar(It.Is<IdNomeExcluidoAuditavelDTO>(x => x.Id == dto.Id && x.Nome == dto.Nome)), Times.Once);
        }

        [Fact]
        public async Task DadoNome_QuandoObterTodosOuPorNome_EntaoRetornaOkComResultadoPaginado()
        {
            // Arrange
            var nomePesquisa = new Faker().Commerce.Department();
            var resultadoPaginado = GerarPaginacaoResultadoDTO();

            servicoAssuntoMock
                .Setup(s => s.ObterPaginado(nomePesquisa))
                .ReturnsAsync(resultadoPaginado);

            // Act
            var resultado = await sut.ObterTodosOuPorNome(nomePesquisa, servicoAssuntoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(resultadoPaginado);
            servicoAssuntoMock.Verify(s => s.ObterPaginado(nomePesquisa), Times.Once);
        }

        [Fact]
        public async Task DadoChamadaValida_QuandoObterTodos_EntaoRetornaOkComListaResumida()
        {
            // Arrange
            var listaCompleta = GerarListaIdNomeExcluidoAuditavelDTO(3);
            var listaResumidaEsperada = listaCompleta.Select(s => new IdNomeDTO { Id = s.Id, Nome = s.Nome }).ToList();

            servicoAssuntoMock
                .Setup(s => s.ObterTodos())
                .ReturnsAsync(listaCompleta);

            // Act
            var resultado = await sut.ObterTodos(servicoAssuntoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(listaResumidaEsperada);
            servicoAssuntoMock.Verify(s => s.ObterTodos(), Times.Once);
        }

        [Fact]
        public async Task DadoIdValido_QuandoObterPorId_EntaoRetornaOkComDto()
        {
            // Arrange
            var id = new Faker().Random.Long(1, 1000);
            var dtoEsperado = GerarIdNomeExcluidoAuditavelDTO().Generate();
            dtoEsperado.Id = id;

            servicoAssuntoMock
                .Setup(s => s.ObterPorId(id))
                .ReturnsAsync(dtoEsperado);

            // Act
            var resultado = await sut.ObterPorId(id, servicoAssuntoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dtoEsperado);
            servicoAssuntoMock.Verify(s => s.ObterPorId(id), Times.Once);
        }

        [Fact]
        public async Task DadoIdValido_QuandoExclusaoLogica_EntaoRetornaOkComVerdadeiro()
        {
            // Arrange
            var id = new Faker().Random.Long(1, 1000);

            servicoAssuntoMock
                .Setup(s => s.Excluir(id))
                .ReturnsAsync(true);

            // Act
            var resultado = await sut.ExclusaoLogica(id, servicoAssuntoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);
            servicoAssuntoMock.Verify(s => s.Excluir(id), Times.Once);
        }

        // ================= HELPER BOGUS GENERATORS ================= //

        private static NomeDTO GerarNomeDTO() => new Faker<NomeDTO>("pt_BR")
            .RuleFor(x => x.Nome, f => f.Commerce.Department())
            .Generate();

        private static IdNomeDTO GerarIdNomeDTO() => new Faker<IdNomeDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Nome, f => f.Commerce.Department())
            .Generate();

        private static Faker<IdNomeExcluidoAuditavelDTO> GerarIdNomeExcluidoAuditavelDTO() => new Faker<IdNomeExcluidoAuditavelDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Nome, f => f.Commerce.Department())
            .RuleFor(x => x.Excluido, f => f.Random.Bool())
            .RuleFor(x => x.AlteradoEm, f => f.Date.Recent())
            .RuleFor(x => x.AlteradoPor, f => f.Name.FullName())
            .RuleFor(x => x.AlteradoLogin, f => f.Internet.UserName())
            .RuleFor(x => x.CriadoEm, f => f.Date.Past())
            .RuleFor(x => x.CriadoPor, f => f.Name.FullName())
            .RuleFor(x => x.CriadoLogin, f => f.Internet.UserName())
            ;

        private static List<IdNomeExcluidoAuditavelDTO> GerarListaIdNomeExcluidoAuditavelDTO(int quantidade) =>
            GerarIdNomeExcluidoAuditavelDTO().Generate(quantidade).ToList();

        private static PaginacaoResultadoDTO<IdNomeExcluidoAuditavelDTO> GerarPaginacaoResultadoDTO() => new Faker<PaginacaoResultadoDTO<IdNomeExcluidoAuditavelDTO>>("pt_BR")
            .RuleFor(x => x.TotalPaginas, f => f.Random.Int(1, 10))
            .RuleFor(x => x.TotalRegistros, f => f.Random.Int(10, 100))
            .RuleFor(x => x.Items, GerarListaIdNomeExcluidoAuditavelDTO(5))
            .Generate();
    }
}