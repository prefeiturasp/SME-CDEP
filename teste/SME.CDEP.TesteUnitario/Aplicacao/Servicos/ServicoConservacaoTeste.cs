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
    public class ServicoConservacaoTeste
    {
        private readonly AutoMocker _mocker;
        private readonly ServicoConservacao _sut;

        public ServicoConservacaoTeste()
        {
            _mocker = new AutoMocker();
            _sut = _mocker.CreateInstance<ServicoConservacao>();
        }

        #region Testes de Construtor

        [Fact]
        public void DadoRepositorioNulo_QuandoConstruir_EntaoLancaArgumentNullException()
        {
            // Arrange
            var mapper = _mocker.GetMock<AutoMapper.IMapper>();

            // Act
            Action acao = () => _ = new ServicoConservacao(null!, mapper.Object);

            // Assert
            acao.Should().Throw<ArgumentNullException>()
                .WithMessage("*repositorio*");
        }

        [Fact]
        public void DadoMapperNulo_QuandoConstruir_EntaoLancaArgumentNullException()
        {
            // Arrange
            var repositorio = _mocker.GetMock<IRepositorioConservacao>();

            // Act
            Action acao = () => _ = new ServicoConservacao(repositorio.Object, null!);

            // Assert
            acao.Should().Throw<ArgumentNullException>()
                .WithMessage("*mapper*");
        }

        [Fact]
        public void DadoRepositorioEMapperValidos_QuandoConstruir_EntaoInstanciaComSucesso()
        {
            // Act
            var servico = _mocker.CreateInstance<ServicoConservacao>();

            // Assert
            servico.Should().NotBeNull();
            servico.Should().BeOfType<ServicoConservacao>();
        }

        #endregion

        #region Testes de Inserir (Herança ServicoAplicacao)

        [Fact]
        public async Task DadoConservacaoDTOValido_QuandoInserir_EntaoDeveRetornarIdMaiorQueZero()
        {
            // Arrange
            var conservacaoDTO = CriarConservacaoDTOValido();
            const long idEsperado = 10;

            var conservacaoMapeada = CriarConservacaoValida(id: idEsperado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Conservacao>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(conservacaoMapeada);

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.Inserir(It.IsAny<Conservacao>()))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.Inserir(conservacaoDTO);

            // Assert
            resultado.Should().Be(idEsperado);
            resultado.Should().BeGreaterThan(0);
            _mocker.GetMock<IRepositorioConservacao>()
                .Verify(r => r.Inserir(It.IsAny<Conservacao>()), Times.Once);
        }

        [Fact]
        public async Task DadoConservacaoDTOParaInserir_QuandoInserir_EntaoDeveMapearDTOParaEntidadeCorretamente()
        {
            // Arrange
            var conservacaoDTO = CriarConservacaoDTOValido();
            var conservacaoMapeada = CriarConservacaoValida();

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Conservacao>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(conservacaoMapeada);

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.Inserir(It.IsAny<Conservacao>()))
                .ReturnsAsync(1);

            // Act
            await _sut.Inserir(conservacaoDTO);

            // Assert
            _mocker.GetMock<AutoMapper.IMapper>()
                .Verify(m => m.Map<Conservacao>(It.IsAny<IdNomeExcluidoDTO>()), Times.Once);
        }

        [Fact]
        public async Task DadoConservacaoDTOValido_QuandoInserir_EntaoDeveInteragirComRepositorio()
        {
            // Arrange
            var conservacaoDTO = CriarConservacaoDTOValido();
            var conservacaoMapeada = CriarConservacaoValida();

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Conservacao>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(conservacaoMapeada);

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.Inserir(It.IsAny<Conservacao>()))
                .ReturnsAsync(5);

            // Act
            await _sut.Inserir(conservacaoDTO);

            // Assert
            _mocker.GetMock<IRepositorioConservacao>()
                .Verify(r => r.Inserir(conservacaoMapeada), Times.Once);
        }

        #endregion

        #region Testes de ObterTodos (Herança ServicoAplicacao)

        [Fact]
        public async Task DadoRepositorioComConservacoes_QuandoObterTodos_EntaoDeveRetornarListaDeConservacaoDTOs()
        {
            // Arrange
            var conservacoes = new List<Conservacao>
            {
                CriarConservacaoValida(id: 1, nome: "Excelente", excluido: false),
                CriarConservacaoValida(id: 2, nome: "Bom", excluido: false),
                CriarConservacaoValida(id: 3, nome: "Regular", excluido: false)
            };

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(conservacoes);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(It.IsAny<Conservacao>()))
                .Returns<Conservacao>(c => new IdNomeExcluidoDTO
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
        public async Task DadoRepositorioComConservacoeAtivasEExcluidas_QuandoObterTodos_EntaoDeveRetornarApenasAtivas()
        {
            // Arrange
            var conservacoes = new List<Conservacao>
            {
                CriarConservacaoValida(id: 1, nome: "Excelente", excluido: false),
                CriarConservacaoValida(id: 2, nome: "Péssima", excluido: true),
                CriarConservacaoValida(id: 3, nome: "Bom", excluido: false),
                CriarConservacaoValida(id: 4, nome: "Ruim", excluido: true)
            };

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(conservacoes);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(It.IsAny<Conservacao>()))
                .Returns<Conservacao>(c => new IdNomeExcluidoDTO
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
        public async Task DadoRepositorioSemConservacoes_QuandoObterTodos_EntaoDeveRetornarListaVazia()
        {
            // Arrange
            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync([]);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(It.IsAny<Conservacao>()))
                .Returns<Conservacao>(c => new IdNomeExcluidoDTO
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
        public async Task DadoConservacaoExistenteNaoExcluida_QuandoObterPorId_EntaoDeveRetornarConservacaoDTO()
        {
            // Arrange
            var conservacao = CriarConservacaoValida(id: 5, nome: "Bom", excluido: false);
            var conservacaoDTO = new IdNomeExcluidoDTO
            {
                Id = conservacao.Id,
                Nome = conservacao.Nome,
                Excluido = conservacao.Excluido
            };

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.ObterPorId(5))
                .ReturnsAsync(conservacao);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(conservacao))
                .Returns(conservacaoDTO);

            // Act
            var resultado = await _sut.ObterPorId(5);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<IdNomeExcluidoDTO>();
            resultado.Id.Should().Be(5);
            resultado.Nome.Should().Be("Bom");
        }

        [Fact]
        public async Task DadoConservacaoExcluida_QuandoObterPorId_EntaoDeveRetornarNull()
        {
            // Arrange
            var conservacao = CriarConservacaoValida(id: 5, excluido: true);

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.ObterPorId(5))
                .ReturnsAsync(conservacao);

            // Act
            var resultado = await _sut.ObterPorId(5);

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DadoIdInexistente_QuandoObterPorId_EntaoDeveRetornarNull()
        {
            // Arrange
            var conservacaoNula = (Conservacao)null!;

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync(conservacaoNula);

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
        public async Task DadoConservacaoDTOExistente_QuandoAlterar_EntaoDeveRetornarConservacaoDTOAlterada()
        {
            // Arrange
            var conservacaoDTO = CriarConservacaoDTOValido(id: 3, nome: "Péssima");
            var conservacaoAlterada = CriarConservacaoValida(id: 3, nome: "Péssima");
            var conservacaoDTOAlterada = new IdNomeExcluidoDTO
            {
                Id = conservacaoAlterada.Id,
                Nome = conservacaoAlterada.Nome,
                Excluido = conservacaoAlterada.Excluido
            };

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.Atualizar(It.IsAny<Conservacao>()))
                .ReturnsAsync(conservacaoAlterada);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Conservacao>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(conservacaoAlterada);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(conservacaoAlterada))
                .Returns(conservacaoDTOAlterada);

            // Act
            var resultado = await _sut.Alterar(conservacaoDTO);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<IdNomeExcluidoDTO>();
            resultado.Id.Should().Be(3);
            resultado.Nome.Should().Be("Péssima");
        }

        [Fact]
        public async Task DadoConservacaoDTOParaAlterar_QuandoAlterar_EntaoDeveInteragirComRepositorio()
        {
            // Arrange
            var conservacaoDTO = CriarConservacaoDTOValido(id: 3);
            var conservacaoMapeada = CriarConservacaoValida(id: 3);
            var conservacaoDTORetorno = new IdNomeExcluidoDTO
            {
                Id = 3,
                Nome = "Excelente",
                Excluido = false
            };

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Conservacao>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(conservacaoMapeada);

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.Atualizar(It.IsAny<Conservacao>()))
                .ReturnsAsync(conservacaoMapeada);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(conservacaoMapeada))
                .Returns(conservacaoDTORetorno);

            // Act
            await _sut.Alterar(conservacaoDTO);

            // Assert
            _mocker.GetMock<IRepositorioConservacao>()
                .Verify(r => r.Atualizar(It.IsAny<Conservacao>()), Times.Once);
        }

        #endregion

        #region Testes de Excluir (Herança ServicoAplicacao)

        [Fact]
        public async Task DadoConservacaoExistente_QuandoExcluir_EntaoDeveRetornarVerdadeiro()
        {
            // Arrange
            var conservacao = CriarConservacaoValida(id: 7, excluido: false);
            var conservacaoExcluida = CriarConservacaoValida(id: 7, excluido: true);
            var conservacaoDTO = new IdNomeExcluidoDTO
            {
                Id = conservacao.Id,
                Nome = conservacao.Nome,
                Excluido = conservacao.Excluido
            };

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.ObterPorId(7))
                .ReturnsAsync(conservacao);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(conservacao))
                .Returns(conservacaoDTO);

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.Atualizar(It.IsAny<Conservacao>()))
                .ReturnsAsync(conservacaoExcluida);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Conservacao>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(conservacaoExcluida);

            // Act
            var resultado = await _sut.Excluir(7);

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact]
        public async Task DadoConservacaoParaExcluir_QuandoExcluir_EntaoDeveMarcarComoExcluida()
        {
            // Arrange
            var conservacao = CriarConservacaoValida(id: 7, excluido: false);
            var conservacaoExcluida = CriarConservacaoValida(id: 7, excluido: true);
            var conservacaoDTO = new IdNomeExcluidoDTO
            {
                Id = conservacao.Id,
                Nome = conservacao.Nome,
                Excluido = conservacao.Excluido
            };

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.ObterPorId(7))
                .ReturnsAsync(conservacao);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(conservacao))
                .Returns(conservacaoDTO);

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.Atualizar(It.IsAny<Conservacao>()))
                .ReturnsAsync(conservacaoExcluida);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Conservacao>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(conservacaoExcluida);

            // Act
            await _sut.Excluir(7);

            // Assert
            _mocker.GetMock<IRepositorioConservacao>()
                .Verify(r => r.Atualizar(It.Is<Conservacao>(c =>
                    c.Excluido
                )), Times.Once);
        }

        #endregion

        #region Testes de ObterPorNome (Método Específico)

        [Fact]
        public async Task DadoNomeValido_QuandoObterPorNome_EntaoDeveRetornarId()
        {
            // Arrange
            const string nome = "Excelente";
            const long idEsperado = 5;

            _mocker.GetMock<IRepositorioConservacao>()
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
            const string nome = "Bom";

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.ObterPorNome(nome))
                .ReturnsAsync(10);

            // Act
            await _sut.ObterPorNome(nome);

            // Assert
            _mocker.GetMock<IRepositorioConservacao>()
                .Verify(r => r.ObterPorNome(nome), Times.Once);
        }

        [Fact]
        public async Task DadoNomeNaoExistente_QuandoObterPorNome_EntaoDeveRetornarZero()
        {
            // Arrange
            const string nome = "CONSERVACAO_INEXISTENTE";

            _mocker.GetMock<IRepositorioConservacao>()
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
            const string nome = "Muito Bom (Excelente)";
            const long idEsperado = 3;

            _mocker.GetMock<IRepositorioConservacao>()
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
            const string nome = "Ótima";
            const long idEsperado = 2;

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.ObterPorNome(nome))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.ObterPorNome(nome);

            // Assert
            resultado.Should().Be(idEsperado);
            _mocker.GetMock<IRepositorioConservacao>()
                .Verify(r => r.ObterPorNome(nome), Times.Once);
        }

        [Fact]
        public async Task DadoMultiplosNomes_QuandoObterPorNomeVarias_EntaoDeveRetornarIdsCorretos()
        {
            // Arrange
            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.ObterPorNome("Excelente"))
                .ReturnsAsync(1);

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.ObterPorNome("Bom"))
                .ReturnsAsync(2);

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.ObterPorNome("Ruim"))
                .ReturnsAsync(3);

            // Act
            var resultado1 = await _sut.ObterPorNome("Excelente");
            var resultado2 = await _sut.ObterPorNome("Bom");
            var resultado3 = await _sut.ObterPorNome("Ruim");

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
            var conservacaoDTO = CriarConservacaoDTOValido(nome: "Excelente");
            var conservacaoMapeada = CriarConservacaoValida(id: 1, nome: "Excelente");
            const long idInserido = 1;

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Conservacao>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(conservacaoMapeada);

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.Inserir(It.IsAny<Conservacao>()))
                .ReturnsAsync(idInserido);

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.ObterPorId(idInserido))
                .ReturnsAsync(conservacaoMapeada);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(conservacaoMapeada))
                .Returns(new IdNomeExcluidoDTO
                {
                    Id = conservacaoMapeada.Id,
                    Nome = conservacaoMapeada.Nome,
                    Excluido = conservacaoMapeada.Excluido
                });

            // Act
            var idResultado = await _sut.Inserir(conservacaoDTO);
            var conservacaoRecuperada = await _sut.ObterPorId(idResultado);

            // Assert
            idResultado.Should().Be(idInserido);
            conservacaoRecuperada.Should().NotBeNull();
            conservacaoRecuperada.Nome.Should().Be("Excelente");
        }

        [Fact]
        public async Task DadoFluxoCompletoDeInsercaoAltEracaoEExclusao_QuandoExecutarServico_EntaoRetornaResultadoEsperado()
        {
            // Arrange
            var conservacaoDTO = CriarConservacaoDTOValido(nome: "Bom");
            var conservacaoMapeada = CriarConservacaoValida(id: 1, nome: "Bom");
            var conservacaoExcluida = CriarConservacaoValida(id: 1, nome: "Excelente", excluido: true);

            const long idInserido = 1;

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Conservacao>(It.IsAny<IdNomeExcluidoDTO>()))
                .Returns(conservacaoMapeada);

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.Inserir(It.IsAny<Conservacao>()))
                .ReturnsAsync(idInserido);

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.ObterPorId(idInserido))
                .ReturnsAsync(conservacaoMapeada);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeExcluidoDTO>(It.IsAny<Conservacao>()))
                .Returns<Conservacao>(c => new IdNomeExcluidoDTO
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Excluido = c.Excluido
                });

            _mocker.GetMock<IRepositorioConservacao>()
                .Setup(r => r.Atualizar(It.IsAny<Conservacao>()))
                .ReturnsAsync(conservacaoExcluida);

            // Act
            var idResultado = await _sut.Inserir(conservacaoDTO);
            var conservacaoRecuperada = await _sut.ObterPorId(idResultado);
            var resultadoExclusao = await _sut.Excluir(idResultado);

            // Assert
            idResultado.Should().Be(idInserido);
            conservacaoRecuperada.Should().NotBeNull();
            resultadoExclusao.Should().BeTrue();
        }

        #endregion

        #region Testes de Implementação de Interface

        [Fact]
        public void DadoServicoConservacao_QuandoVerificarTipo_EntaoDeveImplementarIServicoConservacao()
        {
            // Assert
            _sut.Should().BeAssignableTo<IServicoConservacao>();
        }

        [Fact]
        public void DadoServicoConservacao_QuandoVerificarTipo_EntaoDeveSerucedidoDe()
        {
            // Assert
            _sut.Should().BeAssignableTo<ServicoAplicacao<Conservacao, IdNomeExcluidoDTO>>();
        }

        #endregion

        #region Métodos Auxiliares

        private static IdNomeExcluidoDTO CriarConservacaoDTOValido(
            long id = 0,
            string nome = "Excelente",
            bool excluido = false)
        {
            return new IdNomeExcluidoDTO
            {
                Id = id,
                Nome = nome,
                Excluido = excluido
            };
        }

        private static Conservacao CriarConservacaoValida(
            long id = 0,
            string nome = "Excelente",
            bool excluido = false)
        {
            return new Conservacao
            {
                Id = id,
                Nome = nome,
                Excluido = excluido
            };
        }

        #endregion
    }
}
