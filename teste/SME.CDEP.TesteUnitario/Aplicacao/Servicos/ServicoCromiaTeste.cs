using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoCromiaTeste
    {
        private readonly AutoMocker _mocker;
        private readonly ServicoCromia _sut;

        public ServicoCromiaTeste()
        {
            _mocker = new AutoMocker();
            _sut = _mocker.CreateInstance<ServicoCromia>();
        }

        #region Testes de Construtor

        [Fact]
        public void DadoRepositorioNulo_QuandoConstruir_EntaoLancaArgumentNullException()
        {
            // Arrange
            var mapper = _mocker.GetMock<AutoMapper.IMapper>();

            // Act
            Action acao = () => _ = new ServicoCromia(null!, mapper.Object);

            // Assert
            acao.Should().Throw<ArgumentNullException>()
                .WithMessage("*repositorio*");
        }

        [Fact]
        public void DadoMapperNulo_QuandoConstruir_EntaoLancaArgumentNullException()
        {
            // Arrange
            var repositorio = _mocker.GetMock<IRepositorioCromia>();

            // Act
            Action acao = () => _ = new ServicoCromia(repositorio.Object, null!);

            // Assert
            acao.Should().Throw<ArgumentNullException>()
                .WithMessage("*mapper*");
        }

        [Fact]
        public void DadoRepositorioEMapperValidos_QuandoConstruir_EntaoInstanciaComSucesso()
        {
            // Act
            var servico = _mocker.CreateInstance<ServicoCromia>();

            // Assert
            servico.Should().NotBeNull();
            servico.Should().BeOfType<ServicoCromia>();
        }

        #endregion

        #region Testes de Inserir (Herança ServicoAplicacao)

        [Fact]
        public async Task DadoCromiaDTOValido_QuandoInserir_EntaoDeveRetornarIdMaiorQueZero()
        {
            // Arrange
            var cromiaDTODTO = CriarCromiaDTOValido();
            const long idEsperado = 10;

            var cromiaMapeada = CriarCromiaValida(id: idEsperado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Cromia>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(cromiaMapeada);

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.Inserir(It.IsAny<Cromia>()))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.Inserir(cromiaDTODTO);

            // Assert
            resultado.Should().Be(idEsperado);
            resultado.Should().BeGreaterThan(0);
            _mocker.GetMock<IRepositorioCromia>()
                .Verify(r => r.Inserir(It.IsAny<Cromia>()), Times.Once);
        }

        [Fact]
        public async Task DadoCromiaDTOParaInserir_QuandoInserir_EntaoDeveMapearDTOParaEntidadeCorretamente()
        {
            // Arrange
            var cromiaDTODTO = CriarCromiaDTOValido();
            var cromiaMapeada = CriarCromiaValida();

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Cromia>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(cromiaMapeada);

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.Inserir(It.IsAny<Cromia>()))
                .ReturnsAsync(1);

            // Act
            await _sut.Inserir(cromiaDTODTO);

            // Assert
            _mocker.GetMock<AutoMapper.IMapper>()
                .Verify(m => m.Map<Cromia>(It.IsAny<IdNomeExcluidoDTO>()), Times.Once);
        }

        [Fact]
        public async Task DadoCromiaDTOValido_QuandoInserir_EntaoDeveInteragirComRepositorio()
        {
            // Arrange
            var cromiaDTODTO = CriarCromiaDTOValido();
            var cromiaMapeada = CriarCromiaValida();

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Cromia>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(cromiaMapeada);

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.Inserir(It.IsAny<Cromia>()))
                .ReturnsAsync(5);

            // Act
            await _sut.Inserir(cromiaDTODTO);

            // Assert
            _mocker.GetMock<IRepositorioCromia>()
                .Verify(r => r.Inserir(cromiaMapeada), Times.Once);
        }

        #endregion

        #region Testes de ObterTodos (Herança ServicoAplicacao)

        [Fact]
        public async Task DadoRepositorioComCromias_QuandoObterTodos_EntaoDeveRetornarListaDeCromiaDTOs()
        {
            // Arrange
            var cromias = new List<Cromia>
            {
                CriarCromiaValida(id: 1, nome: "Colorida", excluido: false),
                CriarCromiaValida(id: 2, nome: "Preta e Branca", excluido: false),
                CriarCromiaValida(id: 3, nome: "Sepia", excluido: false)
            };

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(cromias);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(It.IsAny<Cromia>()))
                .Returns<Cromia>(c => new IdNomeExcluidoDTO
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Excluido = c.Excluido
                });

            // Act
            var resultado = await _sut.ObterTodos();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(3);
            resultado.Should().AllSatisfy(c => c.Should().BeOfType<IdNomeExcluidoDTO>());
        }

        [Fact]
        public async Task DadoRepositorioComCromiasAtivasEExcluidas_QuandoObterTodos_EntaoDeveRetornarApenasAtivas()
        {
            // Arrange
            var cromias = new List<Cromia>
            {
                CriarCromiaValida(id: 1, nome: "Colorida", excluido: false),
                CriarCromiaValida(id: 2, nome: "Preta e Branca", excluido: true),
                CriarCromiaValida(id: 3, nome: "Sepia", excluido: false),
                CriarCromiaValida(id: 4, nome: "Infravermelha", excluido: true)
            };

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(cromias);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(It.IsAny<Cromia>()))
                .Returns<Cromia>(c => new IdNomeExcluidoDTO
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Excluido = c.Excluido
                });

            // Act
            var resultado = await _sut.ObterTodos();

            // Assert
            resultado.Should().HaveCount(2);
            resultado.Should().AllSatisfy(c => c.Excluido.Should().BeFalse());
        }

        [Fact]
        public async Task DadoRepositorioSemCromias_QuandoObterTodos_EntaoDeveRetornarListaVazia()
        {
            // Arrange
            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync([]);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(It.IsAny<Cromia>()))
                .Returns<Cromia>(c => new IdNomeExcluidoDTO
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Excluido = c.Excluido
                });

            // Act
            var resultado = await _sut.ObterTodos();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
        }

        #endregion

        #region Testes de ObterPorId (Herança ServicoAplicacao)

        [Fact]
        public async Task DadoCromiaExistenteNaoExcluida_QuandoObterPorId_EntaoDeveRetornarCromiaDTO()
        {
            // Arrange
            var cromia = CriarCromiaValida(id: 5, nome: "Colorida", excluido: false);
            var cromiaDTO = new IdNomeExcluidoDTO
            {
                Id = cromia.Id,
                Nome = cromia.Nome,
                Excluido = cromia.Excluido
            };

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.ObterPorId(5))
                .ReturnsAsync(cromia);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(cromia))
                .Returns(cromiaDTO);

            // Act
            var resultado = await _sut.ObterPorId(5);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<IdNomeExcluidoDTO>();
            resultado.Id.Should().Be(5);
            resultado.Nome.Should().Be("Colorida");
        }

        [Fact]
        public async Task DadoCromiaExcluida_QuandoObterPorId_EntaoDeveRetornarNull()
        {
            // Arrange
            var cromia = CriarCromiaValida(id: 5, excluido: true);

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.ObterPorId(5))
                .ReturnsAsync(cromia);

            // Act
            var resultado = await _sut.ObterPorId(5);

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DadoIdInexistente_QuandoObterPorId_EntaoDeveRetornarNull()
        {
            // Arrange
            var cromiaNula = (Cromia)null!;

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync(cromiaNula);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(null))
                .Returns((IdNomeExcluidoDTO)null!);

            // Act
            var resultado = await _sut.ObterPorId(999);

            // Assert
            resultado.Should().BeNull();
        }

        #endregion

        #region Testes de Alterar (Herança ServicoAplicacao)

        [Fact]
        public async Task DadoCromiaDTOExistente_QuandoAlterar_EntaoDeveRetornarCromiaDTOAlterada()
        {
            // Arrange
            var cromiaDTODTO = CriarCromiaDTOValido(id: 3, nome: "Sepia");
            var cromiaAlterada = CriarCromiaValida(id: 3, nome: "Sepia");
            var cromiaDTOAlterada = new IdNomeExcluidoDTO
            {
                Id = cromiaAlterada.Id,
                Nome = cromiaAlterada.Nome,
                Excluido = cromiaAlterada.Excluido
            };

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.Atualizar(It.IsAny<Cromia>()))
                .ReturnsAsync(cromiaAlterada);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Cromia>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(cromiaAlterada);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(cromiaAlterada))
                .Returns(cromiaDTOAlterada);

            // Act
            var resultado = await _sut.Alterar(cromiaDTODTO);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<IdNomeExcluidoDTO>();
            resultado.Id.Should().Be(3);
            resultado.Nome.Should().Be("Sepia");
        }

        [Fact]
        public async Task DadoCromiaDTOParaAlterar_QuandoAlterar_EntaoDeveInteragirComRepositorio()
        {
            // Arrange
            var cromiaDTODTO = CriarCromiaDTOValido(id: 3);
            var cromiaMapeada = CriarCromiaValida(id: 3);
            var cromiaDTORetorno = new IdNomeExcluidoDTO
            {
                Id = 3,
                Nome = "Preta e Branca",
                Excluido = false
            };

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Cromia>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(cromiaMapeada);

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.Atualizar(It.IsAny<Cromia>()))
                .ReturnsAsync(cromiaMapeada);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(cromiaMapeada))
                .Returns(cromiaDTORetorno);

            // Act
            await _sut.Alterar(cromiaDTODTO);

            // Assert
            _mocker.GetMock<IRepositorioCromia>()
                .Verify(r => r.Atualizar(It.IsAny<Cromia>()), Times.Once);
        }

        #endregion

        #region Testes de Excluir (Herança ServicoAplicacao)

        [Fact]
        public async Task DadoCromiaExistente_QuandoExcluir_EntaoDeveRetornarVerdadeiro()
        {
            // Arrange
            var cromia = CriarCromiaValida(id: 7, excluido: false);
            var cromiaExcluida = CriarCromiaValida(id: 7, excluido: true);
            var cromiaDTO = new IdNomeExcluidoDTO
            {
                Id = cromia.Id,
                Nome = cromia.Nome,
                Excluido = cromia.Excluido
            };

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.ObterPorId(7))
                .ReturnsAsync(cromia);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(cromia))
                .Returns(cromiaDTO);

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.Atualizar(It.IsAny<Cromia>()))
                .ReturnsAsync(cromiaExcluida);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Cromia>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(cromiaExcluida);

            // Act
            var resultado = await _sut.Excluir(7);

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact]
        public async Task DadoCromiaParaExcluir_QuandoExcluir_EntaoDeveMarcarComoExcluida()
        {
            // Arrange
            var cromia = CriarCromiaValida(id: 7, excluido: false);
            var cromiaExcluida = CriarCromiaValida(id: 7, excluido: true);
            var cromiaDTO = new IdNomeExcluidoDTO
            {
                Id = cromia.Id,
                Nome = cromia.Nome,
                Excluido = cromia.Excluido
            };

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.ObterPorId(7))
                .ReturnsAsync(cromia);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(cromia))
                .Returns(cromiaDTO);

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.Atualizar(It.IsAny<Cromia>()))
                .ReturnsAsync(cromiaExcluida);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Cromia>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(cromiaExcluida);

            // Act
            await _sut.Excluir(7);

            // Assert
            _mocker.GetMock<IRepositorioCromia>()
                .Verify(r => r.Atualizar(It.Is<Cromia>(c =>
                    c.Excluido
                )), Times.Once);
        }

        #endregion

        #region Testes de ObterPorNome (Método Específico)

        [Fact]
        public async Task DadoNomeValido_QuandoObterPorNome_EntaoDeveRetornarId()
        {
            // Arrange
            const string nome = "Colorida";
            const long idEsperado = 5;

            _mocker.GetMock<IRepositorioCromia>()
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
            const string nome = "Preta e Branca";

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.ObterPorNome(nome))
                .ReturnsAsync(10);

            // Act
            await _sut.ObterPorNome(nome);

            // Assert
            _mocker.GetMock<IRepositorioCromia>()
                .Verify(r => r.ObterPorNome(nome), Times.Once);
        }

        [Fact]
        public async Task DadoNomeNaoExistente_QuandoObterPorNome_EntaoDeveRetornarZero()
        {
            // Arrange
            const string nome = "CROMIA_INEXISTENTE";

            _mocker.GetMock<IRepositorioCromia>()
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
            const string nome = "Preta & Branca";
            const long idEsperado = 3;

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.ObterPorNome(nome))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.ObterPorNome(nome);

            // Assert
            resultado.Should().Be(idEsperado);
        }

        [Fact]
        public async Task DadoNomeComAcentuacao_QuandoObterPorNome_EntaoDeveRetornarIdCorreto()
        {
            // Arrange
            const string nome = "Sépia";
            const long idEsperado = 2;

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.ObterPorNome(nome))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.ObterPorNome(nome);

            // Assert
            resultado.Should().Be(idEsperado);
            _mocker.GetMock<IRepositorioCromia>()
                .Verify(r => r.ObterPorNome(nome), Times.Once);
        }

        [Fact]
        public async Task DadoMultiplosNomes_QuandoObterPorNomeVarias_EntaoDeveRetornarIdsCorretos()
        {
            // Arrange
            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.ObterPorNome("Colorida"))
                .ReturnsAsync(1);

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.ObterPorNome("Preta e Branca"))
                .ReturnsAsync(2);

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.ObterPorNome("Sepia"))
                .ReturnsAsync(3);

            // Act
            var resultado1 = await _sut.ObterPorNome("Colorida");
            var resultado2 = await _sut.ObterPorNome("Preta e Branca");
            var resultado3 = await _sut.ObterPorNome("Sepia");

            // Assert
            resultado1.Should().Be(1);
            resultado2.Should().Be(2);
            resultado3.Should().Be(3);
        }

        #endregion

        #region Testes de Fluxo Integrado

        [Fact]
        public async Task DadoFluxoCompletoDeInsercaoEConsulta_QuandoExecutarServico_EntaoRetornaResultadoEsperado()
        {
            // Arrange
            var cromiaDTODTO = CriarCromiaDTOValido(nome: "Colorida");
            var cromiaMapeada = CriarCromiaValida(id: 1, nome: "Colorida");
            const long idInserido = 1;

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Cromia>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(cromiaMapeada);

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.Inserir(It.IsAny<Cromia>()))
                .ReturnsAsync(idInserido);

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.ObterPorId(idInserido))
                .ReturnsAsync(cromiaMapeada);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(cromiaMapeada))
                .Returns(new IdNomeExcluidoDTO
                {
                    Id = cromiaMapeada.Id,
                    Nome = cromiaMapeada.Nome,
                    Excluido = cromiaMapeada.Excluido
                });

            // Act
            var idResultado = await _sut.Inserir(cromiaDTODTO);
            var cromiaRecuperada = await _sut.ObterPorId(idResultado);

            // Assert
            idResultado.Should().Be(idInserido);
            cromiaRecuperada.Should().NotBeNull();
            cromiaRecuperada.Nome.Should().Be("Colorida");
        }

        [Fact]
        public async Task DadoFluxoCompletoDeInsercaoAlteracaoEExclusao_QuandoExecutarServico_EntaoRetornaResultadoEsperado()
        {
            // Arrange
            var cromiaDTODTO = CriarCromiaDTOValido(nome: "Preta e Branca");
            var cromiaMapeada = CriarCromiaValida(id: 1, nome: "Preta e Branca");
            var cromiaExcluida = CriarCromiaValida(id: 1, nome: "Sepia", excluido: true);

            const long idInserido = 1;

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Cromia>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(cromiaMapeada);

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.Inserir(It.IsAny<Cromia>()))
                .ReturnsAsync(idInserido);

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.ObterPorId(idInserido))
                .ReturnsAsync(cromiaMapeada);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(It.IsAny<Cromia>()))
                .Returns<Cromia>(c => new IdNomeExcluidoDTO
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Excluido = c.Excluido
                });

            _mocker.GetMock<IRepositorioCromia>()
                .Setup(r => r.Atualizar(It.IsAny<Cromia>()))
                .ReturnsAsync(cromiaExcluida);

            // Act
            var idResultado = await _sut.Inserir(cromiaDTODTO);
            var cromiaRecuperada = await _sut.ObterPorId(idResultado);
            var resultadoExclusao = await _sut.Excluir(idResultado);

            // Assert
            idResultado.Should().Be(idInserido);
            cromiaRecuperada.Should().NotBeNull();
            resultadoExclusao.Should().BeTrue();
        }

        #endregion

        #region Testes de Implementação de Interface

        [Fact]
        public void DadoServicoCromia_QuandoVerificarTipo_EntaoDeveImplementarIServicoCromia()
        {
            // Assert
            _sut.Should().BeAssignableTo<IServicoCromia>();
        }

        [Fact]
        public void DadoServicoCromia_QuandoVerificarTipo_EntaoDeveSerHerdadoDe()
        {
            // Assert
            _sut.Should().BeAssignableTo<ServicoAplicacao<Cromia, IdNomeExcluidoDTO>>();
        }

        #endregion

        #region Métodos Auxiliares

        private static IdNomeExcluidoDTO CriarCromiaDTOValido(
            long id = 0,
            string nome = "Colorida",
            bool excluido = false)
        {
            return new IdNomeExcluidoDTO
            {
                Id = id,
                Nome = nome,
                Excluido = excluido
            };
        }

        private static Cromia CriarCromiaValida(
            long id = 0,
            string nome = "Colorida",
            bool excluido = false)
        {
            return new Cromia
            {
                Id = id,
                Nome = nome,
                Excluido = excluido
            };
        }

        #endregion
    }
}
