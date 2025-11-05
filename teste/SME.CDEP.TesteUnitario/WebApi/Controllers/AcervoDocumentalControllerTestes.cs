using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Webapi.Controllers;

namespace SME.CDEP.TesteUnitario.WebApi.Controllers
{
    public class AcervoDocumentalControllerTestes
    {
        private readonly Mock<IServicoAcervoDocumental> _servicoMock;
        private readonly AcervoDocumentalController _controller;
        private readonly Faker _faker;

        public AcervoDocumentalControllerTestes()
        {
            _servicoMock = new Mock<IServicoAcervoDocumental>();
            _controller = new AcervoDocumentalController();
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public async Task Inserir_ComDadosValidos_DeveRetornarOkComIdDoNovoAcervo()
        {
            // Arrange
            var acervoDto = new AcervoDocumentalCadastroDTO
            {
                Titulo = _faker.Lorem.Sentence(),
                Ano = "2024",
                IdiomaId = 1,
                NumeroPagina = 100,
                AcessoDocumentosIds = new long[] { 1 }
            };
            var idEsperado = _faker.Random.Long(1, 1000);

            _servicoMock.Setup(s => s.Inserir(acervoDto)).ReturnsAsync(idEsperado);

            // Act
            var resultado = await _controller.Inserir(acervoDto, _servicoMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be(idEsperado);

            _servicoMock.Verify(s => s.Inserir(acervoDto), Times.Once);
        }

        [Fact]
        public async Task Alterar_ComDadosValidos_DeveRetornarOkComAcervoAlterado()
        {
            // Arrange
            var acervoDto = new AcervoDocumentalAlteracaoDTO
            {
                Id = 1,
                AcervoId = 10,
                Titulo = _faker.Lorem.Sentence(),
                Ano = "2023",
                IdiomaId = 1,
                NumeroPagina = 150,
                AcessoDocumentosIds = new long[] { 1, 2 }
            };

            var dtoRetorno = new AcervoDocumentalDTO { Id = acervoDto.Id, Titulo = acervoDto.Titulo };

            _servicoMock.Setup(s => s.Alterar(acervoDto)).ReturnsAsync(dtoRetorno);

            // Act
            var resultado = await _controller.Alterar(acervoDto, _servicoMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var valorRetornado = Assert.IsType<AcervoDocumentalDTO>(okResult.Value);
            valorRetornado.Should().BeEquivalentTo(dtoRetorno);

            _servicoMock.Verify(s => s.Alterar(acervoDto), Times.Once);
        }

        [Fact]
        public async Task ObterPorId_ComIdValido_DeveRetornarOkComAcervo()
        {
            // Arrange
            var acervoId = _faker.Random.Long(1, 1000);
            var dtoEsperado = new AcervoDocumentalDTO { Id = acervoId, Titulo = "Documento de Teste" };

            _servicoMock.Setup(s => s.ObterPorId(acervoId)).ReturnsAsync(dtoEsperado);

            // Act
            var resultado = await _controller.ObterPorId(acervoId, _servicoMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var valorRetornado = Assert.IsType<AcervoDocumentalDTO>(okResult.Value);
            valorRetornado.Id.Should().Be(acervoId);
            valorRetornado.Titulo.Should().Be(dtoEsperado.Titulo);

            _servicoMock.Verify(s => s.ObterPorId(acervoId), Times.Once);
        }

        [Fact]
        public async Task ExclusaoLogica_ComIdValido_DeveRetornarOkComResultadoBooleano()
        {
            // Arrange
            var acervoId = _faker.Random.Long(1, 1000);
            _servicoMock.Setup(s => s.Excluir(acervoId)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.ExclusaoLogica(acervoId, _servicoMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            okResult.Value.Should().Be(true);

            _servicoMock.Verify(s => s.Excluir(acervoId), Times.Once);
        }
    }
}
