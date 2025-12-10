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

        [Fact]
        public async Task DadoQueExistemSolicitacoesNoBanco_QuandoObterQuantidadeSolicitacoesMensais_EntaoDeveRetornarOkComListaDeDtos()
        {
            // Arrange
            var quantidadeSolicitacoesMensaisDto = new List<PainelGerencialQuantidadeSolicitacaoMensalDto>
            {
                new() { Id = 1, Nome = "Janeiro", Valor = 80 },
                new() { Id = 2, Nome = "Fevereiro", Valor = 60 }
            };
            _servicoPainelGerencialMock
                .Setup(s => s.ObterQuantidadeSolicitacoesMensaisDoAnoAtualAsync())
                .ReturnsAsync(quantidadeSolicitacoesMensaisDto);
            // Act
            var resultado = await _painelGerencialController.ObterQuantidadeSolicitacoesMensais();
            // Assert
            var okResult = resultado as Microsoft.AspNetCore.Mvc.OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(quantidadeSolicitacoesMensaisDto);
        }

        [Fact]
        public async Task DadoQueExistemSolicitacoesNoBanco_QuandoObterQuantidadeSolicitacoesPorTipoAcervo_EntaoDeveRetornarOkComListaDeDtos()
        {
            // Arrange
            var quantidadeSolicitacoesPorTipoAcervoDto = new List<PainelGerencialQuantidadeSolicitacaoPorTipoDeAcervoDto>
            {
                new() { Id = (int)TipoAcervo.Bibliografico, Nome = "Bibliografico", Valor = 100 },
                new() { Id = (int)TipoAcervo.Fotografico, Nome = "Fotografico", Valor = 50 }
            };
            _servicoPainelGerencialMock
                .Setup(s => s.ObterQuantidadeDeSolicitacoesPorTipoAcervoAsync())
                .ReturnsAsync(quantidadeSolicitacoesPorTipoAcervoDto);
            // Act
            var resultado = await _painelGerencialController.ObterQuantidadeSolicitacoesPorTipoAcervo();
            // Assert
            var okResult = resultado as Microsoft.AspNetCore.Mvc.OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(quantidadeSolicitacoesPorTipoAcervoDto);
        }

        [Fact]
        public async Task DadoQueExistemEmprestimosNoBanco_QuandoObterQuantidadeAcervoEmprestadoPorSituacao_EntaoDeveRetornarOkComListaDeDtos()
        {
            // Arrange
            var quantidadeAcervoEmprestadoPorSituacaoDto = new List<PainelGerencialQuantidadeAcervoEmprestadoPorSituacaoDto>
            {
                new() { Id = (int)SituacaoEmprestimo.EMPRESTADO, Nome = "Emprestado", Valor = 30 },
                new() { Id = (int)SituacaoEmprestimo.DEVOLUCAO_EM_ATRASO, Nome = "Devolução em atraso", Valor = 10 }
            };
            _servicoPainelGerencialMock
                .Setup(s => s.ObterQuantidadeAcervoEmprestadoPorSituacaoAsync())
                .ReturnsAsync(quantidadeAcervoEmprestadoPorSituacaoDto);
            // Act
            var resultado = await _painelGerencialController.ObterQuantidadeAcervoEmprestadoPorSituacao();
            // Assert
            var okResult = resultado as Microsoft.AspNetCore.Mvc.OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(quantidadeAcervoEmprestadoPorSituacaoDto);
        }
    }
}
