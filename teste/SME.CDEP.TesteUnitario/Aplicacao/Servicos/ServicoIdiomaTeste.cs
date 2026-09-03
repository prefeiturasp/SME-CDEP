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
    public class ServicoIdiomaTeste
    {
        private readonly AutoMocker _mocker;
        private readonly ServicoIdioma _sut;

        public ServicoIdiomaTeste()
        {
            _mocker = new AutoMocker();
            _sut = _mocker.CreateInstance<ServicoIdioma>();
        }

        #region Testes de Construtor

        [Fact]
        public void DadoRepositorioNulo_QuandoConstruir_EntaoLancaArgumentNullException()
        {
            // Arrange
            var mapper = _mocker.GetMock<AutoMapper.IMapper>();

            // Act
            Action acao = () => _ = new ServicoIdioma(null!, mapper.Object);

            // Assert
            acao.Should().Throw<ArgumentNullException>()
                .WithMessage("*repositorio*");
        }

        [Fact]
        public void DadoMapperNulo_QuandoConstruir_EntaoLancaArgumentNullException()
        {
            // Arrange
            var repositorio = _mocker.GetMock<IRepositorioIdioma>();

            // Act
            Action acao = () => _ = new ServicoIdioma(repositorio.Object, null!);

            // Assert
            acao.Should().Throw<ArgumentNullException>()
                .WithMessage("*mapper*");
        }

        [Fact]
        public void DadoRepositorioEMapperValidos_QuandoConstruir_EntaoInstanciaComSucesso()
        {
            // Act
            var servico = _mocker.CreateInstance<ServicoIdioma>();

            // Assert
            servico.Should().NotBeNull();
            servico.Should().BeOfType<ServicoIdioma>();
        }

        #endregion

        #region Testes de Inserir (Herança ServicoAplicacao)

        [Fact]
        public async Task DadoIdiomaDTOValido_QuandoInserir_EntaoDeveRetornarIdMaiorQueZero()
        {
            // Arrange
            var idiomaDTO = CriarIdiomaDTOValido();
            const long idEsperado = 10;

            var idiomaMapeado = CriarIdiomaValido(id: idEsperado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Idioma>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(idiomaMapeado);

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.Inserir(It.IsAny<Idioma>()))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.Inserir(idiomaDTO);

            // Assert
            resultado.Should().Be(idEsperado);
            resultado.Should().BeGreaterThan(0);
            _mocker.GetMock<IRepositorioIdioma>()
                .Verify(r => r.Inserir(It.IsAny<Idioma>()), Times.Once);
        }

        [Fact]
        public async Task DadoIdiomaDTOParaInserir_QuandoInserir_EntaoDeveMapearDTOParaEntidadeCorretamente()
        {
            // Arrange
            var idiomaDTO = CriarIdiomaDTOValido();
            var idiomaMapeado = CriarIdiomaValido();

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Idioma>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(idiomaMapeado);

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.Inserir(It.IsAny<Idioma>()))
                .ReturnsAsync(1);

            // Act
            await _sut.Inserir(idiomaDTO);

            // Assert
            _mocker.GetMock<AutoMapper.IMapper>()
                .Verify(m => m.Map<Idioma>(It.IsAny<IdNomeExcluidoDTO>()), Times.Once);
        }

        [Fact]
        public async Task DadoIdiomaDTOValido_QuandoInserir_EntaoDeveInteragirComRepositorio()
        {
            // Arrange
            var idiomaDTO = CriarIdiomaDTOValido();
            var idiomaMapeado = CriarIdiomaValido();

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Idioma>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(idiomaMapeado);

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.Inserir(It.IsAny<Idioma>()))
                .ReturnsAsync(5);

            // Act
            await _sut.Inserir(idiomaDTO);

            // Assert
            _mocker.GetMock<IRepositorioIdioma>()
                .Verify(r => r.Inserir(idiomaMapeado), Times.Once);
        }

        #endregion

        #region Testes de ObterTodos (Herança ServicoAplicacao)

        [Fact]
        public async Task DadoRepositorioComIdiomas_QuandoObterTodos_EntaoDeveRetornarListaDeIdiomaDTOs()
        {
            // Arrange
            var idiomas = new List<Idioma>
            {
                CriarIdiomaValido(id: 1, nome: "Português", excluido: false),
                CriarIdiomaValido(id: 2, nome: "Inglês", excluido: false),
                CriarIdiomaValido(id: 3, nome: "Espanhol", excluido: false)
            };

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(idiomas);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(It.IsAny<Idioma>()))
                .Returns<Idioma>(i => new IdNomeExcluidoDTO
                {
                    Id = i.Id,
                    Nome = i.Nome,
                    Excluido = i.Excluido
                });

            // Act
            var resultado = await _sut.ObterTodos();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(3);
            resultado.Should().AllSatisfy(i => i.Should().BeOfType<IdNomeExcluidoDTO>());
        }

        [Fact]
        public async Task DadoRepositorioComIdiomasAtivosEExcluidos_QuandoObterTodos_EntaoDeveRetornarApenasAtivos()
        {
            // Arrange
            var idiomas = new List<Idioma>
            {
                CriarIdiomaValido(id: 1, nome: "Português", excluido: false),
                CriarIdiomaValido(id: 2, nome: "Grego", excluido: true),
                CriarIdiomaValido(id: 3, nome: "Inglês", excluido: false),
                CriarIdiomaValido(id: 4, nome: "Latim", excluido: true)
            };

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(idiomas);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(It.IsAny<Idioma>()))
                .Returns<Idioma>(i => new IdNomeExcluidoDTO
                {
                    Id = i.Id,
                    Nome = i.Nome,
                    Excluido = i.Excluido
                });

            // Act
            var resultado = await _sut.ObterTodos();

            // Assert
            resultado.Should().HaveCount(2);
            resultado.Should().AllSatisfy(i => i.Excluido.Should().BeFalse());
        }

        [Fact]
        public async Task DadoRepositorioSemIdiomas_QuandoObterTodos_EntaoDeveRetornarListaVazia()
        {
            // Arrange
            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync([]);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(It.IsAny<Idioma>()))
                .Returns<Idioma>(i => new IdNomeExcluidoDTO
                {
                    Id = i.Id,
                    Nome = i.Nome,
                    Excluido = i.Excluido
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
        public async Task DadoIdiomaExistenteNaoExcluido_QuandoObterPorId_EntaoDeveRetornarIdiomaDTO()
        {
            // Arrange
            var idioma = CriarIdiomaValido(id: 5, nome: "Português", excluido: false);
            var idiomaDTO = new IdNomeExcluidoDTO
            {
                Id = idioma.Id,
                Nome = idioma.Nome,
                Excluido = idioma.Excluido
            };

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.ObterPorId(5))
                .ReturnsAsync(idioma);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(idioma))
                .Returns(idiomaDTO);

            // Act
            var resultado = await _sut.ObterPorId(5);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<IdNomeExcluidoDTO>();
            resultado.Id.Should().Be(5);
            resultado.Nome.Should().Be("Português");
        }

        [Fact]
        public async Task DadoIdiomaExcluido_QuandoObterPorId_EntaoDeveRetornarNull()
        {
            // Arrange
            var idioma = CriarIdiomaValido(id: 5, excluido: true);

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.ObterPorId(5))
                .ReturnsAsync(idioma);

            // Act
            var resultado = await _sut.ObterPorId(5);

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DadoIdInexistente_QuandoObterPorId_EntaoDeveRetornarNull()
        {
            // Arrange
            var idiomaNulo = (Idioma)null!;

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync(idiomaNulo);

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
        public async Task DadoIdiomaDTOExistente_QuandoAlterar_EntaoDeveRetornarIdiomaDTOAlterado()
        {
            // Arrange
            var idiomaDTO = CriarIdiomaDTOValido(id: 3, nome: "Francês");
            var idiomaAlterado = CriarIdiomaValido(id: 3, nome: "Francês");
            var idiomaDTOAlterado = new IdNomeExcluidoDTO
            {
                Id = idiomaAlterado.Id,
                Nome = idiomaAlterado.Nome,
                Excluido = idiomaAlterado.Excluido
            };

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.Atualizar(It.IsAny<Idioma>()))
                .ReturnsAsync(idiomaAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Idioma>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(idiomaAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(idiomaAlterado))
                .Returns(idiomaDTOAlterado);

            // Act
            var resultado = await _sut.Alterar(idiomaDTO);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<IdNomeExcluidoDTO>();
            resultado.Id.Should().Be(3);
            resultado.Nome.Should().Be("Francês");
        }

        [Fact]
        public async Task DadoIdiomaDTOParaAlterar_QuandoAlterar_EntaoDeveInteragirComRepositorio()
        {
            // Arrange
            var idiomaDTO = CriarIdiomaDTOValido(id: 3);
            var idiomaMapeado = CriarIdiomaValido(id: 3);
            var idiomaDTORetorno = new IdNomeExcluidoDTO
            {
                Id = 3,
                Nome = "Inglês",
                Excluido = false
            };

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Idioma>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(idiomaMapeado);

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.Atualizar(It.IsAny<Idioma>()))
                .ReturnsAsync(idiomaMapeado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(idiomaMapeado))
                .Returns(idiomaDTORetorno);

            // Act
            await _sut.Alterar(idiomaDTO);

            // Assert
            _mocker.GetMock<IRepositorioIdioma>()
                .Verify(r => r.Atualizar(It.IsAny<Idioma>()), Times.Once);
        }

        #endregion

        #region Testes de Excluir (Herança ServicoAplicacao)

        [Fact]
        public async Task DadoIdiomaExistente_QuandoExcluir_EntaoDeveRetornarVerdadeiro()
        {
            // Arrange
            var idioma = CriarIdiomaValido(id: 7, excluido: false);
            var idiomaExcluido = CriarIdiomaValido(id: 7, excluido: true);
            var idiomaDTO = new IdNomeExcluidoDTO
            {
                Id = idioma.Id,
                Nome = idioma.Nome,
                Excluido = idioma.Excluido
            };

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.ObterPorId(7))
                .ReturnsAsync(idioma);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(idioma))
                .Returns(idiomaDTO);

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.Atualizar(It.IsAny<Idioma>()))
                .ReturnsAsync(idiomaExcluido);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Idioma>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(idiomaExcluido);

            // Act
            var resultado = await _sut.Excluir(7);

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact]
        public async Task DadoIdiomaParaExcluir_QuandoExcluir_EntaoDeveMarcarComoExcluido()
        {
            // Arrange
            var idioma = CriarIdiomaValido(id: 7, excluido: false);
            var idiomaExcluido = CriarIdiomaValido(id: 7, excluido: true);
            var idiomaDTO = new IdNomeExcluidoDTO
            {
                Id = idioma.Id,
                Nome = idioma.Nome,
                Excluido = idioma.Excluido
            };

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.ObterPorId(7))
                .ReturnsAsync(idioma);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(idioma))
                .Returns(idiomaDTO);

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.Atualizar(It.IsAny<Idioma>()))
                .ReturnsAsync(idiomaExcluido);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Idioma>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(idiomaExcluido);

            // Act
            await _sut.Excluir(7);

            // Assert
            _mocker.GetMock<IRepositorioIdioma>()
                .Verify(r => r.Atualizar(It.Is<Idioma>(i =>
                    i.Excluido
                )), Times.Once);
        }

        #endregion

        #region Testes de ObterPorNome (Método Específico)

        [Fact]
        public async Task DadoNomeValido_QuandoObterPorNome_EntaoDeveRetornarId()
        {
            // Arrange
            const string nome = "Português";
            const long idEsperado = 5;

            _mocker.GetMock<IRepositorioIdioma>()
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
            const string nome = "Inglês";

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.ObterPorNome(nome))
                .ReturnsAsync(10);

            // Act
            await _sut.ObterPorNome(nome);

            // Assert
            _mocker.GetMock<IRepositorioIdioma>()
                .Verify(r => r.ObterPorNome(nome), Times.Once);
        }

        [Fact]
        public async Task DadoNomeNaoExistente_QuandoObterPorNome_EntaoDeveRetornarZero()
        {
            // Arrange
            const string nome = "IDIOMA_INEXISTENTE";

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.ObterPorNome(nome))
                .ReturnsAsync(0);

            // Act
            var resultado = await _sut.ObterPorNome(nome);

            // Assert
            resultado.Should().Be(0);
        }

        [Fact]
        public async Task DadoNomeComAcento_QuandoObterPorNome_EntaoDeveProcessarCorretamente()
        {
            // Arrange
            const string nome = "Português";
            const long idEsperado = 1;

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.ObterPorNome(nome))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.ObterPorNome(nome);

            // Assert
            resultado.Should().Be(idEsperado);
        }

        [Fact]
        public async Task DadoNomeComEspacos_QuandoObterPorNome_EntaoDeveRetornarIdCorreto()
        {
            // Arrange
            const string nome = "Chinês Mandarim";
            const long idEsperado = 8;

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.ObterPorNome(nome))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.ObterPorNome(nome);

            // Assert
            resultado.Should().Be(idEsperado);
            _mocker.GetMock<IRepositorioIdioma>()
                .Verify(r => r.ObterPorNome(nome), Times.Once);
        }

        [Fact]
        public async Task DadoMultiplosNomes_QuandoObterPorNomeVarias_EntaoDeveRetornarIdsCorretos()
        {
            // Arrange
            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.ObterPorNome("Português"))
                .ReturnsAsync(1);

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.ObterPorNome("Inglês"))
                .ReturnsAsync(2);

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.ObterPorNome("Espanhol"))
                .ReturnsAsync(3);

            // Act
            var resultado1 = await _sut.ObterPorNome("Português");
            var resultado2 = await _sut.ObterPorNome("Inglês");
            var resultado3 = await _sut.ObterPorNome("Espanhol");

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
            var idiomaDTO = CriarIdiomaDTOValido(nome: "Alemão");
            var idiomaMapeado = CriarIdiomaValido(id: 1, nome: "Alemão");
            const long idInserido = 1;

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Idioma>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(idiomaMapeado);

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.Inserir(It.IsAny<Idioma>()))
                .ReturnsAsync(idInserido);

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.ObterPorId(idInserido))
                .ReturnsAsync(idiomaMapeado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(idiomaMapeado))
                .Returns(new IdNomeExcluidoDTO
                {
                    Id = idiomaMapeado.Id,
                    Nome = idiomaMapeado.Nome,
                    Excluido = idiomaMapeado.Excluido
                });

            // Act
            var idResultado = await _sut.Inserir(idiomaDTO);
            var idiomaRecuperado = await _sut.ObterPorId(idResultado);

            // Assert
            idResultado.Should().Be(idInserido);
            idiomaRecuperado.Should().NotBeNull();
            idiomaRecuperado.Nome.Should().Be("Alemão");
        }

        [Fact]
        public async Task DadoFluxoCompletoDeInsercaoAlteracaoEExclusao_QuandoExecutarServico_EntaoRetornaResultadoEsperado()
        {
            // Arrange
            var idiomaDTO = CriarIdiomaDTOValido(nome: "Inglês");
            var idiomaMapeado = CriarIdiomaValido(id: 1, nome: "Inglês");
            var idiomaExcluido = CriarIdiomaValido(id: 1, nome: "Português", excluido: true);

            const long idInserido = 1;

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Idioma>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(idiomaMapeado);

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.Inserir(It.IsAny<Idioma>()))
                .ReturnsAsync(idInserido);

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.ObterPorId(idInserido))
                .ReturnsAsync(idiomaMapeado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(It.IsAny<Idioma>()))
                .Returns<Idioma>(i => new IdNomeExcluidoDTO
                {
                    Id = i.Id,
                    Nome = i.Nome,
                    Excluido = i.Excluido
                });

            _mocker.GetMock<IRepositorioIdioma>()
                .Setup(r => r.Atualizar(It.IsAny<Idioma>()))
                .ReturnsAsync(idiomaExcluido);

            // Act
            var idResultado = await _sut.Inserir(idiomaDTO);
            var idiomaRecuperado = await _sut.ObterPorId(idResultado);
            var resultadoExclusao = await _sut.Excluir(idResultado);

            // Assert
            idResultado.Should().Be(idInserido);
            idiomaRecuperado.Should().NotBeNull();
            resultadoExclusao.Should().BeTrue();
        }

        #endregion

        #region Testes de Implementação de Interface

        [Fact]
        public void DadoServicoIdioma_QuandoVerificarTipo_EntaoDeveImplementarIServicoIdioma()
        {
            // Assert
            _sut.Should().BeAssignableTo<IServicoIdioma>();
        }

        [Fact]
        public void DadoServicoIdioma_QuandoVerificarTipo_EntaoDeveSercedidoDe()
        {
            // Assert
            _sut.Should().BeAssignableTo<ServicoAplicacao<Idioma, IdNomeExcluidoDTO>>();
        }

        #endregion

        #region Métodos Auxiliares

        private static IdNomeExcluidoDTO CriarIdiomaDTOValido(
            long id = 0,
            string nome = "Português",
            bool excluido = false)
        {
            return new IdNomeExcluidoDTO
            {
                Id = id,
                Nome = nome,
                Excluido = excluido
            };
        }

        private static Idioma CriarIdiomaValido(
            long id = 0,
            string nome = "Português",
            bool excluido = false)
        {
            return new Idioma
            {
                Id = id,
                Nome = nome,
                Excluido = excluido
            };
        }

        #endregion
    }
}
