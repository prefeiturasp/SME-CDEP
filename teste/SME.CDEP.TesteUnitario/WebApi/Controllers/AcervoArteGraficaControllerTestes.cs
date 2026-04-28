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
    public class AcervoArteGraficaControllerTestes
    {
        private readonly Mock<IServicoAcervoArteGrafica> servicoAcervoArteGraficaMock;
        private readonly AcervoArteGraficaController sut;

        public AcervoArteGraficaControllerTestes()
        {
            var mocker = new AutoMocker();

            servicoAcervoArteGraficaMock = mocker.GetMock<IServicoAcervoArteGrafica>();

            sut = mocker.CreateInstance<AcervoArteGraficaController>();
        }

        [Fact]
        public async Task DadoAcervoArteGraficaCadastroValido_QuandoInserir_EntaoRetornaOkComId()
        {
            // Arrange
            var dto = GerarAcervoArteGraficaCadastroDTO();
            var idGerado = new Faker().Random.Long(1, 1000);

            servicoAcervoArteGraficaMock
                .Setup(s => s.Inserir(dto))
                .ReturnsAsync(idGerado);

            // Act
            var resultado = await sut.Inserir(dto, servicoAcervoArteGraficaMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(idGerado);
            servicoAcervoArteGraficaMock.Verify(s => s.Inserir(dto), Times.Once);
        }

        [Fact]
        public async Task DadoAcervoArteGraficaAlteracaoValida_QuandoAlterar_EntaoRetornaOkComAcervoAlterado()
        {
            // Arrange
            var dto = GerarAcervoArteGraficaAlteracaoDTO();
            var dtoRetorno = GerarAcervoArteGraficaDTO();

            servicoAcervoArteGraficaMock
                .Setup(s => s.Alterar(dto))
                .ReturnsAsync(dtoRetorno);

            // Act
            var resultado = await sut.Alterar(dto, servicoAcervoArteGraficaMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dtoRetorno);
            servicoAcervoArteGraficaMock.Verify(s => s.Alterar(dto), Times.Once);
        }

        [Fact]
        public async Task DadoAcervoIdValido_QuandoObterPorId_EntaoRetornaOkComAcervoEncontrado()
        {
            // Arrange
            var id = new Faker().Random.Long(1, 1000);
            var dtoRetorno = GerarAcervoArteGraficaDTO();

            servicoAcervoArteGraficaMock
                .Setup(s => s.ObterPorId(id))
                .ReturnsAsync(dtoRetorno);

            // Act
            var resultado = await sut.ObterPorId(id, servicoAcervoArteGraficaMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dtoRetorno);
            servicoAcervoArteGraficaMock.Verify(s => s.ObterPorId(id), Times.Once);
        }

        [Fact]
        public async Task DadoAcervoIdValido_QuandoExclusaoLogica_EntaoRetornaOkComResultadoBooleano()
        {
            // Arrange
            var id = new Faker().Random.Long(1, 1000);
            var exclusaoSucesso = true;

            servicoAcervoArteGraficaMock
                .Setup(s => s.Excluir(id))
                .ReturnsAsync(exclusaoSucesso);

            // Act
            var resultado = await sut.ExclusaoLogica(id, servicoAcervoArteGraficaMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(exclusaoSucesso);
            servicoAcervoArteGraficaMock.Verify(s => s.Excluir(id), Times.Once);
        }

        // ================= HELPER BOGUS GENERATORS ================= //

        private static AcervoArteGraficaCadastroDTO GerarAcervoArteGraficaCadastroDTO() => new Faker<AcervoArteGraficaCadastroDTO>("pt_BR")
            .RuleFor(x => x.Titulo, f => f.Lorem.Sentence(3))
            .RuleFor(x => x.Descricao, f => f.Lorem.Paragraph())
            .RuleFor(x => x.Codigo, f => f.Random.AlphaNumeric(10))
            .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
            .RuleFor(x => x.SituacaoAcervo, f => f.PickRandom<SituacaoAcervo>())
            .RuleFor(x => x.Localizacao, f => f.Address.FullAddress())
            .RuleFor(x => x.Procedencia, f => f.Lorem.Sentence())
            .RuleFor(x => x.CopiaDigital, f => f.Random.Bool())
            .RuleFor(x => x.PermiteUsoImagem, f => f.Random.Bool())
            .RuleFor(x => x.ConservacaoId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.CromiaId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.Tecnica, f => f.Lorem.Word())
            .RuleFor(x => x.SuporteId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.Quantidade, f => f.Random.Long(1, 50))
            .Generate();

        private static AcervoArteGraficaAlteracaoDTO GerarAcervoArteGraficaAlteracaoDTO() => new Faker<AcervoArteGraficaAlteracaoDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.AcervoId, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Titulo, f => f.Lorem.Sentence(3))
            .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
            .RuleFor(x => x.Procedencia, f => f.Lorem.Sentence())
            .RuleFor(x => x.ConservacaoId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.CromiaId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.SuporteId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.Quantidade, f => f.Random.Long(1, 50))
            .Generate();

        private static AcervoArteGraficaDTO GerarAcervoArteGraficaDTO() => new Faker<AcervoArteGraficaDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.AcervoId, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Titulo, f => f.Lorem.Sentence(3))
            .RuleFor(x => x.Codigo, f => f.Random.AlphaNumeric(10))
            .RuleFor(x => x.Localizacao, f => f.Address.FullAddress())
            .RuleFor(x => x.Procedencia, f => f.Lorem.Sentence())
            .RuleFor(x => x.Descricao, f => f.Lorem.Paragraph())
            .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
            .Generate();
    }
}