using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoSuporteTeste
    {
        private readonly AutoMocker _mocker;
        private readonly ServicoSuporte _sut;

        public ServicoSuporteTeste()
        {
            _mocker = new AutoMocker();
            _sut = _mocker.CreateInstance<ServicoSuporte>();
        }

        #region Testes de Construtor

        [Fact]
        public void DadoRepositorioNulo_QuandoConstruir_EntaoLancaArgumentNullException()
        {
            // Arrange
            var mapper = _mocker.GetMock<AutoMapper.IMapper>();

            // Act
            Action acao = () => _ = new ServicoSuporte(null!, mapper.Object);

            // Assert
            acao.Should().Throw<ArgumentNullException>()
                .WithMessage("*repositorio*");
        }

        [Fact]
        public void DadoMapperNulo_QuandoConstruir_EntaoLancaArgumentNullException()
        {
            // Arrange
            var repositorio = _mocker.GetMock<IRepositorioSuporte>();

            // Act
            Action acao = () => _ = new ServicoSuporte(repositorio.Object, null!);

            // Assert
            acao.Should().Throw<ArgumentNullException>()
                .WithMessage("*mapper*");
        }

        [Fact]
        public void DadoRepositorioEMapperValidos_QuandoConstruir_EntaoInstanciaComSucesso()
        {
            // Act
            var servico = _mocker.CreateInstance<ServicoSuporte>();

            // Assert
            servico.Should().NotBeNull();
            servico.Should().BeOfType<ServicoSuporte>();
        }

        #endregion

        #region Testes de Inserir (Herança ServicoAplicacao)

        [Fact]
        public async Task DadoSuporteDTOValido_QuandoInserir_EntaoDeveRetornarIdMaiorQueZero()
        {
            // Arrange
            var suporteDTO = CriarSuporteDTOValido();
            const long idEsperado = 10;

            var suporteMapeado = CriarSuporteValido(id: idEsperado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Suporte>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(suporteMapeado);

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.Inserir(It.IsAny<Suporte>()))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.Inserir(suporteDTO);

            // Assert
            resultado.Should().Be(idEsperado);
            resultado.Should().BeGreaterThan(0);
            _mocker.GetMock<IRepositorioSuporte>()
                .Verify(r => r.Inserir(It.IsAny<Suporte>()), Times.Once);
        }

        [Fact]
        public async Task DadoSuporteDTOParaInserir_QuandoInserir_EntaoDeveMapearDTOParaEntidadeCorretamente()
        {
            // Arrange
            var suporteDTO = CriarSuporteDTOValido();
            var suporteMapeado = CriarSuporteValido();

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Suporte>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(suporteMapeado);

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.Inserir(It.IsAny<Suporte>()))
                .ReturnsAsync(1);

            // Act
            await _sut.Inserir(suporteDTO);

            // Assert
            _mocker.GetMock<AutoMapper.IMapper>()
                .Verify(m => m.Map<Suporte>(It.IsAny<IdNomeTipoExcluidoDTO>()), Times.Once);
        }

        [Fact]
        public async Task DadoSuporteDTOValido_QuandoInserir_EntaoDeveInteragirComRepositorio()
        {
            // Arrange
            var suporteDTO = CriarSuporteDTOValido();
            var suporteMapeado = CriarSuporteValido();

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Suporte>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(suporteMapeado);

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.Inserir(It.IsAny<Suporte>()))
                .ReturnsAsync(5);

            // Act
            await _sut.Inserir(suporteDTO);

            // Assert
            _mocker.GetMock<IRepositorioSuporte>()
                .Verify(r => r.Inserir(suporteMapeado), Times.Once);
        }

        #endregion

        #region Testes de ObterTodos (Herança ServicoAplicacao)

        [Fact]
        public async Task DadoRepositorioComSuportes_QuandoObterTodos_EntaoDeveRetornarListaDeSuporteDTOs()
        {
            // Arrange
            var suportes = new List<Suporte>
            {
                CriarSuporteValido(id: 1, nome: "Papel", tipo: TipoSuporte.IMAGEM, excluido: false),
                CriarSuporteValido(id: 2, nome: "Digital", tipo: TipoSuporte.VIDEO, excluido: false),
                CriarSuporteValido(id: 3, nome: "Tela", tipo: TipoSuporte.IMAGEM, excluido: false)
            };

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(suportes);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(It.IsAny<Suporte>()))
                .Returns<Suporte>(s => new IdNomeTipoExcluidoDTO
                {
                    Id = s.Id,
                    Nome = s.Nome,
                    Tipo = (int)s.Tipo,
                    Excluido = s.Excluido
                });

            // Act
            var resultado = await _sut.ObterTodos();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(3);
            resultado.Should().AllSatisfy(s => s.Should().BeOfType<IdNomeTipoExcluidoDTO>());
        }

        [Fact]
        public async Task DadoRepositorioComSuportesAtivosEExcluidos_QuandoObterTodos_EntaoDeveRetornarApenasAtivos()
        {
            // Arrange
            var suportes = new List<Suporte>
            {
                CriarSuporteValido(id: 1, nome: "Papel", tipo: TipoSuporte.IMAGEM, excluido: false),
                CriarSuporteValido(id: 2, nome: "Excluido", tipo: TipoSuporte.VIDEO, excluido: true),
                CriarSuporteValido(id: 3, nome: "Digital", tipo: TipoSuporte.IMAGEM, excluido: false),
                CriarSuporteValido(id: 4, nome: "Removido", tipo: TipoSuporte.VIDEO, excluido: true)
            };

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(suportes);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(It.IsAny<Suporte>()))
                .Returns<Suporte>(s => new IdNomeTipoExcluidoDTO
                {
                    Id = s.Id,
                    Nome = s.Nome,
                    Tipo = (int)s.Tipo,
                    Excluido = s.Excluido
                });

            // Act
            var resultado = await _sut.ObterTodos();

            // Assert
            resultado.Should().HaveCount(2);
            resultado.Should().AllSatisfy(s => s.Excluido.Should().BeFalse());
        }

        [Fact]
        public async Task DadoRepositorioSemSuportes_QuandoObterTodos_EntaoDeveRetornarListaVazia()
        {
            // Arrange
            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync([]);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(It.IsAny<Suporte>()))
                .Returns<Suporte>(s => new IdNomeTipoExcluidoDTO
                {
                    Id = s.Id,
                    Nome = s.Nome,
                    Tipo = (int)s.Tipo,
                    Excluido = s.Excluido
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
        public async Task DadoSuporteExistenteNaoExcluido_QuandoObterPorId_EntaoDeveRetornarSuporteDTO()
        {
            // Arrange
            var suporte = CriarSuporteValido(id: 5, nome: "Papel", tipo: TipoSuporte.IMAGEM, excluido: false);
            var suporteDTO = new IdNomeTipoExcluidoDTO
            {
                Id = suporte.Id,
                Nome = suporte.Nome,
                Tipo = (int)suporte.Tipo,
                Excluido = suporte.Excluido
            };

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterPorId(5))
                .ReturnsAsync(suporte);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(suporte))
                .Returns(suporteDTO);

            // Act
            var resultado = await _sut.ObterPorId(5);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<IdNomeTipoExcluidoDTO>();
            resultado.Id.Should().Be(5);
            resultado.Nome.Should().Be("Papel");
        }

        [Fact]
        public async Task DadoSuporteExcluido_QuandoObterPorId_EntaoDeveRetornarNull()
        {
            // Arrange
            var suporte = CriarSuporteValido(id: 5, excluido: true);

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterPorId(5))
                .ReturnsAsync(suporte);

            // Act
            var resultado = await _sut.ObterPorId(5);

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DadoIdInexistente_QuandoObterPorId_EntaoDeveRetornarNull()
        {
            // Arrange
            var suporteNulo = (Suporte)null!;

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync(suporteNulo);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(null))
                .Returns((IdNomeTipoExcluidoDTO)null!);

            // Act
            var resultado = await _sut.ObterPorId(999);

            // Assert
            resultado.Should().BeNull();
        }

        #endregion

        #region Testes de Alterar (Herança ServicoAplicacao)

        [Fact]
        public async Task DadoSuporteDTOExistente_QuandoAlterar_EntaoDeveRetornarSuporteDTOAlterado()
        {
            // Arrange
            var suporteDTO = CriarSuporteDTOValido(id: 3, nome: "Digital");
            var suporteAlterado = CriarSuporteValido(id: 3, nome: "Digital");
            var suporteDTOAlterado = new IdNomeTipoExcluidoDTO
            {
                Id = suporteAlterado.Id,
                Nome = suporteAlterado.Nome,
                Tipo = (int)suporteAlterado.Tipo,
                Excluido = suporteAlterado.Excluido
            };

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.Atualizar(It.IsAny<Suporte>()))
                .ReturnsAsync(suporteAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Suporte>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(suporteAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(suporteAlterado))
                .Returns(suporteDTOAlterado);

            // Act
            var resultado = await _sut.Alterar(suporteDTO);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<IdNomeTipoExcluidoDTO>();
            resultado.Id.Should().Be(3);
            resultado.Nome.Should().Be("Digital");
        }

        [Fact]
        public async Task DadoSuporteDTOParaAlterar_QuandoAlterar_EntaoDeveInteragirComRepositorio()
        {
            // Arrange
            var suporteDTO = CriarSuporteDTOValido(id: 3);
            var suporteMapeado = CriarSuporteValido(id: 3);
            var suporteDTORetorno = new IdNomeTipoExcluidoDTO
            {
                Id = 3,
                Nome = "Papel",
                Tipo = (int)TipoSuporte.IMAGEM,
                Excluido = false
            };

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Suporte>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(suporteMapeado);

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.Atualizar(It.IsAny<Suporte>()))
                .ReturnsAsync(suporteMapeado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(suporteMapeado))
                .Returns(suporteDTORetorno);

            // Act
            await _sut.Alterar(suporteDTO);

            // Assert
            _mocker.GetMock<IRepositorioSuporte>()
                .Verify(r => r.Atualizar(It.IsAny<Suporte>()), Times.Once);
        }

        #endregion

        #region Testes de Excluir (Herança ServicoAplicacao)

        [Fact]
        public async Task DadoSuporteExistente_QuandoExcluir_EntaoDeveRetornarVerdadeiro()
        {
            // Arrange
            var suporte = CriarSuporteValido(id: 7, excluido: false);
            var suporteExcluido = CriarSuporteValido(id: 7, excluido: true);
            var suporteDTO = new IdNomeTipoExcluidoDTO
            {
                Id = suporte.Id,
                Nome = suporte.Nome,
                Tipo = (int)suporte.Tipo,
                Excluido = suporte.Excluido
            };

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterPorId(7))
                .ReturnsAsync(suporte);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(suporte))
                .Returns(suporteDTO);

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.Atualizar(It.IsAny<Suporte>()))
                .ReturnsAsync(suporteExcluido);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Suporte>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(suporteExcluido);

            // Act
            var resultado = await _sut.Excluir(7);

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact]
        public async Task DadoSuporteParaExcluir_QuandoExcluir_EntaoDeveMarcarComoExcluido()
        {
            // Arrange
            var suporte = CriarSuporteValido(id: 7, excluido: false);
            var suporteExcluido = CriarSuporteValido(id: 7, excluido: true);
            var suporteDTO = new IdNomeTipoExcluidoDTO
            {
                Id = suporte.Id,
                Nome = suporte.Nome,
                Tipo = (int)suporte.Tipo,
                Excluido = suporte.Excluido
            };

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterPorId(7))
                .ReturnsAsync(suporte);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(suporte))
                .Returns(suporteDTO);

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.Atualizar(It.IsAny<Suporte>()))
                .ReturnsAsync(suporteExcluido);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Suporte>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(suporteExcluido);

            // Act
            await _sut.Excluir(7);

            // Assert
            _mocker.GetMock<IRepositorioSuporte>()
                .Verify(r => r.Atualizar(It.Is<Suporte>(s =>
                    s.Excluido
                )), Times.Once);
        }

        #endregion

        #region Testes de ObterPorNomeETipo (Método Específico)

        [Fact]
        public async Task DadoNomeETipoValidos_QuandoObterPorNomeETipo_EntaoDeveRetornarId()
        {
            // Arrange
            const string nome = "Papel";
            const int tipo = (int)TipoSuporte.IMAGEM;
            const long idEsperado = 5;

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterPorNomeTipo(nome, tipo))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.ObterPorNomeETipo(nome, tipo);

            // Assert
            resultado.Should().Be(idEsperado);
            resultado.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task DadoNomeETipoValidos_QuandoObterPorNomeETipo_EntaoDeveInteragirComRepositorio()
        {
            // Arrange
            const string nome = "Digital";
            const int tipo = (int)TipoSuporte.VIDEO;

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterPorNomeTipo(nome, tipo))
                .ReturnsAsync(10);

            // Act
            await _sut.ObterPorNomeETipo(nome, tipo);

            // Assert
            _mocker.GetMock<IRepositorioSuporte>()
                .Verify(r => r.ObterPorNomeTipo(nome, tipo), Times.Once);
        }

        [Fact]
        public async Task DadoNomeETipoNaoExistentes_QuandoObterPorNomeETipo_EntaoDeveRetornarZero()
        {
            // Arrange
            const string nome = "SUPORTE_INEXISTENTE";
            const int tipo = (int)TipoSuporte.IMAGEM;

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterPorNomeTipo(nome, tipo))
                .ReturnsAsync(0);

            // Act
            var resultado = await _sut.ObterPorNomeETipo(nome, tipo);

            // Assert
            resultado.Should().Be(0);
        }

        [Fact]
        public async Task DadoNomeComCaracteresEspeciais_QuandoObterPorNomeETipo_EntaoDeveProcessarCorretamente()
        {
            // Arrange
            const string nome = "Papel (A4) - Branco";
            const int tipo = (int)TipoSuporte.IMAGEM;
            const long idEsperado = 3;

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterPorNomeTipo(nome, tipo))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.ObterPorNomeETipo(nome, tipo);

            // Assert
            resultado.Should().Be(idEsperado);
        }

        [Fact]
        public async Task DadoNomeComAcentuacao_QuandoObterPorNomeETipo_EntaoDeveRetornarIdCorreto()
        {
            // Arrange
            const string nome = "Papéis";
            const int tipo = (int)TipoSuporte.IMAGEM;
            const long idEsperado = 2;

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterPorNomeTipo(nome, tipo))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.ObterPorNomeETipo(nome, tipo);

            // Assert
            resultado.Should().Be(idEsperado);
            _mocker.GetMock<IRepositorioSuporte>()
                .Verify(r => r.ObterPorNomeTipo(nome, tipo), Times.Once);
        }

        [Fact]
        public async Task DadoMultiplosNomesETipos_QuandoObterPorNomeETipoVarios_EntaoDeveRetornarIdsCorretos()
        {
            // Arrange
            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterPorNomeTipo("Papel", (int)TipoSuporte.IMAGEM))
                .ReturnsAsync(1);

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterPorNomeTipo("Digital", (int)TipoSuporte.VIDEO))
                .ReturnsAsync(2);

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterPorNomeTipo("Tela", (int)TipoSuporte.IMAGEM))
                .ReturnsAsync(3);

            // Act
            var resultado1 = await _sut.ObterPorNomeETipo("Papel", (int)TipoSuporte.IMAGEM);
            var resultado2 = await _sut.ObterPorNomeETipo("Digital", (int)TipoSuporte.VIDEO);
            var resultado3 = await _sut.ObterPorNomeETipo("Tela", (int)TipoSuporte.IMAGEM);

            // Assert
            resultado1.Should().Be(1);
            resultado2.Should().Be(2);
            resultado3.Should().Be(3);
        }

        [Fact]
        public async Task DadoTiposEnumeradosDiferentes_QuandoObterPorNomeETipo_EntaoDeveRetornarResultadosCorretos()
        {
            // Arrange
            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterPorNomeTipo("Recurso", (int)TipoSuporte.NAO_DEFINIDO))
                .ReturnsAsync(1);

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterPorNomeTipo("Imagem", (int)TipoSuporte.IMAGEM))
                .ReturnsAsync(2);

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterPorNomeTipo("Video", (int)TipoSuporte.VIDEO))
                .ReturnsAsync(3);

            // Act
            var resultadoNaoDef = await _sut.ObterPorNomeETipo("Recurso", (int)TipoSuporte.NAO_DEFINIDO);
            var resultadoImagem = await _sut.ObterPorNomeETipo("Imagem", (int)TipoSuporte.IMAGEM);
            var resultadoVideo = await _sut.ObterPorNomeETipo("Video", (int)TipoSuporte.VIDEO);

            // Assert
            resultadoNaoDef.Should().Be(1);
            resultadoImagem.Should().Be(2);
            resultadoVideo.Should().Be(3);
        }

        #endregion

        #region Testes de Fluxo Integrado

        [Fact]
        public async Task DadoFluxoCompletoDeInsercaoEConsulta_QuandoExecutarServico_EntaoRetornaResultadoEsperado()
        {
            // Arrange
            var suporteDTO = CriarSuporteDTOValido(nome: "Papel");
            var suporteMapeado = CriarSuporteValido(id: 1, nome: "Papel", tipo: TipoSuporte.IMAGEM);
            const long idInserido = 1;

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Suporte>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(suporteMapeado);

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.Inserir(It.IsAny<Suporte>()))
                .ReturnsAsync(idInserido);

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterPorId(idInserido))
                .ReturnsAsync(suporteMapeado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(suporteMapeado))
                .Returns(new IdNomeTipoExcluidoDTO
                {
                    Id = suporteMapeado.Id,
                    Nome = suporteMapeado.Nome,
                    Tipo = (int)suporteMapeado.Tipo,
                    Excluido = suporteMapeado.Excluido
                });

            // Act
            var idResultado = await _sut.Inserir(suporteDTO);
            var suporteRecuperado = await _sut.ObterPorId(idResultado);

            // Assert
            idResultado.Should().Be(idInserido);
            suporteRecuperado.Should().NotBeNull();
            suporteRecuperado.Nome.Should().Be("Papel");
        }

        [Fact]
        public async Task DadoFluxoCompletoDeInsercaoAlteracaoEExclusao_QuandoExecutarServico_EntaoRetornaResultadoEsperado()
        {
            // Arrange
            var suporteDTO = CriarSuporteDTOValido(nome: "Digital");
            var suporteMapeado = CriarSuporteValido(id: 1, nome: "Digital", tipo: TipoSuporte.VIDEO);
            var suporteExcluido = CriarSuporteValido(id: 1, nome: "Digital", tipo: TipoSuporte.VIDEO, excluido: true);

            const long idInserido = 1;

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Suporte>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(suporteMapeado);

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.Inserir(It.IsAny<Suporte>()))
                .ReturnsAsync(idInserido);

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.ObterPorId(idInserido))
                .ReturnsAsync(suporteMapeado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(It.IsAny<Suporte>()))
                .Returns<Suporte>(s => new IdNomeTipoExcluidoDTO
                {
                    Id = s.Id,
                    Nome = s.Nome,
                    Tipo = (int)s.Tipo,
                    Excluido = s.Excluido
                });

            _mocker.GetMock<IRepositorioSuporte>()
                .Setup(r => r.Atualizar(It.IsAny<Suporte>()))
                .ReturnsAsync(suporteExcluido);

            // Act
            var idResultado = await _sut.Inserir(suporteDTO);
            var suporteRecuperado = await _sut.ObterPorId(idResultado);
            var resultadoExclusao = await _sut.Excluir(idResultado);

            // Assert
            idResultado.Should().Be(idInserido);
            suporteRecuperado.Should().NotBeNull();
            resultadoExclusao.Should().BeTrue();
        }

        #endregion

        #region Testes de Implementação de Interface

        [Fact]
        public void DadoServicoSuporte_QuandoVerificarTipo_EntaoDeveImplementarIServicoSuporte()
        {
            // Assert
            _sut.Should().BeAssignableTo<IServicoSuporte>();
        }

        [Fact]
        public void DadoServicoSuporte_QuandoVerificarTipo_EntaoDeveSercedidoServicoAplicacao()
        {
            // Assert
            _sut.Should().BeAssignableTo<ServicoAplicacao<Suporte, IdNomeTipoExcluidoDTO>>();
        }

        #endregion

        #region Métodos Auxiliares

        private static IdNomeTipoExcluidoDTO CriarSuporteDTOValido(
            long id = 0,
            string nome = "Papel",
            int tipo = (int)TipoSuporte.IMAGEM,
            bool excluido = false)
        {
            return new IdNomeTipoExcluidoDTO
            {
                Id = id,
                Nome = nome,
                Tipo = tipo,
                Excluido = excluido
            };
        }

        private static Suporte CriarSuporteValido(
            long id = 0,
            string nome = "Papel",
            TipoSuporte tipo = TipoSuporte.IMAGEM,
            bool excluido = false)
        {
            return new Suporte
            {
                Id = id,
                Nome = nome,
                Tipo = tipo,
                Excluido = excluido
            };
        }

        #endregion
    }
}
