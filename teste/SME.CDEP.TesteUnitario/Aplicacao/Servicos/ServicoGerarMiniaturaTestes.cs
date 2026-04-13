using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Moq.Protected;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;
using System.Net;
using System.Text;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoGerarMiniaturaTestes
    {
        private readonly Mock<IServicoArmazenamento> servicoArmazenamentoMock;
        private readonly Mock<IRepositorioArquivo> repositorioArquivoMock;
        private readonly Mock<IHttpClientFactory> httpClientFactoryMock;
        private readonly ServicoGerarMiniatura sut;

        public ServicoGerarMiniaturaTestes()
        {
            var mocker = new AutoMocker();

            servicoArmazenamentoMock = mocker.GetMock<IServicoArmazenamento>();
            repositorioArquivoMock = mocker.GetMock<IRepositorioArquivo>();
            httpClientFactoryMock = mocker.GetMock<IHttpClientFactory>();

            sut = mocker.CreateInstance<ServicoGerarMiniatura>();
        }

        [Fact]
        public void DadoDependenciasValidas_QuandoInstanciarServico_EntaoRetornaInstanciaComSucesso()
        {
            // Arrange
            Action acao = () =>
            {
                _ = new ServicoGerarMiniatura(
                    servicoArmazenamentoMock.Object,
                    repositorioArquivoMock.Object,
                    httpClientFactoryMock.Object);
            };

            // Act & Assert
            acao.Should().NotThrow();
            sut.Should().NotBeNull();
        }

        [Fact]
        public async Task DadoParametrosValidos_QuandoGerarMiniatura_EntaoGeraESalvaMiniaturaComSucesso()
        {
            // Arrange
            var idEsperado = 99L;
            var nomeArquivoFisico = "imagem_teste.jpg";
            var nomeMiniatura = "miniatura_teste.jpg";
            var tipoConteudo = "image/jpeg";
            var tipoArquivo = TipoArquivo.AcervoFotografico;
            var urlImagem = "http://fake-storage.com/imagem_teste.jpg";

            var bytesImagemValida = GerarBytesImagemDummyEmMemoria();
            MockarRespostaHttpClient(HttpStatusCode.OK, bytesImagemValida);

            servicoArmazenamentoMock
                .Setup(s => s.Obter(nomeArquivoFisico, false))
                .ReturnsAsync(urlImagem);

            servicoArmazenamentoMock
                .Setup(s => s.Armazenar(It.IsAny<string>(), It.IsAny<Stream>(), tipoConteudo))
                .ReturnsAsync("url_gerada_armazenamento");

            repositorioArquivoMock
                .Setup(r => r.SalvarAsync(It.IsAny<Arquivo>()))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await sut.GerarMiniatura(tipoConteudo, nomeArquivoFisico, nomeMiniatura, tipoArquivo);

            // Assert
            resultado.Should().Be(idEsperado);

            servicoArmazenamentoMock.Verify(s => s.Obter(nomeArquivoFisico, false), Times.Once);

            servicoArmazenamentoMock.Verify(s => s.Armazenar(
                It.Is<string>(nome => nome.EndsWith(".jpg")),
                It.IsAny<Stream>(),
                tipoConteudo), Times.Once);

            repositorioArquivoMock.Verify(r => r.SalvarAsync(It.Is<Arquivo>(a =>
                a.Nome == nomeMiniatura &&
                a.TipoConteudo == tipoConteudo &&
                a.Tipo == tipoArquivo &&
                a.Codigo != Guid.Empty)), Times.Once);
        }

        [Fact]
        public async Task DadoUrlInacessivelNoArmazenamento_QuandoGerarMiniatura_EntaoLancaHttpRequestException()
        {
            // Arrange
            var nomeArquivoFisico = "imagem_inexistente.jpg";
            var urlInvalida = "http://fake-storage.com/imagem_inexistente.jpg";

            MockarRespostaHttpClient(HttpStatusCode.NotFound, Array.Empty<byte>());

            servicoArmazenamentoMock
                .Setup(s => s.Obter(nomeArquivoFisico, false))
                .ReturnsAsync(urlInvalida);

            // Act
            Func<Task> acao = async () => await sut.GerarMiniatura("image/jpeg", nomeArquivoFisico, "miniatura", TipoArquivo.AcervoFotografico);

            // Assert
            await acao.Should().ThrowAsync<HttpRequestException>();
        }

        [Fact]
        public async Task DadoConteudoRetornadoNaoSendoImagemValida_QuandoGerarMiniatura_EntaoLancaUnknownImageFormatException()
        {
            // Arrange
            var nomeArquivoFisico = "arquivo_corrompido.jpg";
            var urlArquivo = "http://fake-storage.com/arquivo_corrompido.jpg";

            var bytesNaoImagem = Encoding.UTF8.GetBytes("Isso não é uma imagem válida, é um texto.");

            MockarRespostaHttpClient(HttpStatusCode.OK, bytesNaoImagem);

            servicoArmazenamentoMock
                .Setup(s => s.Obter(nomeArquivoFisico, false))
                .ReturnsAsync(urlArquivo);

            // Act
            Func<Task> acao = async () => await sut.GerarMiniatura("image/jpeg", nomeArquivoFisico, "min", TipoArquivo.AcervoFotografico);

            // Assert
            await acao.Should().ThrowAsync<UnknownImageFormatException>();
        }

        // ================= MÉTODOS PRIVADOS AUXILIARES ================= //

        private void MockarRespostaHttpClient(HttpStatusCode statusCode, byte[] conteudoRetorno)
        {
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new ByteArrayContent(conteudoRetorno)
                });

            var httpClient = new HttpClient(mockHandler.Object);

            httpClientFactoryMock
                .Setup(f => f.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);
        }

        private static byte[] GerarBytesImagemDummyEmMemoria()
        {
            using var img = new Image<Rgba32>(10, 10);
            using var ms = new MemoryStream();

            img.SaveAsJpeg(ms);

            return ms.ToArray();
        }
    }
}