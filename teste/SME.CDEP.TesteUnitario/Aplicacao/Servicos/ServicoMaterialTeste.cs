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
    public class ServicoMaterialTeste
    {
        private readonly AutoMocker _mocker;
        private readonly ServicoMaterial _sut;

        public ServicoMaterialTeste()
        {
            _mocker = new AutoMocker();
            _sut = _mocker.CreateInstance<ServicoMaterial>();
        }

        #region Testes de Construtor

        [Fact]
        public void DadoRepositorioNulo_QuandoConstruir_EntaoLancaArgumentNullException()
        {
            // Arrange
            var mapper = _mocker.GetMock<AutoMapper.IMapper>();

            // Act
            Action acao = () => _ = new ServicoMaterial(null!, mapper.Object);

            // Assert
            acao.Should().Throw<ArgumentNullException>()
                .WithMessage("*repositorio*");
        }

        [Fact]
        public void DadoMapperNulo_QuandoConstruir_EntaoLancaArgumentNullException()
        {
            // Arrange
            var repositorio = _mocker.GetMock<IRepositorioMaterial>();

            // Act
            Action acao = () => _ = new ServicoMaterial(repositorio.Object, null!);

            // Assert
            acao.Should().Throw<ArgumentNullException>()
                .WithMessage("*mapper*");
        }

        [Fact]
        public void DadoRepositorioEMapperValidos_QuandoConstruir_EntaoInstanciaComSucesso()
        {
            // Act
            var servico = _mocker.CreateInstance<ServicoMaterial>();

            // Assert
            servico.Should().NotBeNull();
            servico.Should().BeOfType<ServicoMaterial>();
        }

        #endregion

        #region Testes de Inserir (Herança ServicoAplicacao)

        [Fact]
        public async Task DadoMaterialDTOValido_QuandoInserir_EntaoDeveRetornarIdMaiorQueZero()
        {
            // Arrange
            var materialDTO = CriarMaterialDTOValido();
            const long idEsperado = 10;

            var materialMapeado = CriarMaterialValido(id: idEsperado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Material>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(materialMapeado);

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.Inserir(It.IsAny<Material>()))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.Inserir(materialDTO);

            // Assert
            resultado.Should().Be(idEsperado);
            resultado.Should().BeGreaterThan(0);
            _mocker.GetMock<IRepositorioMaterial>()
                .Verify(r => r.Inserir(It.IsAny<Material>()), Times.Once);
        }

        [Fact]
        public async Task DadoMaterialDTOParaInserir_QuandoInserir_EntaoDeveMapearDTOParaEntidadeCorretamente()
        {
            // Arrange
            var materialDTO = CriarMaterialDTOValido();
            var materialMapeado = CriarMaterialValido();

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Material>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(materialMapeado);

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.Inserir(It.IsAny<Material>()))
                .ReturnsAsync(1);

            // Act
            await _sut.Inserir(materialDTO);

            // Assert
            _mocker.GetMock<AutoMapper.IMapper>()
                .Verify(m => m.Map<Material>(It.IsAny<IdNomeTipoExcluidoDTO>()), Times.Once);
        }

        [Fact]
        public async Task DadoMaterialDTOValido_QuandoInserir_EntaoDeveInteragirComRepositorio()
        {
            // Arrange
            var materialDTO = CriarMaterialDTOValido();
            var materialMapeado = CriarMaterialValido();

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Material>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(materialMapeado);

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.Inserir(It.IsAny<Material>()))
                .ReturnsAsync(5);

            // Act
            await _sut.Inserir(materialDTO);

            // Assert
            _mocker.GetMock<IRepositorioMaterial>()
                .Verify(r => r.Inserir(materialMapeado), Times.Once);
        }

        #endregion

        #region Testes de ObterTodos (Herança ServicoAplicacao)

        [Fact]
        public async Task DadoRepositorioComMateriais_QuandoObterTodos_EntaoDeveRetornarListaDeMaterialDTOs()
        {
            // Arrange
            var materiais = new List<Material>
            {
                CriarMaterialValido(id: 1, nome: "Papel", tipo: TipoMaterial.DOCUMENTAL, excluido: false),
                CriarMaterialValido(id: 2, nome: "Livro", tipo: TipoMaterial.BIBLIOGRAFICO, excluido: false),
                CriarMaterialValido(id: 3, nome: "Disco", tipo: TipoMaterial.DOCUMENTAL, excluido: false)
            };

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(materiais);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(It.IsAny<Material>()))
                .Returns<Material>(m => new IdNomeTipoExcluidoDTO
                {
                    Id = m.Id,
                    Nome = m.Nome,
                    Excluido = m.Excluido
                });

            // Act
            var resultado = await _sut.ObterTodos();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(3);
            resultado.Should().AllSatisfy(m => m.Should().BeOfType<IdNomeTipoExcluidoDTO>());
        }

        [Fact]
        public async Task DadoRepositorioComMateriaisAtivosEExcluidos_QuandoObterTodos_EntaoDeveRetornarApenasAtivos()
        {
            // Arrange
            var materiais = new List<Material>
            {
                CriarMaterialValido(id: 1, nome: "Papel", tipo: TipoMaterial.DOCUMENTAL, excluido: false),
                CriarMaterialValido(id: 2, nome: "Péssimo", tipo: TipoMaterial.BIBLIOGRAFICO, excluido: true),
                CriarMaterialValido(id: 3, nome: "Livro", tipo: TipoMaterial.DOCUMENTAL, excluido: false),
                CriarMaterialValido(id: 4, nome: "Ruim", tipo: TipoMaterial.BIBLIOGRAFICO, excluido: true)
            };

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(materiais);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(It.IsAny<Material>()))
                .Returns<Material>(m => new IdNomeTipoExcluidoDTO
                {
                    Id = m.Id,
                    Nome = m.Nome,
                    Excluido = m.Excluido
                });

            // Act
            var resultado = await _sut.ObterTodos();

            // Assert
            resultado.Should().HaveCount(2);
            resultado.Should().AllSatisfy(m => m.Excluido.Should().BeFalse());
        }

        [Fact]
        public async Task DadoRepositorioSemMateriais_QuandoObterTodos_EntaoDeveRetornarListaVazia()
        {
            // Arrange
            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync([]);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(It.IsAny<Material>()))
                .Returns<Material>(m => new IdNomeTipoExcluidoDTO
                {
                    Id = m.Id,
                    Nome = m.Nome,
                    Excluido = m.Excluido
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
        public async Task DadoMaterialExistenteNaoExcluido_QuandoObterPorId_EntaoDeveMaterailDTO()
        {
            // Arrange
            var material = CriarMaterialValido(id: 5, nome: "Livro", tipo: TipoMaterial.BIBLIOGRAFICO, excluido: false);
            var materialDTO = new IdNomeTipoExcluidoDTO
            {
                Id = material.Id,
                Nome = material.Nome,
                Excluido = material.Excluido
            };

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.ObterPorId(5))
                .ReturnsAsync(material);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(material))
                .Returns(materialDTO);

            // Act
            var resultado = await _sut.ObterPorId(5);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<IdNomeTipoExcluidoDTO>();
            resultado.Id.Should().Be(5);
            resultado.Nome.Should().Be("Livro");
        }

        [Fact]
        public async Task DadoMaterialExcluido_QuandoObterPorId_EntaoDeveRetornarNull()
        {
            // Arrange
            var material = CriarMaterialValido(id: 5, excluido: true);

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.ObterPorId(5))
                .ReturnsAsync(material);

            // Act
            var resultado = await _sut.ObterPorId(5);

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DadoIdInexistente_QuandoObterPorId_EntaoDeveRetornarNull()
        {
            // Arrange
            var materialNulo = (Material)null!;

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync(materialNulo);

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
        public async Task DadoMaterialDTOExistente_QuandoAlterar_EntaoDeveRetornarMaterialDTOAlterado()
        {
            // Arrange
            var materialDTO = CriarMaterialDTOValido(id: 3, nome: "Pano");
            var materialAlterado = CriarMaterialValido(id: 3, nome: "Pano");
            var materialDTOAlterado = new IdNomeTipoExcluidoDTO
            {
                Id = materialAlterado.Id,
                Nome = materialAlterado.Nome,
                Excluido = materialAlterado.Excluido
            };

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.Atualizar(It.IsAny<Material>()))
                .ReturnsAsync(materialAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Material>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(materialAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(materialAlterado))
                .Returns(materialDTOAlterado);

            // Act
            var resultado = await _sut.Alterar(materialDTO);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<IdNomeTipoExcluidoDTO>();
            resultado.Id.Should().Be(3);
            resultado.Nome.Should().Be("Pano");
        }

        [Fact]
        public async Task DadoMaterialDTOParaAlterar_QuandoAlterar_EntaoDeveInteragirComRepositorio()
        {
            // Arrange
            var materialDTO = CriarMaterialDTOValido(id: 3);
            var materialMapeado = CriarMaterialValido(id: 3);
            var materialDTORetorno = new IdNomeTipoExcluidoDTO
            {
                Id = 3,
                Nome = "Papel",
                Excluido = false
            };

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Material>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(materialMapeado);

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.Atualizar(It.IsAny<Material>()))
                .ReturnsAsync(materialMapeado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(materialMapeado))
                .Returns(materialDTORetorno);

            // Act
            await _sut.Alterar(materialDTO);

            // Assert
            _mocker.GetMock<IRepositorioMaterial>()
                .Verify(r => r.Atualizar(It.IsAny<Material>()), Times.Once);
        }

        #endregion

        #region Testes de Excluir (Herança ServicoAplicacao)

        [Fact]
        public async Task DadoMaterialExistente_QuandoExcluir_EntaoDeveRetornarVerdadeiro()
        {
            // Arrange
            var material = CriarMaterialValido(id: 7, excluido: false);
            var materialExcluido = CriarMaterialValido(id: 7, excluido: true);
            var materialDTO = new IdNomeTipoExcluidoDTO
            {
                Id = material.Id,
                Nome = material.Nome,
                Excluido = material.Excluido
            };

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.ObterPorId(7))
                .ReturnsAsync(material);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(material))
                .Returns(materialDTO);

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.Atualizar(It.IsAny<Material>()))
                .ReturnsAsync(materialExcluido);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Material>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(materialExcluido);

            // Act
            var resultado = await _sut.Excluir(7);

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact]
        public async Task DadoMaterialParaExcluir_QuandoExcluir_EntaoDeveMarcarComoExcluido()
        {
            // Arrange
            var material = CriarMaterialValido(id: 7, excluido: false);
            var materialExcluido = CriarMaterialValido(id: 7, excluido: true);
            var materialDTO = new IdNomeTipoExcluidoDTO
            {
                Id = material.Id,
                Nome = material.Nome,
                Excluido = material.Excluido
            };

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.ObterPorId(7))
                .ReturnsAsync(material);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(material))
                .Returns(materialDTO);

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.Atualizar(It.IsAny<Material>()))
                .ReturnsAsync(materialExcluido);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Material>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(materialExcluido);

            // Act
            await _sut.Excluir(7);

            // Assert
            _mocker.GetMock<IRepositorioMaterial>()
                .Verify(r => r.Atualizar(It.Is<Material>(m =>
                    m.Excluido
                )), Times.Once);
        }

        #endregion

        #region Testes de ObterPorNomeETipo (Método Específico)

        [Fact]
        public async Task DadoNomeETipoValido_QuandoObterPorNomeETipo_EntaoDeveRetornarId()
        {
            // Arrange
            const string nome = "Papel";
            const TipoMaterial tipo = TipoMaterial.DOCUMENTAL;
            const long idEsperado = 5;

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.ObterPorNomeTipo(nome, tipo))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.ObterPorNomeETipo(nome, tipo);

            // Assert
            resultado.Should().Be(idEsperado);
            resultado.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task DadoNomeETipoValido_QuandoObterPorNomeETipo_EntaoDeveInteragirComRepositorio()
        {
            // Arrange
            const string nome = "Livro";
            const TipoMaterial tipo = TipoMaterial.BIBLIOGRAFICO;

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.ObterPorNomeTipo(nome, tipo))
                .ReturnsAsync(10);

            // Act
            await _sut.ObterPorNomeETipo(nome, tipo);

            // Assert
            _mocker.GetMock<IRepositorioMaterial>()
                .Verify(r => r.ObterPorNomeTipo(nome, tipo), Times.Once);
        }

        [Fact]
        public async Task DadoNomeETipoNaoExistente_QuandoObterPorNomeETipo_EntaoDeveRetornarZero()
        {
            // Arrange
            const string nome = "MATERIAL_INEXISTENTE";
            const TipoMaterial tipo = TipoMaterial.NAO_DEFINIDO;

            _mocker.GetMock<IRepositorioMaterial>()
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
            const TipoMaterial tipo = TipoMaterial.DOCUMENTAL;
            const long idEsperado = 3;

            _mocker.GetMock<IRepositorioMaterial>()
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
            const TipoMaterial tipo = TipoMaterial.DOCUMENTAL;
            const long idEsperado = 2;

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.ObterPorNomeTipo(nome, tipo))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.ObterPorNomeETipo(nome, tipo);

            // Assert
            resultado.Should().Be(idEsperado);
            _mocker.GetMock<IRepositorioMaterial>()
                .Verify(r => r.ObterPorNomeTipo(nome, tipo), Times.Once);
        }

        [Fact]
        public async Task DadoMultiplosNomesETipos_QuandoObterPorNomeETipoVarios_EntaoDeveRetornarIdsCorretos()
        {
            // Arrange
            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.ObterPorNomeTipo("Papel", TipoMaterial.DOCUMENTAL))
                .ReturnsAsync(1);

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.ObterPorNomeTipo("Livro", TipoMaterial.BIBLIOGRAFICO))
                .ReturnsAsync(2);

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.ObterPorNomeTipo("Disco", TipoMaterial.DOCUMENTAL))
                .ReturnsAsync(3);

            // Act
            var resultado1 = await _sut.ObterPorNomeETipo("Papel", TipoMaterial.DOCUMENTAL);
            var resultado2 = await _sut.ObterPorNomeETipo("Livro", TipoMaterial.BIBLIOGRAFICO);
            var resultado3 = await _sut.ObterPorNomeETipo("Disco", TipoMaterial.DOCUMENTAL);

            // Assert
            resultado1.Should().Be(1);
            resultado2.Should().Be(2);
            resultado3.Should().Be(3);
        }

        [Fact]
        public async Task DadoTiposEnumeradosDiferentes_QuandoObterPorNomeETipo_EntaoDeveRetornarResultadosCorretos()
        {
            // Arrange
            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.ObterPorNomeTipo("Recurso", TipoMaterial.NAO_DEFINIDO))
                .ReturnsAsync(1);

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.ObterPorNomeTipo("Documento", TipoMaterial.DOCUMENTAL))
                .ReturnsAsync(2);

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.ObterPorNomeTipo("Publicação", TipoMaterial.BIBLIOGRAFICO))
                .ReturnsAsync(3);

            // Act
            var resultadoNaoDef = await _sut.ObterPorNomeETipo("Recurso", TipoMaterial.NAO_DEFINIDO);
            var resultadoDocumental = await _sut.ObterPorNomeETipo("Documento", TipoMaterial.DOCUMENTAL);
            var resultadoBibliografico = await _sut.ObterPorNomeETipo("Publicação", TipoMaterial.BIBLIOGRAFICO);

            // Assert
            resultadoNaoDef.Should().Be(1);
            resultadoDocumental.Should().Be(2);
            resultadoBibliografico.Should().Be(3);
        }

        #endregion

        #region Testes de Fluxo Integrado

        [Fact]
        public async Task DadoFluxoCompletoDeInsercaoEConsulta_QuandoExecutarServico_EntaoRetornaResultadoEsperado()
        {
            // Arrange
            var materialDTO = CriarMaterialDTOValido(nome: "Papel");
            var materialMapeado = CriarMaterialValido(id: 1, nome: "Papel", tipo: TipoMaterial.DOCUMENTAL);
            const long idInserido = 1;

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Material>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(materialMapeado);

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.Inserir(It.IsAny<Material>()))
                .ReturnsAsync(idInserido);

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.ObterPorId(idInserido))
                .ReturnsAsync(materialMapeado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(materialMapeado))
                .Returns(new IdNomeTipoExcluidoDTO
                {
                    Id = materialMapeado.Id,
                    Nome = materialMapeado.Nome,
                    Excluido = materialMapeado.Excluido
                });

            // Act
            var idResultado = await _sut.Inserir(materialDTO);
            var materialRecuperado = await _sut.ObterPorId(idResultado);

            // Assert
            idResultado.Should().Be(idInserido);
            materialRecuperado.Should().NotBeNull();
            materialRecuperado.Nome.Should().Be("Papel");
        }

        [Fact]
        public async Task DadoFluxoCompletoDeInsercaoAlteracaoEExclusao_QuandoExecutarServico_EntaoRetornaResultadoEsperado()
        {
            // Arrange
            var materialDTO = CriarMaterialDTOValido(nome: "Livro");
            var materialMapeado = CriarMaterialValido(id: 1, nome: "Livro", tipo: TipoMaterial.BIBLIOGRAFICO);
            var materialExcluido = CriarMaterialValido(id: 1, nome: "Papel", tipo: TipoMaterial.DOCUMENTAL, excluido: true);

            const long idInserido = 1;

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Material>(It.IsAny<IdNomeTipoExcluidoDTO>()))
                .Returns(materialMapeado);

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.Inserir(It.IsAny<Material>()))
                .ReturnsAsync(idInserido);

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.ObterPorId(idInserido))
                .ReturnsAsync(materialMapeado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<IdNomeTipoExcluidoDTO>(It.IsAny<Material>()))
                .Returns<Material>(m => new IdNomeTipoExcluidoDTO
                {
                    Id = m.Id,
                    Nome = m.Nome,
                    Excluido = m.Excluido
                });

            _mocker.GetMock<IRepositorioMaterial>()
                .Setup(r => r.Atualizar(It.IsAny<Material>()))
                .ReturnsAsync(materialExcluido);

            // Act
            var idResultado = await _sut.Inserir(materialDTO);
            var materialRecuperado = await _sut.ObterPorId(idResultado);
            var resultadoExclusao = await _sut.Excluir(idResultado);

            // Assert
            idResultado.Should().Be(idInserido);
            materialRecuperado.Should().NotBeNull();
            resultadoExclusao.Should().BeTrue();
        }

        #endregion

        #region Testes de Implementação de Interface

        [Fact]
        public void DadoServicoMaterial_QuandoVerificarTipo_EntaoDeveImplementarIServicoMaterial()
        {
            // Assert
            _sut.Should().BeAssignableTo<IServicoMaterial>();
        }

        [Fact]
        public void DadoServicoMaterial_QuandoVerificarTipo_EntaoDeveSercedidoSe()
        {
            // Assert
            _sut.Should().BeAssignableTo<ServicoAplicacao<Material, IdNomeTipoExcluidoDTO>>();
        }

        #endregion

        #region Métodos Auxiliares

        private static IdNomeTipoExcluidoDTO CriarMaterialDTOValido(
            long id = 0,
            string nome = "Papel",
            bool excluido = false)
        {
            return new IdNomeTipoExcluidoDTO
            {
                Id = id,
                Nome = nome,
                Excluido = excluido
            };
        }

        private static Material CriarMaterialValido(
            long id = 0,
            string nome = "Papel",
            TipoMaterial tipo = TipoMaterial.DOCUMENTAL,
            bool excluido = false)
        {
            return new Material
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
