using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.UseCase.Interface;
using SME.CDEP.Webapi.Controllers;

namespace SME.CDEP.TesteUnitario.WebApi.Controllers
{
    public class RelatorioControllerTests
    {
        private readonly AutoMocker _mocker;
        private readonly RelatorioController _controller;

        public RelatorioControllerTests()
        {
            _mocker = new AutoMocker();
            _controller = _mocker.CreateInstance<RelatorioController>();
        }

        #region RelatorioControleLivrosEmprestados

        [Fact]
        public async Task DadoFiltrosValidos_QuandoSolicitarRelatorioLivrosEmprestados_EntaoDeveRetornarArquivo()
        {
            // Arrange
            var request = new Faker<RelatorioControleLivroEmprestadosRequest>().Generate();
            var streamRetorno = new MemoryStream();
            var useCaseMock = _mocker.GetMock<IRelatorioControleLivrosEmprestadosUseCase>();

            useCaseMock.Setup(x => x.ExecutarAsync(request))
                       .ReturnsAsync(streamRetorno);

            // Act
            var resultado = await _controller.RelatorioControleLivrosEmprestados(request, useCaseMock.Object);

            // Assert
            var fileResult = Assert.IsType<FileStreamResult>(resultado);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
            Assert.Equal("relatorio.xlsx", fileResult.FileDownloadName);
        }

        [Fact]
        public async Task DadoRetornoNulo_QuandoSolicitarRelatorioLivrosEmprestados_EntaoDeveRetornarNoContent()
        {
            // Arrange
            var request = new Faker<RelatorioControleLivroEmprestadosRequest>().Generate();
            var useCaseMock = _mocker.GetMock<IRelatorioControleLivrosEmprestadosUseCase>();

            useCaseMock.Setup(x => x.ExecutarAsync(request))
                       .ReturnsAsync((Stream?)null);

            // Act
            var resultado = await _controller.RelatorioControleLivrosEmprestados(request, useCaseMock.Object);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        #endregion

        #region RelatorioControleAcervo

        [Fact]
        public async Task DadoFiltrosValidos_QuandoSolicitarRelatorioControleAcervo_EntaoDeveRetornarArquivo()
        {
            // Arrange
            var request = new Faker<RelatorioControleAcervoRequest>().Generate();
            var streamRetorno = new MemoryStream();
            var useCaseMock = _mocker.GetMock<IRelatorioControleAcervoUseCase>();

            useCaseMock.Setup(x => x.Executar(request))
                       .ReturnsAsync(streamRetorno);

            // Act
            var resultado = await _controller.RelatorioControleAcervo(request, useCaseMock.Object);

            // Assert
            var fileResult = Assert.IsType<FileStreamResult>(resultado);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
            Assert.Equal("relatorio.xlsx", fileResult.FileDownloadName);
        }

        [Fact]
        public async Task DadoRetornoNulo_QuandoSolicitarRelatorioControleAcervo_EntaoDeveRetornarNoContent()
        {
            // Arrange
            var request = new Faker<RelatorioControleAcervoRequest>().Generate();
            var useCaseMock = _mocker.GetMock<IRelatorioControleAcervoUseCase>();

            // Act
            var resultado = await _controller.RelatorioControleAcervo(request, useCaseMock.Object);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        #endregion

        #region RelatorioControleAcervoAutor

        [Fact]
        public async Task DadoFiltrosValidos_QuandoSolicitarRelatorioControleAcervoAutor_EntaoDeveRetornarArquivo()
        {
            // Arrange
            var request = new Faker<RelatorioControleAcervoAutorRequest>().Generate();
            var streamRetorno = new MemoryStream();
            var useCaseMock = _mocker.GetMock<IRelatorioControleAcervoAutorUseCase>();

            useCaseMock.Setup(x => x.Executar(request))
                       .ReturnsAsync(streamRetorno);

            // Act
            var resultado = await _controller.RelatorioControleAcervoAutor(request, useCaseMock.Object);

            // Assert
            var fileResult = Assert.IsType<FileStreamResult>(resultado);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
        }

        [Fact]
        public async Task DadoRetornoNulo_QuandoSolicitarRelatorioControleAcervoAutor_EntaoDeveRetornarNoContent()
        {
            // Arrange
            var request = new Faker<RelatorioControleAcervoAutorRequest>().Generate();
            var useCaseMock = _mocker.GetMock<IRelatorioControleAcervoAutorUseCase>();

            // Act
            var resultado = await _controller.RelatorioControleAcervoAutor(request, useCaseMock.Object);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        #endregion

        #region RelatorioControleDevolucaoLivros

        [Fact]
        public async Task DadoFiltrosValidos_QuandoSolicitarRelatorioControleDevolucaoLivros_EntaoDeveRetornarArquivo()
        {
            // Arrange
            var request = new Faker<RelatorioControleDevolucaoLivrosRequest>().Generate();
            var streamRetorno = new MemoryStream();
            var useCaseMock = _mocker.GetMock<IRelatorioControleDevolucaoLivrosUseCase>();

            useCaseMock.Setup(x => x.Executar(request))
                       .ReturnsAsync(streamRetorno);

            // Act
            var resultado = await _controller.RelatorioControleAcervoAutor(request, useCaseMock.Object);

            // Assert
            var fileResult = Assert.IsType<FileStreamResult>(resultado);
            Assert.Equal("relatorio.xlsx", fileResult.FileDownloadName);
        }

        [Fact]
        public async Task DadoRetornoNulo_QuandoSolicitarRelatorioControleDevolucaoLivros_EntaoDeveRetornarNoContent()
        {
            // Arrange
            var request = new Faker<RelatorioControleDevolucaoLivrosRequest>().Generate();
            var useCaseMock = _mocker.GetMock<IRelatorioControleDevolucaoLivrosUseCase>();

            // Act
            var resultado = await _controller.RelatorioControleAcervoAutor(request, useCaseMock.Object);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        #endregion

        #region RelatorioControleEditora

        [Fact]
        public async Task DadoFiltrosValidos_QuandoSolicitarRelatorioControleEditora_EntaoDeveRetornarArquivo()
        {
            // Arrange
            var request = new Faker<RelatorioControleEditoraRequest>().Generate();
            var streamRetorno = new MemoryStream();
            var useCaseMock = _mocker.GetMock<IRelatorioControleEditoraUseCase>();

            useCaseMock.Setup(x => x.Executar(request))
                       .ReturnsAsync(streamRetorno);

            // Act
            var resultado = await _controller.RelatorioControleEditora(request, useCaseMock.Object);

            // Assert
            var fileResult = Assert.IsType<FileStreamResult>(resultado);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
        }

        [Fact]
        public async Task DadoRetornoNulo_QuandoSolicitarRelatorioControleEditora_EntaoDeveRetornarNoContent()
        {
            // Arrange
            var request = new Faker<RelatorioControleEditoraRequest>().Generate();
            var useCaseMock = _mocker.GetMock<IRelatorioControleEditoraUseCase>();

            // Act
            var resultado = await _controller.RelatorioControleEditora(request, useCaseMock.Object);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        #endregion

        #region RelatorioTitulosMaisPesquisados

        [Fact]
        public async Task DadoFiltrosValidos_QuandoSolicitarRelatorioTitulosMaisPesquisados_EntaoDeveRetornarArquivo()
        {
            // Arrange
            var request = new Faker<RelatorioTitulosMaisPesquisadosRequest>().Generate();
            var streamRetorno = new MemoryStream();
            var useCaseMock = _mocker.GetMock<IRelatorioTitulosMaisPesquisadosUseCase>();

            useCaseMock.Setup(x => x.ExecutarAsync(request))
                       .ReturnsAsync(streamRetorno);

            // Act
            var resultado = await _controller.RelatorioTitulosMaisPesquisados(request, useCaseMock.Object);

            // Assert
            var fileResult = Assert.IsType<FileStreamResult>(resultado);
            Assert.Equal("relatorio.xlsx", fileResult.FileDownloadName);
        }

        [Fact]
        public async Task DadoRetornoNulo_QuandoSolicitarRelatorioTitulosMaisPesquisados_EntaoDeveRetornarNoContent()
        {
            // Arrange
            var request = new Faker<RelatorioTitulosMaisPesquisadosRequest>().Generate();
            var useCaseMock = _mocker.GetMock<IRelatorioTitulosMaisPesquisadosUseCase>();

            useCaseMock.Setup(x => x.ExecutarAsync(request))
                       .ReturnsAsync((Stream?)null);

            // Act
            var resultado = await _controller.RelatorioTitulosMaisPesquisados(request, useCaseMock.Object);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        #endregion

        #region RelatorioControleDownloadAcervo

        [Fact]
        public async Task DadoFiltrosValidos_QuandoSolicitarRelatorioDownloadAcervo_EntaoDeveRetornarArquivo()
        {
            // Arrange
            var request = new Faker<RelatorioControleDownloadAcervoRequest>().Generate();
            var streamRetorno = new MemoryStream();
            var useCaseMock = _mocker.GetMock<IRelatorioControleDownloadAcervoUseCase>();

            useCaseMock.Setup(x => x.ExecutarAsync(request))
                       .ReturnsAsync(streamRetorno);

            // Act
            var resultado = await _controller.RelatorioControleDownloadAcervo(request, useCaseMock.Object);

            // Assert
            var fileResult = Assert.IsType<FileStreamResult>(resultado);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
        }

        [Fact]
        public async Task DadoRetornoNulo_QuandoSolicitarRelatorioDownloadAcervo_EntaoDeveRetornarNoContent()
        {
            // Arrange
            var request = new Faker<RelatorioControleDownloadAcervoRequest>().Generate();
            var useCaseMock = _mocker.GetMock<IRelatorioControleDownloadAcervoUseCase>();

            useCaseMock.Setup(x => x.ExecutarAsync(request))
                       .ReturnsAsync((Stream?)null);

            // Act
            var resultado = await _controller.RelatorioControleDownloadAcervo(request, useCaseMock.Object);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        #endregion

        #region RelatorioHistoricoSolicitacoesAcervo

        [Fact]
        public async Task DadoFiltrosValidos_QuandoSolicitarRelatorioHistoricoSolicitacoes_EntaoDeveRetornarArquivo()
        {
            // Arrange
            var request = new Faker<RelatorioHistoricoSolicitacoesRequest>()
                .CustomInstantiator(f => new RelatorioHistoricoSolicitacoesRequest(
                    f.Person.FullName,
                    f.Date.Past(),
                    f.Date.Future(),
                    null,
                    null
                ))
                .Generate();

            var streamRetorno = new MemoryStream();
            var useCaseMock = _mocker.GetMock<IRelatorioHistoricoSolicitacoesUseCase>();

            useCaseMock.Setup(x => x.ExecutarAsync(request))
                       .ReturnsAsync(streamRetorno);

            // Act
            var resultado = await _controller.RelatorioHistoricoSolicitacoesAcervo(request, useCaseMock.Object);

            // Assert
            var fileResult = Assert.IsType<FileStreamResult>(resultado);
            Assert.Equal("relatorio.xlsx", fileResult.FileDownloadName);
        }

        [Fact]
        public async Task DadoRetornoNulo_QuandoSolicitarRelatorioHistoricoSolicitacoes_EntaoDeveRetornarNoContent()
        {
            // Arrange
            var request = new Faker<RelatorioHistoricoSolicitacoesRequest>()
                .CustomInstantiator(f => new RelatorioHistoricoSolicitacoesRequest(
                    f.Person.FullName,
                    f.Date.Past(),
                    f.Date.Future(),
                    null,
                    null
                ))
                .Generate();

            var useCaseMock = _mocker.GetMock<IRelatorioHistoricoSolicitacoesUseCase>();

            useCaseMock.Setup(x => x.ExecutarAsync(request))
                       .ReturnsAsync((Stream?)null);

            // Act
            var resultado = await _controller.RelatorioHistoricoSolicitacoesAcervo(request, useCaseMock.Object);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        #endregion
    }
}