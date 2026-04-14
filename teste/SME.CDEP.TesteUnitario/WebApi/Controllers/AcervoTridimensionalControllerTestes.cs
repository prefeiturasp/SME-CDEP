using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Enumerados;
using SME.CDEP.Webapi.Controllers;

namespace SME.CDEP.TesteUnitario.Webapi.Controllers
{
    public class AcervoTridimensionalControllerTestes
    {
        private readonly Mock<IServicoAcervoTridimensional> servicoAcervoTridimensionalMock;
        private readonly AcervoTridimensionalController sut;

        public AcervoTridimensionalControllerTestes()
        {
            var mocker = new AutoMocker();

            servicoAcervoTridimensionalMock = mocker.GetMock<IServicoAcervoTridimensional>();

            sut = mocker.CreateInstance<AcervoTridimensionalController>();
        }

        [Fact]
        public async Task DadoAcervoTridimensionalCadastroValido_QuandoInserir_EntaoRetornaOkComId()
        {
            // Arrange
            var dto = GerarAcervoTridimensionalCadastroDTO();
            var idGerado = new Faker().Random.Long(1, 1000);

            servicoAcervoTridimensionalMock
                .Setup(s => s.Inserir(dto))
                .ReturnsAsync(idGerado);

            // Act
            var resultado = await sut.Inserir(dto, servicoAcervoTridimensionalMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(idGerado);
            servicoAcervoTridimensionalMock.Verify(s => s.Inserir(dto), Times.Once);
        }

        [Fact]
        public async Task DadoAcervoTridimensionalAlteracaoValida_QuandoAlterar_EntaoRetornaOkComAcervoAlterado()
        {
            // Arrange
            var dto = GerarAcervoTridimensionalAlteracaoDTO();
            var dtoRetorno = GerarAcervoTridimensionalDTO();

            servicoAcervoTridimensionalMock
                .Setup(s => s.Alterar(dto))
                .ReturnsAsync(dtoRetorno);

            // Act
            var resultado = await sut.Alterar(dto, servicoAcervoTridimensionalMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dtoRetorno);
            servicoAcervoTridimensionalMock.Verify(s => s.Alterar(dto), Times.Once);
        }

        [Fact]
        public async Task DadoAcervoIdValido_QuandoObterPorId_EntaoRetornaOkComAcervoEncontrado()
        {
            // Arrange
            var id = new Faker().Random.Long(1, 1000);
            var dtoRetorno = GerarAcervoTridimensionalDTO();

            servicoAcervoTridimensionalMock
                .Setup(s => s.ObterPorId(id))
                .ReturnsAsync(dtoRetorno);

            // Act
            var resultado = await sut.ObterPorId(id, servicoAcervoTridimensionalMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dtoRetorno);
            servicoAcervoTridimensionalMock.Verify(s => s.ObterPorId(id), Times.Once);
        }

        [Fact]
        public async Task DadoAcervoIdValido_QuandoExclusaoLogica_EntaoRetornaOkComResultadoBooleano()
        {
            // Arrange
            var id = new Faker().Random.Long(1, 1000);
            var exclusaoSucesso = true;

            servicoAcervoTridimensionalMock
                .Setup(s => s.Excluir(id))
                .ReturnsAsync(exclusaoSucesso);

            // Act
            var resultado = await sut.ExclusaoLogica(id, servicoAcervoTridimensionalMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(exclusaoSucesso);
            servicoAcervoTridimensionalMock.Verify(s => s.Excluir(id), Times.Once);
        }

        // ================= HELPER BOGUS GENERATORS ================= //

        private static AcervoTridimensionalCadastroDTO GerarAcervoTridimensionalCadastroDTO() => new Faker<AcervoTridimensionalCadastroDTO>("pt_BR")
            .RuleFor(x => x.Titulo, f => f.Lorem.Sentence(3))
            .RuleFor(x => x.Descricao, f => f.Lorem.Paragraph())
            .RuleFor(x => x.Codigo, f => f.Random.AlphaNumeric(10))
            .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
            .RuleFor(x => x.SituacaoAcervo, f => f.PickRandom<SituacaoAcervo>())
            .RuleFor(x => x.Procedencia, f => f.Lorem.Sentence())
            .RuleFor(x => x.ConservacaoId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.Quantidade, f => f.Random.Int(1, 50))
            .RuleFor(x => x.Largura, f => f.Random.Double(10, 50).ToString("N2"))
            .RuleFor(x => x.Altura, f => f.Random.Double(10, 50).ToString("N2"))
            .RuleFor(x => x.Profundidade, f => f.Random.Double(10, 50).ToString("N2"))
            .RuleFor(x => x.Diametro, f => f.Random.Double(10, 50).ToString("N2"))
            .Generate();

        private static AcervoTridimensionalAlteracaoDTO GerarAcervoTridimensionalAlteracaoDTO() => new Faker<AcervoTridimensionalAlteracaoDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.AcervoId, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Titulo, f => f.Lorem.Sentence(3))
            .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
            .RuleFor(x => x.Procedencia, f => f.Lorem.Sentence())
            .RuleFor(x => x.ConservacaoId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.Quantidade, f => f.Random.Int(1, 50))
            .Generate();

        private static AcervoTridimensionalDTO GerarAcervoTridimensionalDTO() => new Faker<AcervoTridimensionalDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.AcervoId, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Titulo, f => f.Lorem.Sentence(3))
            .RuleFor(x => x.Codigo, f => f.Random.AlphaNumeric(10))
            .RuleFor(x => x.Procedencia, f => f.Lorem.Sentence())
            .RuleFor(x => x.Descricao, f => f.Lorem.Paragraph())
            .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
            .RuleFor(x => x.Largura, f => f.Random.Double(10, 50).ToString("N2"))
            .RuleFor(x => x.Altura, f => f.Random.Double(10, 50).ToString("N2"))
            .RuleFor(x => x.Profundidade, f => f.Random.Double(10, 50).ToString("N2"))
            .Generate();
    }
}