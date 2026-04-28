using Bogus;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoDownloadArquivoTestes
    {
        private readonly Mock<IRepositorioArquivo> repositorioArquivoMock;
        private readonly Mock<IServicoArmazenamento> servicoArmazenamentoMock;
        private readonly ServicoDownloadArquivo sut;

        public ServicoDownloadArquivoTestes()
        {
            var mocker = new AutoMocker();

            repositorioArquivoMock = mocker.GetMock<IRepositorioArquivo>();
            servicoArmazenamentoMock = mocker.GetMock<IServicoArmazenamento>();

            sut = mocker.CreateInstance<ServicoDownloadArquivo>();
        }

        [Fact]
        public void DadoDependenciasValidas_QuandoInstanciarServico_EntaoRetornaInstanciaComSucesso()
        {
            // Arrange
            Action acao = () => new ServicoDownloadArquivo(repositorioArquivoMock.Object, servicoArmazenamentoMock.Object);

            // Act & Assert
            acao.Should().NotThrow();
            sut.Should().NotBeNull();
        }

        [Fact]
        public async Task DadoArquivoSemEnderecoNoArmazenamento_QuandoDownload_EntaoLancaNegocioException()
        {
            // Arrange
            var codigoArquivo = Guid.NewGuid();
            var arquivoMock = GerarArquivoMock(codigoArquivo);

            repositorioArquivoMock
                .Setup(r => r.ObterPorCodigo(codigoArquivo))
                .ReturnsAsync(arquivoMock);

            servicoArmazenamentoMock
                .Setup(s => s.Obter(It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(string.Empty);

            // Act
            Func<Task> acao = async () => await sut.Download(codigoArquivo);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.ARQUIVO_NAO_ENCONTRADO);

            repositorioArquivoMock.Verify(r => r.ObterPorCodigo(codigoArquivo), Times.Once);
            servicoArmazenamentoMock.Verify(s => s.Obter(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task DadoArquivoSemEnderecoNoArmazenamento_QuandoDownloadPorTipoAcervo_EntaoLancaNegocioException()
        {
            // Arrange
            var tipoAcervo = TipoAcervo.Fotografico;
            var arquivoMock = GerarArquivoMock(Guid.NewGuid());

            repositorioArquivoMock
                .Setup(r => r.ObterArquivoPorNomeTipoArquivo(It.IsAny<string>(), TipoArquivo.Sistema))
                .ReturnsAsync(arquivoMock);

            servicoArmazenamentoMock
                .Setup(s => s.Obter(It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(string.Empty);

            // Act
            Func<Task> acao = async () => await sut.DownloadPorTipoAcervo(tipoAcervo);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.ARQUIVO_NAO_ENCONTRADO);

            repositorioArquivoMock.Verify(r => r.ObterArquivoPorNomeTipoArquivo(It.IsAny<string>(), TipoArquivo.Sistema), Times.Once);
            servicoArmazenamentoMock.Verify(s => s.Obter(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task DadoUrlInvalidaParaHttpClient_QuandoDownload_EntaoLancaHttpRequestException()
        {
            // Arrange
            var codigoArquivo = Guid.NewGuid();
            var arquivoMock = GerarArquivoMock(codigoArquivo);
            var urlInvalidaHttp = "http://localhost-url-invalida-teste-download";

            repositorioArquivoMock
                .Setup(r => r.ObterPorCodigo(codigoArquivo))
                .ReturnsAsync(arquivoMock);

            servicoArmazenamentoMock
                .Setup(s => s.Obter(It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(urlInvalidaHttp);

            // Act
            Func<Task> acao = async () => await sut.Download(codigoArquivo);

            // Assert
            await acao.Should().ThrowAsync<HttpRequestException>();
        }

        [Fact]
        public async Task DadoUrlInvalidaParaWebClient_QuandoGerarMiniatura_EntaoLancaExcecaoDeRede()
        {
            // Arrange
            var codigoArquivo = Guid.NewGuid();
            var arquivoMock = GerarArquivoMock(codigoArquivo);
            var urlInvalidaWebClient = "http://localhost-url-invalida-teste-miniatura";

            repositorioArquivoMock
                .Setup(r => r.ObterPorCodigo(codigoArquivo))
                .ReturnsAsync(arquivoMock);

            servicoArmazenamentoMock
                .Setup(s => s.Obter(It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(urlInvalidaWebClient);

            // Act
            Func<Task> acao = async () => await sut.GerarMiniatura(codigoArquivo);

            // Assert
            await acao.Should().ThrowAsync<System.Net.WebException>();

            repositorioArquivoMock.Verify(r => r.ObterPorCodigo(codigoArquivo), Times.Once);
            servicoArmazenamentoMock.Verify(s => s.Obter(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        // ================= HELPER BOGUS GENERATORS ================= //

        private static Arquivo GerarArquivoMock(Guid codigoArquivo)
        {
            return new Faker<Arquivo>("pt_BR")
                .RuleFor(a => a.Codigo, codigoArquivo)
                .RuleFor(a => a.Nome, f => f.System.FileName("jpg"))
                .RuleFor(a => a.TipoConteudo, "image/jpeg")
                .RuleFor(a => a.Tipo, f => f.PickRandom<TipoArquivo>())
                .Generate();
        }
    }
}