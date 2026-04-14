using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Controllers;

namespace SME.CDEP.TesteUnitario.Webapi.Controllers
{
    public class ArmazenamentoControllerTestes
    {
        private readonly Mock<IServicoUploadArquivo> servicoUploadArquivoMock;
        private readonly Mock<IServicoDownloadArquivo> servicoDownloadArquivoMock;
        private readonly ArmazenamentoController sut;

        public ArmazenamentoControllerTestes()
        {
            var mocker = new AutoMocker();

            servicoUploadArquivoMock = mocker.GetMock<IServicoUploadArquivo>();
            servicoDownloadArquivoMock = mocker.GetMock<IServicoDownloadArquivo>();

            sut = mocker.CreateInstance<ArmazenamentoController>();
        }

        [Fact]
        public async Task DadoArquivoValido_QuandoUploadTemp_EntaoRetornaOkComDadosDoArquivoArmazenado()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var arquivoArmazenadoEsperado = GerarArquivoArmazenadoDTO();

            servicoUploadArquivoMock
                .Setup(s => s.Upload(fileMock.Object, TipoArquivo.Temp))
                .ReturnsAsync(arquivoArmazenadoEsperado);

            // Act
            var resultado = await sut.UploadTemp(fileMock.Object, servicoUploadArquivoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(arquivoArmazenadoEsperado);
            servicoUploadArquivoMock.Verify(s => s.Upload(fileMock.Object, TipoArquivo.Temp), Times.Once);
        }

        [Fact]
        public async Task DadoCodigoArquivoValidoEEncontrado_QuandoDownload_EntaoRetornaFileResult()
        {
            // Arrange
            var codigoArquivo = Guid.NewGuid();
            var bytesArquivo = new byte[] { 1, 2, 3 };
            var contentType = "application/pdf";
            var nomeArquivo = "documento.pdf";

            servicoDownloadArquivoMock
                .Setup(s => s.Download(codigoArquivo))
                .ReturnsAsync((bytesArquivo, contentType, nomeArquivo));

            // Act
            var resultado = await sut.Download(codigoArquivo, servicoDownloadArquivoMock.Object);

            // Assert
            var fileResult = resultado.Should().BeOfType<FileContentResult>().Subject;
            fileResult.FileContents.Should().BeEquivalentTo(bytesArquivo);
            fileResult.ContentType.Should().Be(contentType);
            fileResult.FileDownloadName.Should().Be(nomeArquivo);
            servicoDownloadArquivoMock.Verify(s => s.Download(codigoArquivo), Times.Once);
        }

        [Fact]
        public async Task DadoCodigoArquivoNaoEncontrado_QuandoDownload_EntaoRetornaNoContent()
        {
            // Arrange
            var codigoArquivo = Guid.NewGuid();

            servicoDownloadArquivoMock
                .Setup(s => s.Download(codigoArquivo))
                .ReturnsAsync(((byte[])null!, null!, null!));

            // Act
            var resultado = await sut.Download(codigoArquivo, servicoDownloadArquivoMock.Object);

            // Assert
            resultado.Should().BeOfType<NoContentResult>();
            servicoDownloadArquivoMock.Verify(s => s.Download(codigoArquivo), Times.Once);
        }

        [Fact]
        public async Task DadoArquivoETipoValidos_QuandoUploadPorTipo_EntaoRetornaOkComDadosDoArquivoArmazenado()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var tipoArquivo = TipoArquivo.AcervoDocumental;
            var arquivoArmazenadoEsperado = GerarArquivoArmazenadoDTO(tipoArquivo);

            servicoUploadArquivoMock
                .Setup(s => s.Upload(fileMock.Object, tipoArquivo))
                .ReturnsAsync(arquivoArmazenadoEsperado);

            // Act
            var resultado = await sut.UploadPorTipo(fileMock.Object, tipoArquivo, servicoUploadArquivoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(arquivoArmazenadoEsperado);
            servicoUploadArquivoMock.Verify(s => s.Upload(fileMock.Object, tipoArquivo), Times.Once);
        }

        [Fact]
        public async Task DadoTipoAcervoComModeloEncontrado_QuandoDownloadPorTipoAcervo_EntaoRetornaFileResult()
        {
            // Arrange
            var tipoAcervo = TipoAcervo.DocumentacaoTextual;
            var bytesArquivo = new byte[] { 4, 5, 6 };
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var nomeArquivo = "planilha_acervo_documental.xlsx";

            servicoDownloadArquivoMock
                .Setup(s => s.DownloadPorTipoAcervo(tipoAcervo))
                .ReturnsAsync((bytesArquivo, contentType, nomeArquivo));

            // Act
            var resultado = await sut.DownloadPorTipoAcervo(tipoAcervo, servicoDownloadArquivoMock.Object);

            // Assert
            var fileResult = resultado.Should().BeOfType<FileContentResult>().Subject;
            fileResult.FileContents.Should().BeEquivalentTo(bytesArquivo);
            fileResult.ContentType.Should().Be(contentType);
            fileResult.FileDownloadName.Should().Be(nomeArquivo);
            servicoDownloadArquivoMock.Verify(s => s.DownloadPorTipoAcervo(tipoAcervo), Times.Once);
        }

        [Fact]
        public async Task DadoTipoAcervoSemModeloEncontrado_QuandoDownloadPorTipoAcervo_EntaoRetornaNoContent()
        {
            // Arrange
            var tipoAcervo = TipoAcervo.Audiovisual;

            servicoDownloadArquivoMock
                .Setup(s => s.DownloadPorTipoAcervo(tipoAcervo))
                .ReturnsAsync(((byte[])null!, null!, null!));

            // Act
            var resultado = await sut.DownloadPorTipoAcervo(tipoAcervo, servicoDownloadArquivoMock.Object);

            // Assert
            resultado.Should().BeOfType<NoContentResult>();
            servicoDownloadArquivoMock.Verify(s => s.DownloadPorTipoAcervo(tipoAcervo), Times.Once);
        }

        // ================= HELPER BOGUS GENERATORS ================= //

        private static ArquivoArmazenadoDTO GerarArquivoArmazenadoDTO(TipoArquivo tipoArquivo = TipoArquivo.Temp)
        {
            var faker = new Faker();
            return new ArquivoArmazenadoDTO(
                path: faker.System.FilePath(),
                codigo: Guid.NewGuid(),
                nome: faker.System.FileName(),
                contentType: faker.System.MimeType(),
                tipoArquivo: tipoArquivo
            )
            {
                Id = faker.Random.Long(1, 1000)
            };
        }
    }
}