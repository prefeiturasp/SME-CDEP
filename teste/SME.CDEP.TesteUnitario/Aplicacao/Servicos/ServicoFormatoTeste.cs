using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoFormatoTeste
    {
        private readonly AutoMocker _mocker;
        private readonly ServicoFormato _sut;

        public ServicoFormatoTeste()
        {
            _mocker = new AutoMocker();
            _sut = _mocker.CreateInstance<ServicoFormato>();
        }

        #region Testes de Construtor

        [Fact]
        public void DadoRepositorioNulo_QuandoConstruir_EntaoLancaArgumentNullException()
        {
            // Arrange
            var mapper = _mocker.GetMock<AutoMapper.IMapper>();

            // Act
            Action acao = () => _ = new ServicoFormato(null!, mapper.Object);

            // Assert
            acao.Should().Throw<ArgumentNullException>()
                .WithMessage("*repositorio*");
        }

        [Fact]
        public void DadoMapperNulo_QuandoConstruir_EntaoLancaArgumentNullException()
        {
            // Arrange
            var repositorio = _mocker.GetMock<IRepositorioFormato>();

            // Act
            Action acao = () => _ = new ServicoFormato(repositorio.Object, null!);

            // Assert
            acao.Should().Throw<ArgumentNullException>()
                .WithMessage("*mapper*");
        }

        [Fact]
        public void DadoRepositorioEMapperValidos_QuandoConstruir_EntaoInstanciaComSucesso()
        {
            // Act
            var servico = _mocker.CreateInstance<ServicoFormato>();

            // Assert
            servico.Should().NotBeNull();
            servico.Should().BeOfType<ServicoFormato>();
        }

        #endregion

        #region Testes de Inserir

        [Fact]
        public async Task DadoFormatoDTOValido_QuandoInserir_EntaoDeveRetornarIdMaiorQueZero()
        {
            // Arrange
            var formatoDTO = CriarFormatoDTOValido();
            const long idEsperado = 10;

            var formatoMapeado = CriarFormatoValido(id: idEsperado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Formato>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(formatoMapeado);

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.Inserir(It.IsAny<Formato>()))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.Inserir(formatoDTO);

            // Assert
            resultado.Should().Be(idEsperado);
            resultado.Should().BeGreaterThan(0);
            _mocker.GetMock<IRepositorioFormato>()
                .Verify(r => r.Inserir(It.IsAny<Formato>()), Times.Once);
        }

        [Fact]
        public async Task DadoFormatoDTOParaInserir_QuandoInserir_EntaoDeveMapearDTOParaEntidadeCorretamente()
        {
            // Arrange
            var formatoDTO = CriarFormatoDTOValido();
            var formatoMapeado = CriarFormatoValido();

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Formato>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(formatoMapeado);

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.Inserir(It.IsAny<Formato>()))
                .ReturnsAsync(1);

            // Act
            await _sut.Inserir(formatoDTO);

            // Assert
            _mocker.GetMock<AutoMapper.IMapper>()
                .Verify(m => m.Map<Formato>(It.IsAny<IdNomeTipoExcluidoDTO>()), Times.Once);
        }

        [Fact]
        public async Task DadoFormatoDTOValido_QuandoInserir_EntaoDeveInteragirComRepositorio()
        {
            // Arrange
            var formatoDTO = CriarFormatoDTOValido();
            var formatoMapeado = CriarFormatoValido();

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Formato>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(formatoMapeado);

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.Inserir(It.IsAny<Formato>()))
                .ReturnsAsync(5);

            // Act
            await _sut.Inserir(formatoDTO);

            // Assert
            _mocker.GetMock<IRepositorioFormato>()
                .Verify(r => r.Inserir(formatoMapeado), Times.Once);
        }

        #endregion

        #region Testes de ObterTodos

        [Fact]
        public async Task DadoRepositorioComFormatos_QuandoObterTodos_EntaoDeveRetornarListaDeFormatoDTOs()
        {
            // Arrange
            var formatos = new List<Formato>
            {
                CriarFormatoValido(id: 1, nome: "JPEG", excluido: false),
                CriarFormatoValido(id: 2, nome: "PDF", excluido: false),
                CriarFormatoValido(id: 3, nome: "TIFF", excluido: false)
            };

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(formatos);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(It.IsAny<Formato>()))
                .Returns<Formato>(f => new IdNomeTipoExcluidoDTO
                {
                    Id = f.Id,
                    Nome = f.Nome,
                    Tipo = (int)f.Tipo,
                    Excluido = f.Excluido
                });

            // Act
            var resultado = await _sut.ObterTodos();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(3);
            resultado.Should().AllSatisfy(f => f.Should().BeOfType<IdNomeTipoExcluidoDTO>());
        }

        [Fact]
        public async Task DadoRepositorioComFormatosAtivosEExcluidos_QuandoObterTodos_EntaoDeveRetornarApenasAtivos()
        {
            // Arrange
            var formatos = new List<Formato>
            {
                CriarFormatoValido(id: 1, nome: "JPEG", excluido: false),
                CriarFormatoValido(id: 2, nome: "PDF", excluido: true),
                CriarFormatoValido(id: 3, nome: "TIFF", excluido: false),
                CriarFormatoValido(id: 4, nome: "DOC", excluido: true)
            };

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(formatos);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(It.IsAny<Formato>()))
                .Returns<Formato>(f => new IdNomeTipoExcluidoDTO
                {
                    Id = f.Id,
                    Nome = f.Nome,
                    Tipo = (int)f.Tipo,
                    Excluido = f.Excluido
                });

            // Act
            var resultado = await _sut.ObterTodos();

            // Assert
            resultado.Should().HaveCount(2);
            resultado.Should().AllSatisfy(f => f.Excluido.Should().BeFalse());
        }

        [Fact]
        public async Task DadoRepositorioSemFormatos_QuandoObterTodos_EntaoDeveRetornarListaVazia()
        {
            // Arrange
            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync([]);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(It.IsAny<Formato>()))
                .Returns<Formato>(f => new IdNomeTipoExcluidoDTO
                {
                    Id = f.Id,
                    Nome = f.Nome,
                    Tipo = (int)f.Tipo,
                    Excluido = f.Excluido
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
        public async Task DadoFormatoExistenteNaoExcluido_QuandoObterPorId_EntaoDeveRetornarFormatoDTO()
        {
            // Arrange
            var formato = CriarFormatoValido(id: 5, nome: "PDF", excluido: false);
            var formatoDTO = new IdNomeTipoExcluidoDTO
            {
                Id = formato.Id,
                Nome = formato.Nome,
                Tipo = (int)formato.Tipo,
                Excluido = formato.Excluido
            };

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.ObterPorId(5))
                .ReturnsAsync(formato);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(formato))
                .Returns(formatoDTO);

            // Act
            var resultado = await _sut.ObterPorId(5);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<IdNomeTipoExcluidoDTO>();
            resultado.Id.Should().Be(5);
            resultado.Nome.Should().Be("PDF");
        }

        [Fact]
        public async Task DadoFormatoExcluido_QuandoObterPorId_EntaoDeveRetornarNull()
        {
            // Arrange
            var formato = CriarFormatoValido(id: 5, excluido: true);

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.ObterPorId(5))
                .ReturnsAsync(formato);

            // Act
            var resultado = await _sut.ObterPorId(5);

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DadoIdInexistente_QuandoObterPorId_EntaoDeveRetornarNull()
        {
            // Arrange
            var formatoNulo = (Formato)null!;

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync(formatoNulo);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(null))
                .Returns((IdNomeTipoExcluidoDTO)null!);

            // Act
            var resultado = await _sut.ObterPorId(999);

            // Assert
            resultado.Should().BeNull();
        }

        #endregion

        #region Testes de Alterar

        [Fact]
        public async Task DadoFormatoDTOExistente_QuandoAlterar_EntaoDeveRetornarFormatoDTOAlterado()
        {
            // Arrange
            var formatoDTO = CriarFormatoDTOValido(id: 3, nome: "PNG");
            var formatoAlterado = CriarFormatoValido(id: 3, nome: "PNG");
            var formatoDTOAlterado = new IdNomeTipoExcluidoDTO
            {
                Id = formatoAlterado.Id,
                Nome = formatoAlterado.Nome,
                Tipo = (int)formatoAlterado.Tipo,
                Excluido = formatoAlterado.Excluido
            };

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.Atualizar(It.IsAny<Formato>()))
                .ReturnsAsync(formatoAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Formato>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(formatoAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(formatoAlterado))
                .Returns(formatoDTOAlterado);

            // Act
            var resultado = await _sut.Alterar(formatoDTO);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<IdNomeTipoExcluidoDTO>();
            resultado.Id.Should().Be(3);
            resultado.Nome.Should().Be("PNG");
        }

        [Fact]
        public async Task DadoFormatoDTOParaAlterar_QuandoAlterar_EntaoDeveInteragirComRepositorio()
        {
            // Arrange
            var formatoDTO = CriarFormatoDTOValido(id: 3);
            var formatoMapeado = CriarFormatoValido(id: 3);
            var formatoDTORetorno = new IdNomeTipoExcluidoDTO
            {
                Id = 3,
                Nome = "PNG",
                Tipo = (int)TipoFormato.ACERVO_FOTOS,
                Excluido = false
            };

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Formato>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(formatoMapeado);

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.Atualizar(It.IsAny<Formato>()))
                .ReturnsAsync(formatoMapeado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(formatoMapeado))
                .Returns(formatoDTORetorno);

            // Act
            await _sut.Alterar(formatoDTO);

            // Assert
            _mocker.GetMock<IRepositorioFormato>()
                .Verify(r => r.Atualizar(It.IsAny<Formato>()), Times.Once);
        }

        #endregion

        #region Testes de Excluir

        [Fact]
        public async Task DadoFormatoExistente_QuandoExcluir_EntaoDeveRetornarVerdadeiro()
        {
            // Arrange
            var formato = CriarFormatoValido(id: 7, excluido: false);
            var formatoExcluido = CriarFormatoValido(id: 7, excluido: true);
            var formatoDTO = new IdNomeTipoExcluidoDTO
            {
                Id = formato.Id,
                Nome = formato.Nome,
                Tipo = (int)formato.Tipo,
                Excluido = formato.Excluido
            };

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.ObterPorId(7))
                .ReturnsAsync(formato);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(formato))
                .Returns(formatoDTO);

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.Atualizar(It.IsAny<Formato>()))
                .ReturnsAsync(formatoExcluido);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Formato>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(formatoExcluido);

            // Act
            var resultado = await _sut.Excluir(7);

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact]
        public async Task DadoFormatoParaExcluir_QuandoExcluir_EntaoDeveMarcarComoExcluido()
        {
            // Arrange
            var formato = CriarFormatoValido(id: 7, excluido: false);
            var formatoExcluido = CriarFormatoValido(id: 7, excluido: true);
            var formatoDTO = new IdNomeTipoExcluidoDTO
            {
                Id = formato.Id,
                Nome = formato.Nome,
                Tipo = (int)formato.Tipo,
                Excluido = formato.Excluido
            };

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.ObterPorId(7))
                .ReturnsAsync(formato);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(formato))
                .Returns(formatoDTO);

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.Atualizar(It.IsAny<Formato>()))
                .ReturnsAsync(formatoExcluido);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Formato>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(formatoExcluido);

            // Act
            await _sut.Excluir(7);

            // Assert
            _mocker.GetMock<IRepositorioFormato>()
                .Verify(r => r.Atualizar(It.Is<Formato>(f =>
                    f.Excluido
                )), Times.Once);
        }

        #endregion

        #region Testes de ObterPorNomeETipo

        [Fact]
        public async Task DadoNomeETipoValidos_QuandoObterPorNomeETipo_EntaoDeveRetornarId()
        {
            // Arrange
            const string nome = "JPEG";
            const int tipo = (int)TipoFormato.ACERVO_FOTOS;
            const long idEsperado = 5;

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.ObterPorNomeETipo(nome, tipo))
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
            const string nome = "PDF";
            const int tipo = (int)TipoFormato.ACERVO_FOTOS;

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.ObterPorNomeETipo(nome, tipo))
                .ReturnsAsync(10);

            // Act
            await _sut.ObterPorNomeETipo(nome, tipo);

            // Assert
            _mocker.GetMock<IRepositorioFormato>()
                .Verify(r => r.ObterPorNomeETipo(nome, tipo), Times.Once);
        }

        [Fact]
        public async Task DadoNomeETipoNaoExistentes_QuandoObterPorNomeETipo_EntaoDeveRetornarZero()
        {
            // Arrange
            const string nome = "FORMATO_INEXISTENTE";
            const int tipo = (int)TipoFormato.ACERVO_FOTOS;

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.ObterPorNomeETipo(nome, tipo))
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
            const string nome = "TIFF/GEOTIFF";
            const int tipo = (int)TipoFormato.ACERVO_FOTOS;
            const long idEsperado = 3;

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.ObterPorNomeETipo(nome, tipo))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.ObterPorNomeETipo(nome, tipo);

            // Assert
            resultado.Should().Be(idEsperado);
        }

        [Fact]
        public async Task DadoMultiplosTiposFormato_QuandoObterPorNomeETipo_EntaoDeveRetornarCorretoPorTipo()
        {
            // Arrange
            const string nome = "MP4";
            const int tipoAudiovisual = (int)TipoFormato.ACERVO_AUDIOVISUAL;
            const long idAudiovisual = 20;

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.ObterPorNomeETipo(nome, tipoAudiovisual))
                .ReturnsAsync(idAudiovisual);

            // Act
            var resultado = await _sut.ObterPorNomeETipo(nome, tipoAudiovisual);

            // Assert
            resultado.Should().Be(idAudiovisual);
            _mocker.GetMock<IRepositorioFormato>()
                .Verify(r => r.ObterPorNomeETipo(nome, tipoAudiovisual), Times.Once);
        }

        #endregion

        #region Testes de Fluxo Integrado

        [Fact]
        public async Task DadoFluxoCompletoDeInsercaoEConsulta_QuandoExecutarServico_EntaoRetornaResultadoEsperado()
        {
            // Arrange
            var formatoDTO = CriarFormatoDTOValido(nome: "GIF");
            var formatoMapeado = CriarFormatoValido(id: 1, nome: "GIF");
            const long idInserido = 1;

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Formato>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(formatoMapeado);

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.Inserir(It.IsAny<Formato>()))
                .ReturnsAsync(idInserido);

            _mocker.GetMock<IRepositorioFormato>()
                .Setup(r => r.ObterPorId(idInserido))
                .ReturnsAsync(formatoMapeado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(formatoMapeado))
                .Returns(new IdNomeTipoExcluidoDTO
                {
                    Id = formatoMapeado.Id,
                    Nome = formatoMapeado.Nome,
                    Tipo = (int)formatoMapeado.Tipo,
                    Excluido = formatoMapeado.Excluido
                });

            // Act
            var idResultado = await _sut.Inserir(formatoDTO);
            var formatoRecuperado = await _sut.ObterPorId(idResultado);

            // Assert
            idResultado.Should().Be(idInserido);
            formatoRecuperado.Should().NotBeNull();
            formatoRecuperado.Nome.Should().Be("GIF");
        }

        #endregion

        #region Métodos Auxiliares

        private static IdNomeTipoExcluidoDTO CriarFormatoDTOValido(
            long id = 0,
            string nome = "JPEG",
            int tipo = (int)TipoFormato.ACERVO_FOTOS,
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

        private static Formato CriarFormatoValido(
            long id = 0,
            string nome = "JPEG",
            TipoFormato tipo = TipoFormato.ACERVO_FOTOS,
            bool excluido = false)
        {
            return new Formato
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
