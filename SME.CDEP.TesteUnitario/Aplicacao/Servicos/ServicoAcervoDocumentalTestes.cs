using AutoMapper;
using FluentAssertions;
using Moq;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;
using System.Data;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoAcervoDocumentalTestes
    {
        private readonly Mock<IRepositorioAcervoDocumental> _repositorioAcervoDocumentalMock;
        private readonly Mock<IRepositorioAcervoDocumentalArquivo> _repositorioAcervoDocumentalArquivoMock;
        private readonly Mock<IRepositorioAcervoDocumentalAcessoDocumento> _repositorioAcervoDocumentalAcessoDocumentoMock;
        private readonly Mock<IRepositorioAcessoDocumento> _repositorioAcessoDocumentoMock;
        private readonly Mock<IRepositorioAcervo> _repositorioAcervoMock;
        private readonly Mock<IRepositorioArquivo> _repositorioArquivoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IServicoAcervo> _servicoAcervoMock;
        private readonly Mock<ITransacao> _transacaoMock;
        private readonly Mock<IDbTransaction> _transacaoScopeMock;
        private readonly Mock<IServicoMoverArquivoTemporario> _servicoMoverArquivoTemporarioMock;
        private readonly Mock<IServicoArmazenamento> _servicoArmazenamentoMock;

        private readonly ServicoAcervoDocumental _servico;

        public ServicoAcervoDocumentalTestes()
        {
            _repositorioAcervoDocumentalMock = new Mock<IRepositorioAcervoDocumental>();
            _repositorioAcervoDocumentalArquivoMock = new Mock<IRepositorioAcervoDocumentalArquivo>();
            _repositorioAcervoDocumentalAcessoDocumentoMock = new Mock<IRepositorioAcervoDocumentalAcessoDocumento>();
            _repositorioAcessoDocumentoMock = new Mock<IRepositorioAcessoDocumento>();
            _repositorioAcervoMock = new Mock<IRepositorioAcervo>();
            _repositorioArquivoMock = new Mock<IRepositorioArquivo>();
            _mapperMock = new Mock<IMapper>();
            _servicoAcervoMock = new Mock<IServicoAcervo>();
            _transacaoMock = new Mock<ITransacao>();
            _transacaoScopeMock = new Mock<IDbTransaction>();
            _servicoMoverArquivoTemporarioMock = new Mock<IServicoMoverArquivoTemporario>();
            _servicoArmazenamentoMock = new Mock<IServicoArmazenamento>();

            _transacaoMock.Setup(t => t.Iniciar()).Returns(_transacaoScopeMock.Object);

            _servico = new ServicoAcervoDocumental(
                _repositorioAcervoMock.Object,
                _repositorioAcervoDocumentalMock.Object,
                _mapperMock.Object,
                _transacaoMock.Object,
                _servicoAcervoMock.Object,
                _repositorioArquivoMock.Object,
                _repositorioAcessoDocumentoMock.Object,
                _repositorioAcervoDocumentalArquivoMock.Object,
                _servicoMoverArquivoTemporarioMock.Object,
                _servicoArmazenamentoMock.Object,
                _repositorioAcervoDocumentalAcessoDocumentoMock.Object
            );
        }

        [Fact]
        public async Task Inserir_ComDadosValidos_DeveSalvarCorretamente()
        {
            // Arrange
            var dto = new AcervoDocumentalCadastroDTO { Altura = "10,50", Largura = "20,00", Arquivos = new long[] { 1, 2 }, AcessoDocumentosIds = new long[] { 3 } };
            var arquivos = new List<Arquivo> { new Arquivo { Id = 1 }, new Arquivo { Id = 2 } };
            var acessos = new List<AcessoDocumento> { new AcessoDocumento { Id = 3 } };
            var acervo = new Acervo();
            var acervoDocumental = new AcervoDocumental();
            var novoAcervoId = 12346;

            _repositorioArquivoMock.Setup(r => r.ObterPorIds(dto.Arquivos)).ReturnsAsync(arquivos);
            _repositorioAcessoDocumentoMock.Setup(r => r.ObterPorIds(dto.AcessoDocumentosIds)).ReturnsAsync(acessos);
            _mapperMock.Setup(m => m.Map<Acervo>(dto)).Returns(acervo);
            _mapperMock.Setup(m => m.Map<AcervoDocumental>(dto)).Returns(acervoDocumental);
            _servicoAcervoMock.Setup(s => s.Inserir(acervo)).ReturnsAsync(novoAcervoId);

            // Act
            var resultado = await _servico.Inserir(dto);

            // Assert
            resultado.Should().Be(novoAcervoId);
            acervo.TipoAcervoId.Should().Be((int)TipoAcervo.DocumentacaoTextual);
            acervoDocumental.AcervoId.Should().Be(novoAcervoId);

            _servicoAcervoMock.Verify(s => s.Inserir(acervo), Times.Once);
            _repositorioAcervoDocumentalMock.Verify(r => r.Inserir(acervoDocumental), Times.Once);
            _repositorioAcervoDocumentalArquivoMock.Verify(r => r.Inserir(It.IsAny<AcervoDocumentalArquivo>()), Times.Exactly(arquivos.Count));
            _repositorioAcervoDocumentalAcessoDocumentoMock.Verify(r => r.Inserir(It.IsAny<AcervoDocumentalAcessoDocumento>()), Times.Once);
            _transacaoScopeMock.Verify(t => t.Commit(), Times.Once);
        }

        [Theory]
        [InlineData("abc", "10.5")]
        [InlineData("10.5", "xyz")]
        public async Task Inserir_ComDimensoesInvalidas_DeveLancarNegocioException(string largura, string altura)
        {
            // Arrange
            var dto = new AcervoDocumentalCadastroDTO { Altura = altura, Largura = largura, AcessoDocumentosIds = new long[] { 1 } };

            // Act
            Func<Task> acao = async () => await _servico.Inserir(dto);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>();
            _servicoAcervoMock.Verify(s => s.Inserir(It.IsAny<Acervo>()), Times.Never);
            _transacaoScopeMock.Verify(t => t.Commit(), Times.Never);
        }

        [Fact]
        public async Task Inserir_QuandoRepositorioFalha_DeveExecutarRollbackELancarExcecao()
        {
            // Arrange
            var dto = new AcervoDocumentalCadastroDTO { Altura = "10,05", Largura = "20,00", AcessoDocumentosIds = new long[] { 1 } };
            _mapperMock.Setup(m => m.Map<Acervo>(It.IsAny<AcervoDocumentalCadastroDTO>())).Returns(new Acervo());
            _mapperMock.Setup(m => m.Map<AcervoDocumental>(It.IsAny<AcervoDocumentalCadastroDTO>())).Returns(new AcervoDocumental());
            _repositorioAcessoDocumentoMock.Setup(r => r.ObterPorIds(It.IsAny<long[]>())).ReturnsAsync(new List<AcessoDocumento>());
            _repositorioAcervoDocumentalMock.Setup(r => r.Inserir(It.IsAny<AcervoDocumental>())).ThrowsAsync(new Exception("Falha no banco"));

            // Act
            Func<Task> acao = async () => await _servico.Inserir(dto);

            // Assert
            await acao.Should().ThrowAsync<Exception>().WithMessage("Falha no banco");
            _transacaoScopeMock.Verify(t => t.Rollback(), Times.Once);
            _transacaoScopeMock.Verify(t => t.Commit(), Times.Never);
        }

        [Fact]
        public async Task Alterar_ComDadosValidos_DeveAtualizarCorretamente()
        {
            // Arrange
            var dto = new AcervoDocumentalAlteracaoDTO { Id = 1, AcervoId = 10, Altura = "1,00", Largura = "20,00", Arquivos = new long[] { 1, 3 }, AcessoDocumentosIds = new long[] { 5, 6 } };

            // Mock dos dados existentes
            _repositorioAcervoDocumentalArquivoMock.Setup(r => r.ObterPorAcervoDocumentalId(dto.Id)).ReturnsAsync(new List<AcervoDocumentalArquivo> { new AcervoDocumentalArquivo { ArquivoId = 1 }, new AcervoDocumentalArquivo { ArquivoId = 2 } });
            _repositorioAcervoDocumentalAcessoDocumentoMock.Setup(r => r.ObterPorAcervoDocumentalId(dto.Id)).ReturnsAsync(new List<AcervoDocumentalAcessoDocumento> { new AcervoDocumentalAcessoDocumento { AcessoDocumentoId = 5 }, new AcervoDocumentalAcessoDocumento { AcessoDocumentoId = 7 } });

            // Mock dos mapeamentos e chamadas de serviço/repositório
            _mapperMock.Setup(m => m.Map<AcervoDocumental>(dto)).Returns(new AcervoDocumental { Id = dto.Id, AcervoId = dto.AcervoId });
            _mapperMock.Setup(m => m.Map<AcervoDTO>(dto)).Returns(new AcervoDTO());
            _repositorioAcervoDocumentalMock.Setup(r => r.ObterPorId(dto.AcervoId)).ReturnsAsync(new AcervoDocumentalCompleto());
            _mapperMock.Setup(m => m.Map<AcervoDocumentalDTO>(It.IsAny<AcervoDocumentalCompleto>())).Returns(new AcervoDocumentalDTO());
            _mapperMock.Setup(m => m.Map<AuditoriaDTO>(It.IsAny<AcervoDocumental>())).Returns(new AuditoriaDTO());

            // Act
            await _servico.Alterar(dto);

            // Assert
            _servicoAcervoMock.Verify(s => s.Alterar(It.IsAny<AcervoDTO>()), Times.Once);
            _repositorioAcervoDocumentalMock.Verify(r => r.Atualizar(It.IsAny<AcervoDocumental>()), Times.Once);

            // Arquivo ID 3 foi inserido
            _repositorioAcervoDocumentalArquivoMock.Verify(r => r.Inserir(It.Is<AcervoDocumentalArquivo>(a => a.ArquivoId == 3)), Times.Once);
            // Arquivo ID 2 foi excluído
            _repositorioAcervoDocumentalArquivoMock.Verify(r => r.Excluir(It.Is<long[]>(ids => ids.Contains(2L)), dto.Id), Times.Once);

            // AcessoDocumento ID 6 foi inserido
            _repositorioAcervoDocumentalAcessoDocumentoMock.Verify(r => r.Inserir(It.Is<AcervoDocumentalAcessoDocumento>(a => a.AcessoDocumentoId == 6)), Times.Once);
            // AcessoDocumento ID 7 foi excluído
            _repositorioAcervoDocumentalAcessoDocumentoMock.Verify(r => r.Excluir(It.Is<long[]>(ids => ids.Contains(7L)), dto.Id), Times.Once);

            _transacaoScopeMock.Verify(t => t.Commit(), Times.Once);
        }

        [Fact]
        public async Task ObterPorId_QuandoRegistroExiste_DeveRetornarDto()
        {
            // Arrange
            var id = 12346;
            var entidade = new AcervoDocumentalCompleto { Id = id, Titulo = "Documento Teste" };
            var dto = new AcervoDocumentalDTO { Id = id, Titulo = "Documento Teste" };
            var auditoriaDto = new AuditoriaDTO();

            _repositorioAcervoDocumentalMock.Setup(r => r.ObterPorId(id)).ReturnsAsync(entidade);
            _mapperMock.Setup(m => m.Map<AcervoDocumentalDTO>(entidade)).Returns(dto);
            _mapperMock.Setup(m => m.Map<AuditoriaDTO>(entidade)).Returns(auditoriaDto);

            // Act
            var resultado = await _servico.ObterPorId(id);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(id);
            resultado.Auditoria.Should().Be(auditoriaDto);
            _mapperMock.Verify(m => m.Map<AcervoDocumentalDTO>(entidade), Times.Once);
            _mapperMock.Verify(m => m.Map<AuditoriaDTO>(entidade), Times.Once);
        }

        [Fact]
        public async Task ObterPorId_QuandoRegistroNaoExiste_DeveRetornarNulo()
        {
            // Arrange
            var id = 12346;

            // Act
            var resultado = await _servico.ObterPorId(id);

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task Excluir_ComIdValido_DeveChamarServicoBase()
        {
            // Arrange
            var id = 12346;
            _servicoAcervoMock.Setup(s => s.Excluir(id)).ReturnsAsync(true);

            // Act
            var resultado = await _servico.Excluir(id);

            // Assert
            resultado.Should().BeTrue();
            _servicoAcervoMock.Verify(s => s.Excluir(id), Times.Once);
        }
    }
}
