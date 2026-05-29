using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoAcessoDocumentoTeste
    {
        private readonly AutoMocker _mocker;
        private readonly ServicoAcessoDocumento _sut;

        public ServicoAcessoDocumentoTeste()
        {
            _mocker = new AutoMocker();
            _sut = _mocker.CreateInstance<ServicoAcessoDocumento>();
        }

        #region Testes de Construtor

        [Fact]
        public void DadoRepositorioNulo_QuandoConstruir_EntaoLancaArgumentNullException()
        {
            // Arrange
            var mapper = _mocker.GetMock<AutoMapper.IMapper>();

            // Act
            Action acao = () => _ = new ServicoAcessoDocumento(null!, mapper.Object);

            // Assert
            acao.Should().Throw<ArgumentNullException>()
                .WithMessage("*repositorio*");
        }

        [Fact]
        public void DadoMapperNulo_QuandoConstruir_EntaoLancaArgumentNullException()
        {
            // Arrange
            var repositorio = _mocker.GetMock<IRepositorioAcessoDocumento>();

            // Act
            Action acao = () => _ = new ServicoAcessoDocumento(repositorio.Object, null!);

            // Assert
            acao.Should().Throw<ArgumentNullException>()
                .WithMessage("*mapper*");
        }

        [Fact]
        public void DadoRepositorioEMapperValidos_QuandoConstruir_EntaoInstanciaComSucesso()
        {
            // Act
            var servico = _mocker.CreateInstance<ServicoAcessoDocumento>();

            // Assert
            servico.Should().NotBeNull();
            servico.Should().BeOfType<ServicoAcessoDocumento>();
        }

        #endregion

        #region Testes de Inserir

        [Fact]
        public async Task DadoAcessoDocumentoDTOValido_QuandoInserir_EntaoDeveRetornarIdMaiorQueZero()
        {
            // Arrange
            var acessoDocumentoDTO = CriarAcessoDocumentoDTOValido();
            const long idEsperado = 10;

            var acessoDocumentoMapeado = CriarAcessoDocumentoValido(id: idEsperado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<AcessoDocumento>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(acessoDocumentoMapeado);

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.Inserir(It.IsAny<AcessoDocumento>()))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.Inserir(acessoDocumentoDTO);

            // Assert
            resultado.Should().Be(idEsperado);
            resultado.Should().BeGreaterThan(0);
            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Verify(r => r.Inserir(It.IsAny<AcessoDocumento>()), Times.Once);
        }

        [Fact]
        public async Task DadoAcessoDocumentoDTOParaInserir_QuandoInserir_EntaoDeveMapearDTOParaEntidadeCorretamente()
        {
            // Arrange
            var acessoDocumentoDTO = CriarAcessoDocumentoDTOValido();
            var acessoDocumentoMapeado = CriarAcessoDocumentoValido();

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<AcessoDocumento>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(acessoDocumentoMapeado);

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.Inserir(It.IsAny<AcessoDocumento>()))
                .ReturnsAsync(1);

            // Act
            await _sut.Inserir(acessoDocumentoDTO);

            // Assert
            _mocker.GetMock<AutoMapper.IMapper>()
                .Verify(m => m.Map<AcessoDocumento>(It.IsAny<IdNomeExcluidoDTO>()), Times.Once);
        }

        [Fact]
        public async Task DadoAcessoDocumentoDTOValido_QuandoInserir_EntaoDeveInteragirComRepositorio()
        {
            // Arrange
            var acessoDocumentoDTO = CriarAcessoDocumentoDTOValido();
            var acessoDocumentoMapeado = CriarAcessoDocumentoValido();

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<AcessoDocumento>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(acessoDocumentoMapeado);

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.Inserir(It.IsAny<AcessoDocumento>()))
                .ReturnsAsync(5);

            // Act
            await _sut.Inserir(acessoDocumentoDTO);

            // Assert
            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Verify(r => r.Inserir(acessoDocumentoMapeado), Times.Once);
        }

        #endregion

        #region Testes de ObterTodos

        [Fact]
        public async Task DadoRepositorioComAcessosDocumento_QuandoObterTodos_EntaoDeveRetornarListaDeAcessoDocumentoDTOs()
        {
            // Arrange
            var acessosDocumento = new List<AcessoDocumento>
            {
                CriarAcessoDocumentoValido(id: 1, nome: "Público", excluido: false),
                CriarAcessoDocumentoValido(id: 2, nome: "Restrito", excluido: false),
                CriarAcessoDocumentoValido(id: 3, nome: "Interno", excluido: false)
            };

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(acessosDocumento);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(It.IsAny<AcessoDocumento>()))
                .Returns<AcessoDocumento>(a => new IdNomeExcluidoDTO
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    Excluido = a.Excluido
                });

            // Act
            var resultado = await _sut.ObterTodos();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(3);
            resultado.Should().AllSatisfy(a => a.Should().BeOfType<IdNomeExcluidoDTO>());
        }

        [Fact]
        public async Task DadoRepositorioComAcessosDocumentoAtivosEExcluidos_QuandoObterTodos_EntaoDeveRetornarApenasAtivos()
        {
            // Arrange
            var acessosDocumento = new List<AcessoDocumento>
            {
                CriarAcessoDocumentoValido(id: 1, nome: "Público", excluido: false),
                CriarAcessoDocumentoValido(id: 2, nome: "Restrito", excluido: true),
                CriarAcessoDocumentoValido(id: 3, nome: "Interno", excluido: false),
                CriarAcessoDocumentoValido(id: 4, nome: "Especial", excluido: true)
            };

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(acessosDocumento);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(It.IsAny<AcessoDocumento>()))
                .Returns<AcessoDocumento>(a => new IdNomeExcluidoDTO
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    Excluido = a.Excluido
                });

            // Act
            var resultado = await _sut.ObterTodos();

            // Assert
            resultado.Should().HaveCount(2);
            resultado.Should().AllSatisfy(a => a.Excluido.Should().BeFalse());
        }

        [Fact]
        public async Task DadoRepositorioSemAcessosDocumento_QuandoObterTodos_EntaoDeveRetornarListaVazia()
        {
            // Arrange
            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync([]);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(It.IsAny<AcessoDocumento>()))
                .Returns<AcessoDocumento>(a => new IdNomeExcluidoDTO
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    Excluido = a.Excluido
                });

            // Act
            var resultado = await _sut.ObterTodos();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
        }

        #endregion

        #region Testes de ObterPorId

        [Fact]
        public async Task DadoAcessoDocumentoExistenteNaoExcluido_QuandoObterPorId_EntaoDeveRetornarAcessoDocumentoDTO()
        {
            // Arrange
            var acessoDocumento = CriarAcessoDocumentoValido(id: 5, nome: "Público", excluido: false);
            var acessoDocumentoDTO = new IdNomeExcluidoDTO
            {
                Id = acessoDocumento.Id,
                Nome = acessoDocumento.Nome,
                Excluido = acessoDocumento.Excluido
            };

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.ObterPorId(5))
                .ReturnsAsync(acessoDocumento);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(acessoDocumento))
                .Returns(acessoDocumentoDTO);

            // Act
            var resultado = await _sut.ObterPorId(5);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<IdNomeExcluidoDTO>();
            resultado.Id.Should().Be(5);
            resultado.Nome.Should().Be("Público");
        }

        [Fact]
        public async Task DadoAcessoDocumentoExcluido_QuandoObterPorId_EntaoDeveRetornarNull()
        {
            // Arrange
            var acessoDocumento = CriarAcessoDocumentoValido(id: 5, excluido: true);

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.ObterPorId(5))
                .ReturnsAsync(acessoDocumento);

            // Act
            var resultado = await _sut.ObterPorId(5);

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DadoIdInexistente_QuandoObterPorId_EntaoDeveRetornarNull()
        {
            // Arrange
            var acessoDocumentoNulo = (AcessoDocumento)null!;

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync(acessoDocumentoNulo);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(null))
                .Returns((IdNomeExcluidoDTO)null!);

            // Act
            var resultado = await _sut.ObterPorId(999);

            // Assert
            resultado.Should().BeNull();
        }

        #endregion

        #region Testes de Alterar

        [Fact]
        public async Task DadoAcessoDocumentoDTOExistente_QuandoAlterar_EntaoDeveRetornarAcessoDocumentoDTOAlterado()
        {
            // Arrange
            var acessoDocumentoDTO = CriarAcessoDocumentoDTOValido(id: 3, nome: "Confidencial");
            var acessoDocumentoAlterado = CriarAcessoDocumentoValido(id: 3, nome: "Confidencial");
            var acessoDocumentoDTOAlterado = new IdNomeExcluidoDTO
            {
                Id = acessoDocumentoAlterado.Id,
                Nome = acessoDocumentoAlterado.Nome,
                Excluido = acessoDocumentoAlterado.Excluido
            };

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.Atualizar(It.IsAny<AcessoDocumento>()))
                .ReturnsAsync(acessoDocumentoAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<AcessoDocumento>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(acessoDocumentoAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(acessoDocumentoAlterado))
                .Returns(acessoDocumentoDTOAlterado);

            // Act
            var resultado = await _sut.Alterar(acessoDocumentoDTO);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<IdNomeExcluidoDTO>();
            resultado.Id.Should().Be(3);
            resultado.Nome.Should().Be("Confidencial");
        }

        [Fact]
        public async Task DadoAcessoDocumentoDTOParaAlterar_QuandoAlterar_EntaoDeveInteragirComRepositorio()
        {
            // Arrange
            var acessoDocumentoDTO = CriarAcessoDocumentoDTOValido(id: 3);
            var acessoDocumentoMapeado = CriarAcessoDocumentoValido(id: 3);
            var acessoDocumentoDTORetorno = new IdNomeExcluidoDTO
            {
                Id = 3,
                Nome = "Público",
                Excluido = false
            };

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<AcessoDocumento>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(acessoDocumentoMapeado);

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.Atualizar(It.IsAny<AcessoDocumento>()))
                .ReturnsAsync(acessoDocumentoMapeado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(acessoDocumentoMapeado))
                .Returns(acessoDocumentoDTORetorno);

            // Act
            await _sut.Alterar(acessoDocumentoDTO);

            // Assert
            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Verify(r => r.Atualizar(It.IsAny<AcessoDocumento>()), Times.Once);
        }

        #endregion

        #region Testes de Excluir

        [Fact]
        public async Task DadoAcessoDocumentoExistente_QuandoExcluir_EntaoDeveRetornarVerdadeiro()
        {
            // Arrange
            var acessoDocumento = CriarAcessoDocumentoValido(id: 7, excluido: false);
            var acessoDocumentoExcluido = CriarAcessoDocumentoValido(id: 7, excluido: true);
            var acessoDocumentoDTO = new IdNomeExcluidoDTO
            {
                Id = acessoDocumento.Id,
                Nome = acessoDocumento.Nome,
                Excluido = acessoDocumento.Excluido
            };

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.ObterPorId(7))
                .ReturnsAsync(acessoDocumento);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(acessoDocumento))
                .Returns(acessoDocumentoDTO);

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.Atualizar(It.IsAny<AcessoDocumento>()))
                .ReturnsAsync(acessoDocumentoExcluido);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<AcessoDocumento>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(acessoDocumentoExcluido);

            // Act
            var resultado = await _sut.Excluir(7);

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact]
        public async Task DadoAcessoDocumentoParaExcluir_QuandoExcluir_EntaoDeveMarcarComoExcluido()
        {
            // Arrange
            var acessoDocumento = CriarAcessoDocumentoValido(id: 7, excluido: false);
            var acessoDocumentoExcluido = CriarAcessoDocumentoValido(id: 7, excluido: true);
            var acessoDocumentoDTO = new IdNomeExcluidoDTO
            {
                Id = acessoDocumento.Id,
                Nome = acessoDocumento.Nome,
                Excluido = acessoDocumento.Excluido
            };

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.ObterPorId(7))
                .ReturnsAsync(acessoDocumento);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(acessoDocumento))
                .Returns(acessoDocumentoDTO);

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.Atualizar(It.IsAny<AcessoDocumento>()))
                .ReturnsAsync(acessoDocumentoExcluido);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<AcessoDocumento>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(acessoDocumentoExcluido);

            // Act
            await _sut.Excluir(7);

            // Assert
            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Verify(r => r.Atualizar(It.Is<AcessoDocumento>(a =>
                    a.Excluido
                )), Times.Once);
        }

        #endregion

        #region Testes de ObterPorNome

        [Fact]
        public async Task DadoNomeValido_QuandoObterPorNome_EntaoDeveRetornarId()
        {
            // Arrange
            const string nome = "Público";
            const long idEsperado = 5;

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.ObterPorNome(nome))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.ObterPorNome(nome);

            // Assert
            resultado.Should().Be(idEsperado);
            resultado.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task DadoNomeValido_QuandoObterPorNome_EntaoDeveInteragirComRepositorio()
        {
            // Arrange
            const string nome = "Restrito";

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.ObterPorNome(nome))
                .ReturnsAsync(10);

            // Act
            await _sut.ObterPorNome(nome);

            // Assert
            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Verify(r => r.ObterPorNome(nome), Times.Once);
        }

        [Fact]
        public async Task DadoNomeNaoExistente_QuandoObterPorNome_EntaoDeveRetornarZero()
        {
            // Arrange
            const string nome = "ACESSO_INEXISTENTE";

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.ObterPorNome(nome))
                .ReturnsAsync(0);

            // Act
            var resultado = await _sut.ObterPorNome(nome);

            // Assert
            resultado.Should().Be(0);
        }

        [Fact]
        public async Task DadoNomeComCaracteresEspeciais_QuandoObterPorNome_EntaoDeveProcessarCorretamente()
        {
            // Arrange
            const string nome = "Acesso & Restrição";
            const long idEsperado = 3;

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.ObterPorNome(nome))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.ObterPorNome(nome);

            // Assert
            resultado.Should().Be(idEsperado);
        }

        [Fact]
        public async Task DadoMultiplosChamadasComNomesDiferentes_QuandoObterPorNome_EntaoDeveRetornarCorretos()
        {
            // Arrange
            const string nome1 = "Público";
            const string nome2 = "Confidencial";
            const long id1 = 1;
            const long id2 = 2;

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.ObterPorNome(nome1))
                .ReturnsAsync(id1);

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.ObterPorNome(nome2))
                .ReturnsAsync(id2);

            // Act
            var resultado1 = await _sut.ObterPorNome(nome1);
            var resultado2 = await _sut.ObterPorNome(nome2);

            // Assert
            resultado1.Should().Be(id1);
            resultado2.Should().Be(id2);
            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Verify(r => r.ObterPorNome(nome1), Times.Once);
            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Verify(r => r.ObterPorNome(nome2), Times.Once);
        }

        #endregion

        #region Testes de Fluxo Integrado

        [Fact]
        public async Task DadoFluxoCompletoDeInsercaoEConsulta_QuandoExecutarServico_EntaoRetornaResultadoEsperado()
        {
            // Arrange
            var acessoDocumentoDTO = CriarAcessoDocumentoDTOValido(nome: "Público");
            var acessoDocumentoMapeado = CriarAcessoDocumentoValido(id: 1, nome: "Público");
            const long idInserido = 1;

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<AcessoDocumento>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(acessoDocumentoMapeado);

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.Inserir(It.IsAny<AcessoDocumento>()))
                .ReturnsAsync(idInserido);

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.ObterPorId(idInserido))
                .ReturnsAsync(acessoDocumentoMapeado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(acessoDocumentoMapeado))
                .Returns(new IdNomeExcluidoDTO
                {
                    Id = acessoDocumentoMapeado.Id,
                    Nome = acessoDocumentoMapeado.Nome,
                    Excluido = acessoDocumentoMapeado.Excluido
                });

            // Act
            var idResultado = await _sut.Inserir(acessoDocumentoDTO);
            var acessoDocumentoRecuperado = await _sut.ObterPorId(idResultado);

            // Assert
            idResultado.Should().Be(idInserido);
            acessoDocumentoRecuperado.Should().NotBeNull();
            acessoDocumentoRecuperado.Nome.Should().Be("Público");
        }

        [Fact]
        public async Task DadoFluxoCompletoDeInsercaoEPorNome_QuandoExecutarServico_EntaoRetornaResultadoEsperado()
        {
            // Arrange
            var acessoDocumentoDTO = CriarAcessoDocumentoDTOValido(nome: "Restrito");
            var acessoDocumentoMapeado = CriarAcessoDocumentoValido(id: 5, nome: "Restrito");
            const long idInserido = 5;
            const string nomeInserido = "Restrito";

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<AcessoDocumento>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(acessoDocumentoMapeado);

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.Inserir(It.IsAny<AcessoDocumento>()))
                .ReturnsAsync(idInserido);

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.ObterPorNome(nomeInserido))
                .ReturnsAsync(idInserido);

            // Act
            var idInseridoResultado = await _sut.Inserir(acessoDocumentoDTO);
            var idPorNome = await _sut.ObterPorNome(nomeInserido);

            // Assert
            idInseridoResultado.Should().Be(idInserido);
            idPorNome.Should().Be(idInserido);
        }

        #endregion

        #region Testes de Casos Extremos

        [Fact]
        public async Task DadoNomeComEspacosEmBranco_QuandoObterPorNome_EntaoDeveProcessarCorretamente()
        {
            // Arrange
            const string nome = "  Público  ";
            const long idEsperado = 1;

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.ObterPorNome(nome))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.ObterPorNome(nome);

            // Assert
            resultado.Should().Be(idEsperado);
        }

        [Fact]
        public async Task DadoNomeVazio_QuandoObterPorNome_EntaoDeveRetornarZero()
        {
            // Arrange
            const string nome = "";

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.ObterPorNome(nome))
                .ReturnsAsync(0);

            // Act
            var resultado = await _sut.ObterPorNome(nome);

            // Assert
            resultado.Should().Be(0);
        }

        [Fact]
        public async Task DadoObterPorNomeComResultadoNegativo_QuandoObterPorNome_EntaoDeveRetornarValorRetornado()
        {
            // Arrange
            const string nome = "Teste";
            const long idRetorno = -1;

            _mocker.GetMock<IRepositorioAcessoDocumento>()
                .Setup(r => r.ObterPorNome(nome))
                .ReturnsAsync(idRetorno);

            // Act
            var resultado = await _sut.ObterPorNome(nome);

            // Assert
            resultado.Should().Be(idRetorno);
        }

        #endregion

        #region Métodos Auxiliares

        private static IdNomeExcluidoDTO CriarAcessoDocumentoDTOValido(
            long id = 0,
            string nome = "Público",
            bool excluido = false)
        {
            return new IdNomeExcluidoDTO
            {
                Id = id,
                Nome = nome,
                Excluido = excluido
            };
        }

        private static AcessoDocumento CriarAcessoDocumentoValido(
            long id = 0,
            string nome = "Público",
            bool excluido = false)
        {
            return new AcessoDocumento
            {
                Id = id,
                Nome = nome,
                Excluido = excluido
            };
        }

        #endregion
    }
}
