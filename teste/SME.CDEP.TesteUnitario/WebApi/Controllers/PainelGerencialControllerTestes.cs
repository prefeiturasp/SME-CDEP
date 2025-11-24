using Bogus;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Controllers;

namespace SME.CDEP.TesteUnitario.WebApi.Controllers
{
    public class PainelGerencialControllerTestes
    {
        private readonly Mock<IServicoPainelGerencial> _servicoPainelGerencialMock;
        private readonly PainelGerencialController _painelGerencialController;
        private readonly Faker _faker;

        public PainelGerencialControllerTestes()
        {
            var mocker = new AutoMocker();
            _servicoPainelGerencialMock = mocker.GetMock<IServicoPainelGerencial>();
            _painelGerencialController = mocker.CreateInstance<PainelGerencialController>();
            _faker = new();
        }

        [Fact]
        public async Task DadoQueExistemAcervosNoBanco_QuandoObterAcervosCadastrados_EntaoDeveRetornarOkComListaDeDtos()
        {
            // Arrange
            var acervosCadastradosDto = new List<PainelGerencialAcervosCadastradosDto>
            {
                new() { Id = TipoAcervo.Bibliografico, Nome = "Bibliografico", Valor = 100 },
                new() { Id = TipoAcervo.Fotografico, Nome = "Fotografico", Valor = 50 }
            };
            _servicoPainelGerencialMock
                .Setup(s => s.ObterAcervosCadastradosAsync())
                .ReturnsAsync(acervosCadastradosDto);
            // Act
            var resultado = await _painelGerencialController.ObterAcervosCadastrados();
            // Assert
            var okResult = resultado as Microsoft.AspNetCore.Mvc.OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(acervosCadastradosDto);
        }

        [Fact]
        public async Task DadoQueExistemPesquisasNoBanco_QuandoObterQuantidadePesquisasMensais_EntaoDeveRetornarOkComListaDeDtos()
        {
            // Arrange
            var quantidadePesquisasMensaisDto = new List<PainelGerencialQuantidadePesquisasMensaisDto>
            {
                new() { Id = 1, Nome = "Janeiro", Valor = 200 },
                new() { Id = 2, Nome = "Fevereiro", Valor = 150 }
            };
            _servicoPainelGerencialMock
                .Setup(s => s.ObterQuantidadePesquisasMensaisDoAnoAtualAsync())
                .ReturnsAsync(quantidadePesquisasMensaisDto);

            // Act
            var resultado = await _painelGerencialController.ObterQuantidadePesquisasMensais();

            // Assert
            var okResult = resultado as Microsoft.AspNetCore.Mvc.OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(quantidadePesquisasMensaisDto);
        }
    }
}
