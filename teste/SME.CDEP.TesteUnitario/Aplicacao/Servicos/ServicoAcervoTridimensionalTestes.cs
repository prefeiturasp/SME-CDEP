using AutoMapper;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using System.Data;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoAcervoTridimensionalTestes
    {
        private readonly Mock<IRepositorioAcervoTridimensional> _repositorioAcervoTridimensionalMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ITransacao> _transacaoMock;
        private readonly Mock<IServicoAcervo> _servicoAcervoMock;
        private readonly Mock<IRepositorioArquivo> _repositorioArquivoMock;
        private readonly Mock<IRepositorioAcervoTridimensionalArquivo> _repositorioAcervoTridimensionalArquivoMock;
        private readonly Mock<IServicoMoverArquivoTemporario> _servicoMoverArquivoTemporarioMock;
        private readonly ServicoAcervoTridimensional _sut;

        public ServicoAcervoTridimensionalTestes()
        {
            var mocker = new AutoMocker();

            _repositorioAcervoTridimensionalMock = mocker.GetMock<IRepositorioAcervoTridimensional>();
            _mapperMock = mocker.GetMock<IMapper>();
            _transacaoMock = mocker.GetMock<ITransacao>();
            _servicoAcervoMock = mocker.GetMock<IServicoAcervo>();
            _repositorioArquivoMock = mocker.GetMock<IRepositorioArquivo>();
            _repositorioAcervoTridimensionalArquivoMock = mocker.GetMock<IRepositorioAcervoTridimensionalArquivo>();
            _servicoMoverArquivoTemporarioMock = mocker.GetMock<IServicoMoverArquivoTemporario>();

            _sut = mocker.CreateInstance<ServicoAcervoTridimensional>();
        }

        [Fact]
        public async Task DadoCreditosAutoresPreenchidos_QuandoChamarInserir_EntaoLancaNegocioException()
        {
            // Arrange
            var dto = new AcervoTridimensionalCadastroDTO
            {
                CreditosAutoresIds = [1, 2]
            };

            // Act
            var act = async () => await _sut.Inserir(dto);

            // Assert
            await act.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.ESSE_ACERVO_NAO_POSSUI_CREDITO_OU_AUTOR);
        }

        [Fact]
        public async Task DadoLarguraComFormatoInvalido_QuandoChamarInserir_EntaoLancaNegocioException()
        {
            // Arrange
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Largura = "valor_invalido"
            };

            // Act
            var act = async () => await _sut.Inserir(dto);

            // Assert
            await act.Should().ThrowAsync<NegocioException>()
                .WithMessage(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.LARGURA));
        }

        [Fact]
        public async Task DadoAlturaComFormatoInvalido_QuandoChamarInserir_EntaoLancaNegocioException()
        {
            // Arrange
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Altura = "valor_invalido"
            };

            // Act
            var act = async () => await _sut.Inserir(dto);

            // Assert
            await act.Should().ThrowAsync<NegocioException>()
                .WithMessage(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.ALTURA));
        }

        [Fact]
        public async Task DadoProfundidadeComFormatoInvalido_QuandoChamarInserir_EntaoLancaNegocioException()
        {
            // Arrange
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Profundidade = "valor_invalido"
            };

            // Act
            var act = async () => await _sut.Inserir(dto);

            // Assert
            await act.Should().ThrowAsync<NegocioException>()
                .WithMessage(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.PROFUNDIDADE));
        }

        [Fact]
        public async Task DadoDiametroComFormatoInvalido_QuandoChamarInserir_EntaoLancaNegocioException()
        {
            // Arrange
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Diametro = "valor_invalido"
            };

            // Act
            var act = async () => await _sut.Inserir(dto);

            // Assert
            await act.Should().ThrowAsync<NegocioException>()
                .WithMessage(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.DIAMETRO));
        }

        [Fact]
        public async Task DadoAcervoValidoComArquivos_QuandoChamarInserir_EntaoExecutaFluxoCompletoERetornaId()
        {
            // Arrange
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Codigo = "COD123",
                Arquivos = [1, 2]
            };
            var arquivos = new List<Arquivo> { new() { Id = 1 }, new() { Id = 2 } };
            var acervoMapeado = new Acervo { Codigo = "COD123" };
            var acervoTridimensionalMapeado = new AcervoTridimensional();
            var acervoIdRetornado = 99L;
            var transactionMock = new Mock<IDbTransaction>();

            _repositorioArquivoMock.Setup(r => r.ObterPorIds(dto.Arquivos)).ReturnsAsync(arquivos);
            _mapperMock.Setup(m => m.Map<Acervo>(dto)).Returns(acervoMapeado);
            _mapperMock.Setup(m => m.Map<AcervoTridimensional>(dto)).Returns(acervoTridimensionalMapeado);
            _transacaoMock.Setup(t => t.Iniciar()).Returns(transactionMock.Object);
            _servicoAcervoMock.Setup(s => s.Inserir(acervoMapeado)).ReturnsAsync(acervoIdRetornado);

            // Act
            var resultado = await _sut.Inserir(dto);

            // Assert
            resultado.Should().Be(acervoIdRetornado);
            acervoMapeado.Codigo.Should().EndWith(Constantes.SIGLA_ACERVO_TRIDIMENSIONAL);
            _repositorioAcervoTridimensionalMock.Verify(r => r.Inserir(acervoTridimensionalMapeado), Times.Once);
            _repositorioAcervoTridimensionalArquivoMock.Verify(r => r.Inserir(It.IsAny<AcervoTridimensionalArquivo>()), Times.Exactly(2));
            transactionMock.Verify(t => t.Commit(), Times.Once);
            _servicoMoverArquivoTemporarioMock.Verify(s => s.Mover(TipoArquivo.AcervoTridimensional, It.IsAny<Arquivo>()), Times.Exactly(2));
        }

        [Fact]
        public async Task DadoAcervoValidoSemArquivos_QuandoChamarInserir_EntaoExecutaFluxoSemProcessarArquivos()
        {
            // Arrange
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Codigo = $"COD123{Constantes.SIGLA_ACERVO_TRIDIMENSIONAL}",
                Arquivos = null
            };
            var acervoMapeado = new Acervo { Codigo = dto.Codigo };
            var acervoTridimensionalMapeado = new AcervoTridimensional();
            var acervoIdRetornado = 99L;
            var transactionMock = new Mock<IDbTransaction>();

            _mapperMock.Setup(m => m.Map<Acervo>(dto)).Returns(acervoMapeado);
            _mapperMock.Setup(m => m.Map<AcervoTridimensional>(dto)).Returns(acervoTridimensionalMapeado);
            _transacaoMock.Setup(t => t.Iniciar()).Returns(transactionMock.Object);
            _servicoAcervoMock.Setup(s => s.Inserir(acervoMapeado)).ReturnsAsync(acervoIdRetornado);

            // Act
            var resultado = await _sut.Inserir(dto);

            // Assert
            resultado.Should().Be(acervoIdRetornado);
            acervoMapeado.Codigo.Should().Be(dto.Codigo); // Não duplicou a sigla
            _repositorioAcervoTridimensionalArquivoMock.Verify(r => r.Inserir(It.IsAny<AcervoTridimensionalArquivo>()), Times.Never);
            transactionMock.Verify(t => t.Commit(), Times.Once);
            _servicoMoverArquivoTemporarioMock.Verify(s => s.Mover(It.IsAny<TipoArquivo>(), It.IsAny<Arquivo>()), Times.Never);
        }

        [Fact]
        public async Task DadoErroDePersistencia_QuandoChamarInserir_EntaoRealizaRollbackERethrowException()
        {
            // Arrange
            var dto = new AcervoTridimensionalCadastroDTO { Codigo = "COD" };
            var transactionMock = new Mock<IDbTransaction>();

            _mapperMock.Setup(m => m.Map<Acervo>(dto)).Returns(new Acervo { Codigo = "COD" });
            _mapperMock.Setup(m => m.Map<AcervoTridimensional>(dto)).Returns(new AcervoTridimensional());
            _transacaoMock.Setup(t => t.Iniciar()).Returns(transactionMock.Object);
            _servicoAcervoMock.Setup(s => s.Inserir(It.IsAny<Acervo>())).ThrowsAsync(new System.Exception("Erro DB"));

            // Act
            var act = async () => await _sut.Inserir(dto);

            // Assert
            await act.Should().ThrowAsync<System.Exception>().WithMessage("Erro DB");
            transactionMock.Verify(t => t.Rollback(), Times.Once);
            transactionMock.Verify(t => t.Dispose(), Times.Once);
        }

        [Fact]
        public async Task DadoRegistrosExistentes_QuandoChamarObterTodos_EntaoRetornaColecaoMapeada()
        {
            // Arrange
            var acervos = new List<AcervoTridimensional>
            {
                new() { Id = 1, Procedencia = "Proc A" },
                new() { Id = 2, Procedencia = "Proc B" }
            };
            var acervoDTOs = new List<AcervoTridimensionalDTO>
            {
                new() { Id = 1, Procedencia = "Proc A" },
                new() { Id = 2, Procedencia = "Proc B" }
            };

            _repositorioAcervoTridimensionalMock.Setup(r => r.ObterTodos()).ReturnsAsync(acervos);
            _mapperMock.Setup(m => m.Map<AcervoTridimensionalDTO>(It.IsAny<AcervoTridimensional>()))
                       .Returns((AcervoTridimensional a) => acervoDTOs.First(d => d.Id == a.Id));

            // Act
            var resultado = await _sut.ObterTodos();

            // Assert
            resultado.Should().NotBeNullOrEmpty();
            resultado.Should().HaveCount(2);
            resultado.Select(r => r.Procedencia).Should().Contain("Proc A", "Proc B");
        }

        [Fact]
        public async Task DadoSemRegistros_QuandoChamarObterTodos_EntaoRetornaColecaoVazia()
        {
            // Arrange
            _repositorioAcervoTridimensionalMock.Setup(r => r.ObterTodos()).ReturnsAsync(new List<AcervoTridimensional>());

            // Act
            var resultado = await _sut.ObterTodos();

            // Assert
            resultado.Should().BeEmpty();
        }

        [Fact]
        public async Task DadoCreditosAutoresPreenchidos_QuandoChamarAlterar_EntaoLancaNegocioException()
        {
            // Arrange
            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                CreditosAutoresIds = [1]
            };

            // Act
            var act = async () => await _sut.Alterar(dto);

            // Assert
            await act.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.ESSE_ACERVO_NAO_POSSUI_CREDITO_OU_AUTOR);
        }

        [Fact]
        public async Task DadoDimensoesComFormatoInvalido_QuandoChamarAlterar_EntaoLancaNegocioException()
        {
            // Arrange
            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                Largura = "inválido"
            };

            // Act
            var act = async () => await _sut.Alterar(dto);

            // Assert
            await act.Should().ThrowAsync<NegocioException>()
                .WithMessage(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.LARGURA));
        }

        [Fact]
        public async Task DadoAcervoValido_QuandoChamarAlterar_EntaoExecutaFluxoCompletoERetornaDTO()
        {
            // Arrange
            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                Id = 10,
                AcervoId = 99,
                Codigo = "COD",
                Arquivos = [1, 2]
            };
            var acervoTridimensional = new AcervoTridimensional { Id = 10 };
            var acervoDto = new AcervoDto { Codigo = "COD" };
            var transactionMock = new Mock<IDbTransaction>();
            var arquivosAntigos = new List<AcervoTridimensionalArquivo> { new() { ArquivoId = 1 } };
            var arquivosRetornadosBase = new List<Arquivo> { new() { Id = 2 } };
            var acervoCompleto = new AcervoTridimensionalCompleto { Id = 10, Codigo = $"COD{Constantes.SIGLA_ACERVO_TRIDIMENSIONAL}" };
            var dtoFinalEsperado = new AcervoTridimensionalDTO { Id = 10, Codigo = "COD" };

            _repositorioArquivoMock.Setup(r => r.ObterPorIds(It.IsAny<long[]>())).ReturnsAsync(arquivosRetornadosBase);

            _mapperMock.Setup(m => m.Map<AcervoTridimensional>(dto)).Returns(acervoTridimensional);
            _mapperMock.Setup(m => m.Map<AcervoDto>(dto)).Returns(acervoDto);
            _repositorioAcervoTridimensionalArquivoMock.Setup(r => r.ObterPorAcervoTridimensionalId(dto.Id)).ReturnsAsync(arquivosAntigos);
            _transacaoMock.Setup(t => t.Iniciar()).Returns(transactionMock.Object);

            _repositorioAcervoTridimensionalMock.Setup(r => r.ObterPorId(dto.AcervoId)).ReturnsAsync(acervoCompleto);
            _mapperMock.Setup(m => m.Map<AcervoTridimensionalDTO>(acervoCompleto)).Returns(dtoFinalEsperado);
            _mapperMock.Setup(m => m.Map<AuditoriaDTO>(acervoCompleto)).Returns(new AuditoriaDTO());

            // Act
            var resultado = await _sut.Alterar(dto);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(10);

            _servicoAcervoMock.Verify(s => s.Alterar(acervoDto), Times.Once);
            _repositorioAcervoTridimensionalMock.Verify(r => r.Atualizar(acervoTridimensional), Times.Once);
            _repositorioAcervoTridimensionalArquivoMock.Verify(r => r.Inserir(It.Is<AcervoTridimensionalArquivo>(a => a.ArquivoId == 2)), Times.Once);
            _repositorioAcervoTridimensionalArquivoMock.Verify(r => r.Excluir(It.Is<long[]>(l => l.Length == 0), acervoTridimensional.Id), Times.Once);
            transactionMock.Verify(t => t.Commit(), Times.Once);
            _servicoMoverArquivoTemporarioMock.Verify(s => s.Mover(TipoArquivo.AcervoTridimensional, It.IsAny<Arquivo>()), Times.Once);
        }

        [Fact]
        public async Task DadoErroDePersistencia_QuandoChamarAlterar_EntaoRealizaRollbackERethrowException()
        {
            // Arrange
            var dto = new AcervoTridimensionalAlteracaoDTO { Id = 10, AcervoId = 99, Codigo = "COD" };
            var transactionMock = new Mock<IDbTransaction>();

            _mapperMock.Setup(m => m.Map<AcervoTridimensional>(dto)).Returns(new AcervoTridimensional());
            _mapperMock.Setup(m => m.Map<AcervoDto>(dto)).Returns(new AcervoDto() { Codigo = "COD" });

            _repositorioAcervoTridimensionalArquivoMock.Setup(r => r.ObterPorAcervoTridimensionalId(dto.Id)).ReturnsAsync(new List<AcervoTridimensionalArquivo>());
            _repositorioArquivoMock.Setup(r => r.ObterPorIds(It.IsAny<long[]>())).ReturnsAsync(new List<Arquivo>());

            _transacaoMock.Setup(t => t.Iniciar()).Returns(transactionMock.Object);
            _servicoAcervoMock.Setup(s => s.Alterar(It.IsAny<AcervoDto>())).ThrowsAsync(new System.Exception("Erro Atualizacao DB"));

            // Act
            var act = async () => await _sut.Alterar(dto);

            // Assert
            await act.Should().ThrowAsync<System.Exception>().WithMessage("Erro Atualizacao DB");
            transactionMock.Verify(t => t.Rollback(), Times.Once);
            transactionMock.Verify(t => t.Dispose(), Times.Once);
        }

        [Fact]
        public async Task DadoAcervoInexistente_QuandoChamarObterPorId_EntaoRetornaNulo()
        {
            // Arrange
            long id = 99;
            _repositorioAcervoTridimensionalMock.Setup(r => r.ObterPorId(id)).ReturnsAsync((AcervoTridimensionalCompleto)null!);

            // Act
            var resultado = await _sut.ObterPorId(id);

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DadoAcervoExistente_QuandoChamarObterPorId_EntaoRetornaDTOMapeadoERemoveSufixo()
        {
            // Arrange
            long id = 1;
            var acervoBD = new AcervoTridimensionalCompleto { Id = 1, Codigo = $"COD{Constantes.SIGLA_ACERVO_TRIDIMENSIONAL}" };
            var dtoMapeado = new AcervoTridimensionalDTO { Id = 1, Codigo = "COD" };
            var auditoriaMapeada = new AuditoriaDTO();

            _repositorioAcervoTridimensionalMock.Setup(r => r.ObterPorId(id)).ReturnsAsync(acervoBD);
            _mapperMock.Setup(m => m.Map<AcervoTridimensionalDTO>(acervoBD)).Returns(dtoMapeado);
            _mapperMock.Setup(m => m.Map<AuditoriaDTO>(acervoBD)).Returns(auditoriaMapeada);

            // Act
            var resultado = await _sut.ObterPorId(id);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Codigo.Should().Be("COD");
            resultado.Auditoria.Should().BeSameAs(auditoriaMapeada);
            _mapperMock.Verify(m => m.Map<AcervoTridimensionalDTO>(acervoBD), Times.Once);
            _mapperMock.Verify(m => m.Map<AuditoriaDTO>(acervoBD), Times.Once);
        }

        [Fact]
        public async Task DadoAcervoExistente_QuandoChamarExcluir_EntaoInvocaServicoBaseERetornaVerdadeiro()
        {
            // Arrange
            long id = 10;
            _servicoAcervoMock.Setup(s => s.Excluir(id)).ReturnsAsync(true);

            // Act
            var resultado = await _sut.Excluir(id);

            // Assert
            resultado.Should().BeTrue();
            _servicoAcervoMock.Verify(s => s.Excluir(id), Times.Once);
        }
    }
}