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
    public class AcervoEmprestimoControllerTestes
    {
        private readonly Mock<IServicoAcervoEmprestimo> servicoAcervoEmprestimoMock;
        private readonly AcervoEmprestimoController sut;

        public AcervoEmprestimoControllerTestes()
        {
            var mocker = new AutoMocker();

            servicoAcervoEmprestimoMock = mocker.GetMock<IServicoAcervoEmprestimo>();

            sut = mocker.CreateInstance<AcervoEmprestimoController>();
        }

        [Fact]
        public async Task DadoProrrogacaoValida_QuandoProrrogarEmprestimo_EntaoRetornaOkComResultadoBooleano()
        {
            // Arrange
            var dto = GerarAcervoEmprestimoProrrogacaoDTO();
            var prorrogadoComSucesso = true;

            servicoAcervoEmprestimoMock
                .Setup(s => s.ProrrogarEmprestimo(dto))
                .ReturnsAsync(prorrogadoComSucesso);

            // Act
            var resultado = await sut.ProrrogarEmprestimo(dto, servicoAcervoEmprestimoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(prorrogadoComSucesso);
            servicoAcervoEmprestimoMock.Verify(s => s.ProrrogarEmprestimo(dto), Times.Once);
        }

        [Fact]
        public async Task DadoRequisicaoValida_QuandoObterSituacoesEmprestimo_EntaoRetornaOkComListaDeSituacoes()
        {
            // Arrange
            var listaSituacoes = GerarListaSituacaoItemDTO(3);

            servicoAcervoEmprestimoMock
                .Setup(s => s.ObterSituacoesEmprestimo())
                .ReturnsAsync(listaSituacoes);

            // Act
            var resultado = await sut.ObterSituacoesEmprestimo(servicoAcervoEmprestimoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(listaSituacoes);
            servicoAcervoEmprestimoMock.Verify(s => s.ObterSituacoesEmprestimo(), Times.Once);
        }

        [Fact]
        public async Task DadoItemIdValido_QuandoDevolverItemEmprestado_EntaoRetornaOkComResultadoBooleano()
        {
            // Arrange
            var acervoSolicitacaoItemId = new Faker().Random.Long(1, 1000);
            var devolvidoComSucesso = true;

            servicoAcervoEmprestimoMock
                .Setup(s => s.DevolverItemEmprestado(acervoSolicitacaoItemId))
                .ReturnsAsync(devolvidoComSucesso);

            // Act
            var resultado = await sut.DevolverItemEmprestado(acervoSolicitacaoItemId, servicoAcervoEmprestimoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(devolvidoComSucesso);
            servicoAcervoEmprestimoMock.Verify(s => s.DevolverItemEmprestado(acervoSolicitacaoItemId), Times.Once);
        }

        // ================= HELPER BOGUS GENERATORS ================= //

        private static AcervoEmprestimoProrrogacaoDTO GerarAcervoEmprestimoProrrogacaoDTO() => new Faker<AcervoEmprestimoProrrogacaoDTO>("pt_BR")
            .RuleFor(x => x.AcervoSolicitacaoItemId, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.DataDevolucao, f => f.Date.Future())
            .Generate();

        private static List<SituacaoItemDTO> GerarListaSituacaoItemDTO(int quantidade) => new Faker<SituacaoItemDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 100))
            .RuleFor(x => x.Nome, f => f.Commerce.Department())
            .Generate(quantidade)
            .ToList();
    }
}