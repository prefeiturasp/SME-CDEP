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
            // Arrange & Act
            var servico = new ServicoDownloadArquivo(repositorioArquivoMock.Object, servicoArmazenamentoMock.Object);

            // Assert
            servico.Should().NotBeNull();
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

        [Fact]
        public async Task DadoDownloadComSucesso_QuandoDownload_EntaoRetornaArquivoComDados()
        {
            // Arrange
            var codigoArquivo = Guid.NewGuid();
            var arquivoMock = GerarArquivoMock(codigoArquivo);
            var urlValida = "https://httpbin.org/image/jpeg";

            repositorioArquivoMock
                .Setup(r => r.ObterPorCodigo(codigoArquivo))
                .ReturnsAsync(arquivoMock);

            servicoArmazenamentoMock
                .Setup(s => s.Obter(It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(urlValida);

            // Act
            var resultado = await sut.Download(codigoArquivo);

            // Assert
            resultado.Item1.Should().NotBeEmpty();
            resultado.Item2.Should().Be(arquivoMock.TipoConteudo);
            resultado.Item3.Should().Be(arquivoMock.Nome);

            repositorioArquivoMock.Verify(r => r.ObterPorCodigo(codigoArquivo), Times.Once);
            servicoArmazenamentoMock.Verify(s => s.Obter(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task DadoStatusCodeNaoOk_QuandoDownload_EntaoRetornaArquivoVazio()
        {
            // Arrange
            var codigoArquivo = Guid.NewGuid();
            var arquivoMock = GerarArquivoMock(codigoArquivo);
            var urlComStatusNaoOk = "https://httpbin.org/status/404";

            repositorioArquivoMock
                .Setup(r => r.ObterPorCodigo(codigoArquivo))
                .ReturnsAsync(arquivoMock);

            servicoArmazenamentoMock
                .Setup(s => s.Obter(It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(urlComStatusNaoOk);

            // Act
            var resultado = await sut.Download(codigoArquivo);

            // Assert
            resultado.Item1.Should().BeEmpty();
            resultado.Item2.Should().Be(arquivoMock.TipoConteudo);
            resultado.Item3.Should().Be(arquivoMock.Nome);

            repositorioArquivoMock.Verify(r => r.ObterPorCodigo(codigoArquivo), Times.Once);
            servicoArmazenamentoMock.Verify(s => s.Obter(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task DadoDownloadPorTipoAcervoComSucesso_QuandoDownloadPorTipoAcervo_EntaoRetornaArquivoComDados()
        {
            // Arrange
            var tipoAcervo = TipoAcervo.Fotografico;
            var arquivoMock = GerarArquivoMock(Guid.NewGuid());
            var urlValida = "https://httpbin.org/image/jpeg";

            repositorioArquivoMock
                .Setup(r => r.ObterArquivoPorNomeTipoArquivo(It.IsAny<string>(), TipoArquivo.Sistema))
                .ReturnsAsync(arquivoMock);

            servicoArmazenamentoMock
                .Setup(s => s.Obter(It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(urlValida);

            // Act
            var resultado = await sut.DownloadPorTipoAcervo(tipoAcervo);

            // Assert
            resultado.Item1.Should().NotBeEmpty();
            resultado.Item2.Should().Be(arquivoMock.TipoConteudo);
            resultado.Item3.Should().Be(arquivoMock.Nome);

            repositorioArquivoMock.Verify(r => r.ObterArquivoPorNomeTipoArquivo(It.IsAny<string>(), TipoArquivo.Sistema), Times.Once);
            servicoArmazenamentoMock.Verify(s => s.Obter(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task DadoArquivoTemporario_QuandoDownload_EntaoVerificaParametroTemporario()
        {
            // Arrange
            var codigoArquivo = Guid.NewGuid();
            var arquivoMock = GerarArquivoMock(codigoArquivo, TipoArquivo.Temp);
            var urlInvalida = "http://localhost-url-invalida";

            repositorioArquivoMock
                .Setup(r => r.ObterPorCodigo(codigoArquivo))
                .ReturnsAsync(arquivoMock);

            servicoArmazenamentoMock
                .Setup(s => s.Obter(It.IsAny<string>(), true))
                .ReturnsAsync(urlInvalida);

            // Act
            Func<Task> acao = async () => await sut.Download(codigoArquivo);

            // Assert
            await acao.Should().ThrowAsync<HttpRequestException>();

            repositorioArquivoMock.Verify(r => r.ObterPorCodigo(codigoArquivo), Times.Once);
            servicoArmazenamentoMock.Verify(s => s.Obter(It.IsAny<string>(), true), Times.Once);
        }

        [Fact]
        public async Task DadoArquivoNaoTemporario_QuandoDownload_EntaoVerificaParametroNaoTemporario()
        {
            // Arrange
            var codigoArquivo = Guid.NewGuid();
            var arquivoMock = GerarArquivoMock(codigoArquivo, TipoArquivo.Sistema);
            var urlInvalida = "http://localhost-url-invalida";

            repositorioArquivoMock
                .Setup(r => r.ObterPorCodigo(codigoArquivo))
                .ReturnsAsync(arquivoMock);

            servicoArmazenamentoMock
                .Setup(s => s.Obter(It.IsAny<string>(), false))
                .ReturnsAsync(urlInvalida);

            // Act
            Func<Task> acao = async () => await sut.Download(codigoArquivo);

            // Assert
            await acao.Should().ThrowAsync<HttpRequestException>();

            repositorioArquivoMock.Verify(r => r.ObterPorCodigo(codigoArquivo), Times.Once);
            servicoArmazenamentoMock.Verify(s => s.Obter(It.IsAny<string>(), false), Times.Once);
        }

        [Fact]
        public async Task DadoExtensaoArquivo_QuandoDownload_EntaoMontaNomeComCodigo()
        {
            // Arrange
            var codigoArquivo = Guid.NewGuid();
            var nomeArquivo = "documento.pdf";
            var arquivoMock = GerarArquivoMock(codigoArquivo, nomeArquivo: nomeArquivo);
            var urlInvalida = "http://localhost-url-invalida";

            repositorioArquivoMock
                .Setup(r => r.ObterPorCodigo(codigoArquivo))
                .ReturnsAsync(arquivoMock);

            servicoArmazenamentoMock
                .Setup(s => s.Obter(It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(urlInvalida);

            // Act
            Func<Task> acao = async () => await sut.Download(codigoArquivo);

            // Assert
            await acao.Should().ThrowAsync<HttpRequestException>();

            repositorioArquivoMock.Verify(r => r.ObterPorCodigo(codigoArquivo), Times.Once);
            servicoArmazenamentoMock.Verify(
                s => s.Obter($"{codigoArquivo}.pdf", It.IsAny<bool>()), 
                Times.Once);
        }

        [Fact]
        public async Task DadoDownloadPorTipoAcervoFotografico_QuandoDownloadPorTipoAcervo_EntaoRetornaValoresCorretos()
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
            await acao.Should().ThrowAsync<NegocioException>();

            repositorioArquivoMock.Verify(
                r => r.ObterArquivoPorNomeTipoArquivo(It.IsAny<string>(), TipoArquivo.Sistema), 
                Times.Once);
        }

        [Fact]
        public async Task DadoNullArquivo_QuandoDownload_EntaoLancaException()
        {
            // Arrange
            var codigoArquivo = Guid.NewGuid();

            repositorioArquivoMock
                .Setup(r => r.ObterPorCodigo(codigoArquivo))
                .ReturnsAsync((Arquivo)null);

            // Act
            Func<Task> acao = async () => await sut.Download(codigoArquivo);

            // Assert
            await acao.Should().ThrowAsync<NullReferenceException>();

            repositorioArquivoMock.Verify(r => r.ObterPorCodigo(codigoArquivo), Times.Once);
        }

        private static Arquivo GerarArquivoMock(Guid codigoArquivo, TipoArquivo tipo = TipoArquivo.Sistema, string nomeArquivo = null)
        {
            return new Faker<Arquivo>("pt_BR")
                .RuleFor(a => a.Codigo, codigoArquivo)
                .RuleFor(a => a.Nome, nomeArquivo ?? new Faker().System.FileName("jpg"))
                .RuleFor(a => a.TipoConteudo, "image/jpeg")
                .RuleFor(a => a.Tipo, tipo)
                .Generate();
        }
    }
}