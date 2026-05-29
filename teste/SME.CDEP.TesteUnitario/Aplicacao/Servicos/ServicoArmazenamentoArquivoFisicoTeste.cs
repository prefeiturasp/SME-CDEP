using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoArmazenamentoArquivoFisicoTeste
    {
        private readonly Mock<IServicoArmazenamento> _mockServicoArmazenamento;
        private readonly ServicoArmazenamentoArquivoFisico _sut;

        public ServicoArmazenamentoArquivoFisicoTeste()
        {
            _mockServicoArmazenamento = new Mock<IServicoArmazenamento>();
            _sut = new ServicoArmazenamentoArquivoFisico(_mockServicoArmazenamento.Object);
        }

        #region Testes de Construtor

        [Fact]
        public void DadoServicoArmazenamentoNulo_QuandoConstruir_EntaoLancaArgumentNullException()
        {
            // Arrange & Act
            Action acao = () => _ = new ServicoArmazenamentoArquivoFisico(null!);

            // Assert
            acao.Should().Throw<ArgumentNullException>()
                .WithMessage("*servicoArmazenamento*");
        }

        [Fact]
        public void DadoServicoArmazenamentoValido_QuandoConstruir_EntaoInstanciaComSucesso()
        {
            // Arrange & Act
            var servico = new ServicoArmazenamentoArquivoFisico(_mockServicoArmazenamento.Object);

            // Assert
            servico.Should().NotBeNull();
            servico.Should().BeOfType<ServicoArmazenamentoArquivoFisico>();
        }

        #endregion

        #region Testes de Armazenar - Arquivo Comum

        [Fact]
        public async Task DadoArquivoComumValido_QuandoArmazenar_EntaoDeveRetornarArquivoArmazenadoDTO()
        {
            // Arrange
            var formFile = CriarMockFormFileValido(
                nomeArquivo: "documento.pdf",
                contentType: "application/pdf"
            );

            var caminhoEsperado = "caminho/arquivo.pdf";

            _mockServicoArmazenamento
                .Setup(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(caminhoEsperado);

            // Act
            var resultado = await _sut.Armazenar(formFile.Object, TipoArquivo.AcervoFotografico);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<ArquivoArmazenadoDTO>();
        }

        [Fact]
        public async Task DadoArquivoComumValido_QuandoArmazenar_EntaoDeveChamarServicoArmazenamento()
        {
            // Arrange
            var formFile = CriarMockFormFileValido(
                nomeArquivo: "relatorio.xlsx",
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            );

            var caminhoEsperado = "caminho/arquivo.xlsx";

            _mockServicoArmazenamento
                .Setup(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(caminhoEsperado);

            // Act
            await _sut.Armazenar(formFile.Object, TipoArquivo.AcervoArteGrafica);

            // Assert
            _mockServicoArmazenamento
                .Verify(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), formFile.Object.ContentType), Times.Once);
        }

        [Fact]
        public async Task DadoArquivoComumValido_QuandoArmazenar_EntaoDeveGerar​CodigoGuideUnico()
        {
            // Arrange
            var formFile = CriarMockFormFileValido(
                nomeArquivo: "imagem.jpg",
                contentType: "image/jpeg"
            );

            var caminho = "caminho/arquivo.jpg";

            _mockServicoArmazenamento
                .Setup(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(caminho);

            // Act
            var resultado1 = await _sut.Armazenar(formFile.Object, TipoArquivo.AcervoFotografico);
            var resultado2 = await _sut.Armazenar(formFile.Object, TipoArquivo.AcervoFotografico);

            // Assert
            resultado1.Codigo.Should().NotBe(Guid.Empty);
            resultado2.Codigo.Should().NotBe(Guid.Empty);
            resultado1.Codigo.Should().NotBe(resultado2.Codigo);
        }

        [Fact]
        public async Task DadoArquivoComumComTipoTemp_QuandoArmazenar_EntaoDeveChamarArmazenarTemporaria()
        {
            // Arrange
            var formFile = CriarMockFormFileValido(
                nomeArquivo: "imagem.jpg",
                contentType: "image/jpeg"
            );

            var caminho = "caminho/temp/arquivo.jpg";

            _mockServicoArmazenamento
                .Setup(s => s.ArmazenarTemporaria(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(caminho);

            // Act
            await _sut.Armazenar(formFile.Object, TipoArquivo.Temp);

            // Assert
            _mockServicoArmazenamento
                .Verify(s => s.ArmazenarTemporaria(It.IsAny<string>(), It.IsAny<Stream>(), formFile.Object.ContentType), Times.Once);

            _mockServicoArmazenamento
                .Verify(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DadoArquivoComumComTipoEditor_QuandoArmazenar_EntaoDeveChamarArmazenarTemporaria()
        {
            // Arrange
            var formFile = CriarMockFormFileValido(
                nomeArquivo: "texto.txt",
                contentType: "text/plain"
            );

            var caminho = "caminho/temp/arquivo.txt";

            _mockServicoArmazenamento
                .Setup(s => s.ArmazenarTemporaria(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(caminho);

            // Act
            await _sut.Armazenar(formFile.Object, TipoArquivo.Editor);

            // Assert
            _mockServicoArmazenamento
                .Verify(s => s.ArmazenarTemporaria(It.IsAny<string>(), It.IsAny<Stream>(), formFile.Object.ContentType), Times.Once);
        }

        [Fact]
        public async Task DadoArquivoComum_QuandoArmazenar_EntaoDeveRetornarCaminhoCorreto()
        {
            // Arrange
            var formFile = CriarMockFormFileValido(
                nomeArquivo: "documento.pdf",
                contentType: "application/pdf"
            );

            var caminhoEsperado = "https://bucket.storage.com/arquivo.pdf";

            _mockServicoArmazenamento
                .Setup(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(caminhoEsperado);

            // Act
            var resultado = await _sut.Armazenar(formFile.Object, TipoArquivo.AcervoFotografico);

            // Assert
            resultado.Path.Should().Be(caminhoEsperado);
        }

        [Fact]
        public async Task DadoArquivoComum_QuandoArmazenar_EntaoDeveRetornarNomeArquivoOriginal()
        {
            // Arrange
            var nomeOriginal = "relatório_mensal.xlsx";
            var formFile = CriarMockFormFileValido(
                nomeArquivo: nomeOriginal,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            );

            var caminho = "caminho/arquivo.xlsx";

            _mockServicoArmazenamento
                .Setup(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(caminho);

            // Act
            var resultado = await _sut.Armazenar(formFile.Object, TipoArquivo.AcervoDocumental);

            // Assert
            resultado.Nome.Should().Be(nomeOriginal);
        }

        [Fact]
        public async Task DadoArquivoComum_QuandoArmazenar_EntaoDeveRetornarTipoConteudoCorreto()
        {
            // Arrange
            var contentType = "text/plain";
            var formFile = CriarMockFormFileValido(
                nomeArquivo: "arquivo.txt",
                contentType: contentType
            );

            var caminho = "caminho/arquivo.txt";

            _mockServicoArmazenamento
                .Setup(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(caminho);

            // Act
            var resultado = await _sut.Armazenar(formFile.Object, TipoArquivo.AcervoTridimensional);

            // Assert
            resultado.ContentType.Should().Be(contentType);
        }

        [Fact]
        public async Task DadoArquivoComum_QuandoArmazenar_EntaoDeveRetornarTipoArquivoCorreto()
        {
            // Arrange
            var tipoEsperado = TipoArquivo.AcervoDocumental;
            var formFile = CriarMockFormFileValido(
                nomeArquivo: "documento.pdf",
                contentType: "application/pdf"
            );

            var caminho = "caminho/arquivo.pdf";

            _mockServicoArmazenamento
                .Setup(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(caminho);

            // Act
            var resultado = await _sut.Armazenar(formFile.Object, tipoEsperado);

            // Assert
            resultado.TipoArquivo.Should().Be(tipoEsperado);
        }

        #endregion

        #region Testes de Armazenar - Arquivo TIFF

        [Fact]
        public async Task DadoArquivoTiff_QuandoArmazenar_EntaoDeveConverterParaJpeg()
        {
            // Arrange
            var imagemTiff = ObtenhaTiffBytes();
            var formFile = CriarMockFormFileComBytes(
                nomeArquivo: "fotografia.tiff",
                contentType: "image/tiff",
                bytes: imagemTiff
            );

            var caminho = "caminho/arquivo.jpeg";

            _mockServicoArmazenamento
                .Setup(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), Constantes.CONTENT_TYPE_JPEG))
                .ReturnsAsync(caminho);

            // Act
            var resultado = await _sut.Armazenar(formFile.Object, TipoArquivo.AcervoFotografico);

            // Assert
            resultado.Nome.Should().EndWith(".jpeg");
            resultado.ContentType.Should().Be(Constantes.CONTENT_TYPE_JPEG);
        }

        [Fact]
        public async Task DadoArquivoTiff_QuandoArmazenar_EntaoDeveChamarArmazenarComContentTypeJpeg()
        {
            // Arrange
            var imagemTiff = ObtenhaTiffBytes();
            var formFile = CriarMockFormFileComBytes(
                nomeArquivo: "foto.tiff",
                contentType: "image/tiff",
                bytes: imagemTiff
            );

            var caminho = "caminho/arquivo.jpeg";

            _mockServicoArmazenamento
                .Setup(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), Constantes.CONTENT_TYPE_JPEG))
                .ReturnsAsync(caminho);

            // Act
            await _sut.Armazenar(formFile.Object, TipoArquivo.AcervoFotografico);

            // Assert
            _mockServicoArmazenamento
                .Verify(s => s.Armazenar(
                    It.IsAny<string>(),
                    It.IsAny<Stream>(),
                    Constantes.CONTENT_TYPE_JPEG
                ), Times.Once);
        }

        [Fact]
        public async Task DadoArquivoTiff_QuandoArmazenar_EntaoDeveRetornarArquivoArmazenadoComTipoJpeg()
        {
            // Arrange
            var imagemTiff = ObtenhaTiffBytes();
            var formFile = CriarMockFormFileComBytes(
                nomeArquivo: "imagem.tiff",
                contentType: "image/tiff",
                bytes: imagemTiff
            );

            var caminho = "caminho/arquivo.jpeg";

            _mockServicoArmazenamento
                .Setup(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), Constantes.CONTENT_TYPE_JPEG))
                .ReturnsAsync(caminho);

            // Act
            var resultado = await _sut.Armazenar(formFile.Object, TipoArquivo.AcervoFotografico);

            // Assert
            resultado.Should().NotBeNull();
            resultado.ContentType.Should().Be(Constantes.CONTENT_TYPE_JPEG);
            resultado.Nome.EndsWith(".jpeg").Should().BeTrue();
        }

        [Fact]
        public async Task DadoArquivoTiffComTipoTemp_QuandoArmazenar_EntaoDeveConverterEArmazenarTemporaria()
        {
            // Arrange
            var imagemTiff = ObtenhaTiffBytes();
            var formFile = CriarMockFormFileComBytes(
                nomeArquivo: "rascunho.tif",
                contentType: "image/tiff",
                bytes: imagemTiff
            );

            var caminho = "caminho/temp/arquivo.jpeg";

            _mockServicoArmazenamento
                .Setup(s => s.ArmazenarTemporaria(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(caminho);

            // Act
            var resultado = await _sut.Armazenar(formFile.Object, TipoArquivo.Temp);

            // Assert
            resultado.ContentType.Should().Be(Constantes.CONTENT_TYPE_JPEG);
            _mockServicoArmazenamento
                .Verify(s => s.ArmazenarTemporaria(It.IsAny<string>(), It.IsAny<Stream>(), Constantes.CONTENT_TYPE_JPEG), Times.Once);
            
            _mockServicoArmazenamento
                .Verify(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DadoArquivoTiffComVariacaoDeMaiusculaMenuscula_QuandoArmazenar_EntaoDeveDetectarComoTiff()
        {
            // Arrange
            var imagemTiff = ObtenhaTiffBytes();
            var formFile = CriarMockFormFileComBytes(
                nomeArquivo: "fotografia.TIFF",
                contentType: "image/TIFF",
                bytes: imagemTiff
            );

            var caminho = "caminho/arquivo.jpeg";

            _mockServicoArmazenamento
                .Setup(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), Constantes.CONTENT_TYPE_JPEG))
                .ReturnsAsync(caminho);

            // Act
            var resultado = await _sut.Armazenar(formFile.Object, TipoArquivo.AcervoFotografico);

            // Assert
            resultado.Nome.Should().EndWith(".jpeg");
            resultado.ContentType.Should().Be(Constantes.CONTENT_TYPE_JPEG);
        }

        [Fact]
        public async Task DadoArquivoTiff_QuandoArmazenar_EntaoDeveUsarCodigoGuideNoNomeJpeg()
        {
            // Arrange
            var imagemTiff = ObtenhaTiffBytes();
            var formFile = CriarMockFormFileComBytes(
                nomeArquivo: "imagem.tiff",
                contentType: "image/tiff",
                bytes: imagemTiff
            );

            var caminho = "caminho/arquivo.jpeg";
            string nomeArquivoCapturado = string.Empty;

            _mockServicoArmazenamento
                .Setup(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), Constantes.CONTENT_TYPE_JPEG))
                .Callback<string, Stream, string>((nome, _, _) => nomeArquivoCapturado = nome)
                .ReturnsAsync(caminho);

            // Act
            var resultado = await _sut.Armazenar(formFile.Object, TipoArquivo.AcervoFotografico);

            // Assert
            resultado.Codigo.Should().NotBe(Guid.Empty);
            nomeArquivoCapturado.Should().StartWith(resultado.Codigo.ToString());
            nomeArquivoCapturado.Should().EndWith(".jpeg");
        }

        [Fact]
        public async Task DadoArquivoTiff_QuandoArmazenar_EntaoDeveRetornarCodigoIdentico()
        {
            // Arrange
            var imagemTiff = ObtenhaTiffBytes();
            var formFile = CriarMockFormFileComBytes(
                nomeArquivo: "foto.tiff",
                contentType: "image/tiff",
                bytes: imagemTiff
            );

            var caminho = "caminho/arquivo.jpeg";
            Guid codigoCapturado = Guid.Empty;

            _mockServicoArmazenamento
                .Setup(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), Constantes.CONTENT_TYPE_JPEG))
                .Callback<string, Stream, string>((nome, _, _) => 
                {
                    var codigoStr = nome[..36]; // Tamanho de um GUID em string
                    codigoCapturado = Guid.Parse(codigoStr);
                })
                .ReturnsAsync(caminho);

            // Act
            var resultado = await _sut.Armazenar(formFile.Object, TipoArquivo.AcervoFotografico);

            // Assert
            resultado.Codigo.Should().Be(codigoCapturado);
        }

        #endregion

        #region Testes de Armazenar - Diferentes Tipos de Arquivo

        [Theory]
        [InlineData(TipoArquivo.AcervoFotografico)]
        [InlineData(TipoArquivo.AcervoArteGrafica)]
        [InlineData(TipoArquivo.AcervoTridimensional)]
        [InlineData(TipoArquivo.AcervoDocumental)]
        [InlineData(TipoArquivo.Sistema)]
        public async Task DadoArquivoComumComDiferentesTipos_QuandoArmazenar_EntaoDeveChamarArmazenarPermanente(
            TipoArquivo tipoArquivo)
        {
            // Arrange
            var formFile = CriarMockFormFileValido(
                nomeArquivo: "documento.pdf",
                contentType: "application/pdf"
            );

            var caminho = "caminho/arquivo.pdf";

            _mockServicoArmazenamento
                .Setup(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(caminho);

            // Act
            await _sut.Armazenar(formFile.Object, tipoArquivo);

            // Assert
            _mockServicoArmazenamento
                .Verify(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DadoArquivoComExtensaoComPonto_QuandoArmazenar_EntaoDevePreservarExtensao()
        {
            // Arrange
            var formFile = CriarMockFormFileValido(
                nomeArquivo: "video.mp4",
                contentType: "video/mp4"
            );

            var caminho = "caminho/arquivo.mp4";
            string nomeArquivoCapturado = string.Empty;

            _mockServicoArmazenamento
                .Setup(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()))
                .Callback<string, Stream, string>((nome, _, _) => nomeArquivoCapturado = nome)
                .ReturnsAsync(caminho);

            // Act
            await _sut.Armazenar(formFile.Object, TipoArquivo.AcervoFotografico);

            // Assert
            nomeArquivoCapturado.Should().EndWith(".mp4");
        }

        [Fact]
        public async Task DadoArquivoComExtensaoMaioscula_QuandoArmazenar_EntaoDevePreservarMaiusculaDaExtensao()
        {
            // Arrange
            var formFile = CriarMockFormFileValido(
                nomeArquivo: "documento.PDF",
                contentType: "application/pdf"
            );

            var caminho = "caminho/arquivo.PDF";
            string nomeArquivoCapturado = string.Empty;

            _mockServicoArmazenamento
                .Setup(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()))
                .Callback<string, Stream, string>((nome, _, _) => nomeArquivoCapturado = nome)
                .ReturnsAsync(caminho);

            // Act
            await _sut.Armazenar(formFile.Object, TipoArquivo.AcervoDocumental);

            // Assert
            nomeArquivoCapturado.Should().EndWith(".PDF");
        }

        #endregion

        #region Testes de Integração

        [Fact]
        public async Task DadoFluxoCompletoDeArmazenamentoTemporario_QuandoArmazenarArquivoComum_EntaoRetornaResultadoEsperado()
        {
            // Arrange
            var formFile = CriarMockFormFileValido(
                nomeArquivo: "rascunho.docx",
                contentType: "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            );

            var caminhoTemporario = "https://storage.temp/arquivo.docx";

            _mockServicoArmazenamento
                .Setup(s => s.ArmazenarTemporaria(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(caminhoTemporario);

            // Act
            var resultado = await _sut.Armazenar(formFile.Object, TipoArquivo.Editor);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Path.Should().Be(caminhoTemporario);
            resultado.Codigo.Should().NotBe(Guid.Empty);
            resultado.ContentType.Should().Be(formFile.Object.ContentType);
            resultado.TipoArquivo.Should().Be(TipoArquivo.Editor);
        }

        [Fact]
        public async Task DadoFluxoCompletoDeArmazenamentoPermanente_QuandoArmazenarArquivoComum_EntaoRetornaResultadoEsperado()
        {
            // Arrange
            var formFile = CriarMockFormFileValido(
                nomeArquivo: "documento_final.pdf",
                contentType: "application/pdf"
            );

            var caminhoPermanente = "https://storage.files/arquivo.pdf";

            _mockServicoArmazenamento
                .Setup(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(caminhoPermanente);

            // Act
            var resultado = await _sut.Armazenar(formFile.Object, TipoArquivo.AcervoDocumental);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Path.Should().Be(caminhoPermanente);
            resultado.Codigo.Should().NotBe(Guid.Empty);
            resultado.Nome.Should().Be(formFile.Object.FileName);
            resultado.ContentType.Should().Be(formFile.Object.ContentType);
            resultado.TipoArquivo.Should().Be(TipoArquivo.AcervoDocumental);
        }

        [Fact]
        public async Task DadoFluxoCompletoDeArmazenamentoTiff_QuandoArmazenarArquivoTiff_EntaoRetornaResultadoEsperado()
        {
            // Arrange
            var imagemTiff = ObtenhaTiffBytes();
            var formFile = CriarMockFormFileComBytes(
                nomeArquivo: "fotografia_original.tiff",
                contentType: "image/tiff",
                bytes: imagemTiff
            );

            var caminhoPermanente = "https://storage.files/arquivo.jpeg";

            _mockServicoArmazenamento
                .Setup(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), Constantes.CONTENT_TYPE_JPEG))
                .ReturnsAsync(caminhoPermanente);

            // Act
            var resultado = await _sut.Armazenar(formFile.Object, TipoArquivo.AcervoFotografico);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Path.Should().Be(caminhoPermanente);
            resultado.Codigo.Should().NotBe(Guid.Empty);
            resultado.Nome.Should().EndWith(".jpeg");
            resultado.ContentType.Should().Be(Constantes.CONTENT_TYPE_JPEG);
            resultado.TipoArquivo.Should().Be(TipoArquivo.AcervoFotografico);
        }

        #endregion

        #region Métodos Auxiliares

        private static Mock<IFormFile> CriarMockFormFileValido(string nomeArquivo, string contentType)
        {
            var mockFormFile = new Mock<IFormFile>();
            var bytes = Encoding.UTF8.GetBytes("conteúdo do arquivo");
            var stream = new MemoryStream(bytes);

            mockFormFile.Setup(f => f.FileName).Returns(nomeArquivo);
            mockFormFile.Setup(f => f.ContentType).Returns(contentType);
            mockFormFile.Setup(f => f.Length).Returns(stream.Length);
            mockFormFile.Setup(f => f.OpenReadStream()).Returns(stream);

            return mockFormFile;
        }

        private static Mock<IFormFile> CriarMockFormFileComBytes(string nomeArquivo, string contentType, byte[] bytes)
        {
            var mockFormFile = new Mock<IFormFile>();
            var stream = new MemoryStream(bytes);

            mockFormFile.Setup(f => f.FileName).Returns(nomeArquivo);
            mockFormFile.Setup(f => f.ContentType).Returns(contentType);
            mockFormFile.Setup(f => f.Length).Returns(stream.Length);
            mockFormFile.Setup(f => f.OpenReadStream()).Returns(stream);

            return mockFormFile;
        }

        private static byte[] ObtenhaTiffBytes()
        {
            // Gera um arquivo TIFF válido criando um Bitmap e salvando-o em TIFF
            // Esta é a forma mais confiável de garantir compatibilidade com System.Drawing
            try
            {
                using (var bitmap = new Bitmap(10, 10))
                {
                    using (var graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.Clear(Color.White);
                        graphics.DrawRectangle(Pens.Black, 1, 1, 8, 8);
                    }

                    using (var ms = new MemoryStream())
                    {
                        bitmap.Save(ms, ImageFormat.Tiff);
                        return ms.ToArray();
                    }
                }
            }
            catch
            {
                // Fallback: Um arquivo TIFF minimal válido (estrutura corrigida)
                // Imagem 2x2 pixels em preto e branco
                return new byte[]
                {
                    // TIFF Header (little-endian)
                    0x49, 0x49,                         // "II" - little-endian
                    0x2A, 0x00,                         // Magic number (42)
                    0x08, 0x00, 0x00, 0x00,             // Offset to first IFD
                    
                    // IFD (Image File Directory) - 11 entries
                    0x0B, 0x00,                         // Number of directory entries
                    
                    // ImageWidth tag (256)
                    0x00, 0x01, 0x03, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
                    
                    // ImageLength tag (257)
                    0x01, 0x01, 0x03, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
                    
                    // BitsPerSample tag (258)
                    0x02, 0x01, 0x03, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                    
                    // Compression tag (259) = 1 (None)
                    0x03, 0x01, 0x03, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                    
                    // PhotometricInterpretation tag (262) = 1 (BlackIsZero)
                    0x06, 0x01, 0x03, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                    
                    // StripOffsets tag (273)
                    0x11, 0x01, 0x04, 0x00, 0x01, 0x00, 0x00, 0x00, 0xA8, 0x00, 0x00, 0x00,
                    
                    // SamplesPerPixel tag (277) = 1
                    0x15, 0x01, 0x03, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                    
                    // RowsPerStrip tag (278) = 2
                    0x16, 0x01, 0x03, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
                    
                    // StripByteCounts tag (279) = 4
                    0x17, 0x01, 0x04, 0x00, 0x01, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00,
                    
                    // XResolution tag (282)
                    0x1A, 0x01, 0x05, 0x00, 0x01, 0x00, 0x00, 0x00, 0x9E, 0x00, 0x00, 0x00,
                    
                    // YResolution tag (283)
                    0x1B, 0x01, 0x05, 0x00, 0x01, 0x00, 0x00, 0x00, 0xA6, 0x00, 0x00, 0x00,
                    
                    // Next IFD offset (0 = end)
                    0x00, 0x00, 0x00, 0x00,
                    
                    // Image data: 2x2 pixel strip (4 bytes)
                    0x00, 0x00, 0x00, 0x00,
                    
                    // XResolution rational (72 dpi)
                    0x48, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                    
                    // YResolution rational (72 dpi)
                    0x48, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                };
            }
        }

        #endregion
    }
}
