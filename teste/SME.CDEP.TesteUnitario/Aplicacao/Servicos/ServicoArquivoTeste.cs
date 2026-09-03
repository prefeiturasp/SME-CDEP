using Bogus;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoArquivoTeste
    {
        private readonly AutoMocker _mocker;
        private readonly ServicoArquivo _servico;
        private readonly Faker _faker;

        public ServicoArquivoTeste()
        {
            _mocker = new AutoMocker();
            _faker = new Faker("pt_BR");

            // Mock padrão de contexto para auditoria
            var mockContexto = _mocker.GetMock<IContextoAplicacao>();
            mockContexto.Setup(c => c.NomeUsuario).Returns("UsuarioTeste");
            mockContexto.Setup(c => c.UsuarioLogado).Returns("login.teste");

            _servico = _mocker.CreateInstance<ServicoArquivo>();
        }

        #region Testes de Inserir

        [Fact]
        public async Task DadoArquivoDTOValido_QuandoInserir_EntaoDeveRetornarIdMaiorQueZero()
        {
            // Arrange
            var arquivoDTO = CriarArquivoDTOValido();
            const long idEsperado = 42;

            var arquivoMapeado = CriarArquivoValido(id: idEsperado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Arquivo>(It.IsAny<ArquivoDTO>()))
                .Returns(arquivoMapeado);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.Inserir(It.IsAny<Arquivo>()))
                .ReturnsAsync(idEsperado);

            // Act
            var resultado = await _servico.Inserir(arquivoDTO);

            // Assert
            resultado.Should().Be(idEsperado);
            resultado.Should().BeGreaterThan(0);

            _mocker.GetMock<IRepositorioArquivo>()
                .Verify(r => r.Inserir(It.IsAny<Arquivo>()), Times.Once);
        }

        [Fact]
        public async Task DadoArquivoDTOParaInserir_QuandoInserir_EntaoDeveDefinirCriadoEmComHorarioBrasilia()
        {
            // Arrange
            var arquivoDTO = CriarArquivoDTOValido();
            var arquivoMapeado = CriarArquivoValido();

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Arquivo>(It.IsAny<ArquivoDTO>()))
                .Returns(arquivoMapeado);

            var horarioAntes = DateTimeExtension.HorarioBrasilia();

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.Inserir(It.IsAny<Arquivo>()))
                .ReturnsAsync(1);

            // Act
            await _servico.Inserir(arquivoDTO);

            var horarioDepois = DateTimeExtension.HorarioBrasilia();

            // Assert
            _mocker.GetMock<IRepositorioArquivo>()
                .Verify(r => r.Inserir(It.Is<Arquivo>(a =>
                    a.CriadoEm >= horarioAntes && a.CriadoEm <= horarioDepois
                )), Times.Once);
        }

        [Fact]
        public async Task DadoArquivoDTOParaInserir_QuandoInserir_EntaoDeveDefinirCriadoPorComNomeUsuarioDoContexto()
        {
            // Arrange
            var arquivoDTO = CriarArquivoDTOValido();
            var arquivoMapeado = CriarArquivoValido();
            const string nomeUsuarioEsperado = "UsuarioTeste";

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Arquivo>(It.IsAny<ArquivoDTO>()))
                .Returns(arquivoMapeado);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.Inserir(It.IsAny<Arquivo>()))
                .ReturnsAsync(1);

            // Act
            await _servico.Inserir(arquivoDTO);

            // Assert
            _mocker.GetMock<IRepositorioArquivo>()
                .Verify(r => r.Inserir(It.Is<Arquivo>(a =>
                    a.CriadoPor == nomeUsuarioEsperado
                )), Times.Once);
        }

        [Fact]
        public async Task DadoArquivoDTOParaInserir_QuandoInserir_EntaoDeveDefinirCriadoLoginComUsuarioLogadoDoContexto()
        {
            // Arrange
            var arquivoDTO = CriarArquivoDTOValido();
            var arquivoMapeado = CriarArquivoValido();
            const string usuarioLogadoEsperado = "login.teste";

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Arquivo>(It.IsAny<ArquivoDTO>()))
                .Returns(arquivoMapeado);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.Inserir(It.IsAny<Arquivo>()))
                .ReturnsAsync(1);

            // Act
            await _servico.Inserir(arquivoDTO);

            // Assert
            _mocker.GetMock<IRepositorioArquivo>()
                .Verify(r => r.Inserir(It.Is<Arquivo>(a =>
                    a.CriadoLogin == usuarioLogadoEsperado
                )), Times.Once);
        }

        [Fact]
        public async Task DadoArquivoDTOValido_QuandoInserir_EntaoDeveConverterDTOParaEntidadeCorretamente()
        {
            // Arrange
            var arquivoDTO = CriarArquivoDTOValido();
            var arquivoMapeado = CriarArquivoValido();

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Arquivo>(It.IsAny<ArquivoDTO>()))
                .Returns(arquivoMapeado);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.Inserir(It.IsAny<Arquivo>()))
                .ReturnsAsync(10);

            // Act
            await _servico.Inserir(arquivoDTO);

            // Assert
            _mocker.GetMock<IRepositorioArquivo>()
                .Verify(r => r.Inserir(It.IsAny<Arquivo>()), Times.Once);
        }

        #endregion

        #region Testes de ObterTodos

        [Fact]
        public async Task DadoRepositorioComArquivos_QuandoObterTodos_EntaoDeveRetornarListaDeArquivoDTOs()
        {
            // Arrange
            var arquivos = new List<Arquivo>
            {
                CriarArquivoValido(id: 1, excluido: false),
                CriarArquivoValido(id: 2, excluido: false),
                CriarArquivoValido(id: 3, excluido: false)
            };

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(arquivos);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<ArquivoDTO>(It.IsAny<Arquivo>()))
                .Returns<Arquivo>(a => new ArquivoDTO
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    Codigo = a.Codigo,
                    TipoConteudo = a.TipoConteudo,
                    Tipo = a.Tipo,
                    Excluido = a.Excluido
                });

            // Act
            var resultado = await _servico.ObterTodos();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(3);
            resultado.Should().AllSatisfy(a => a.Should().BeOfType<ArquivoDTO>());
        }

        [Fact]
        public async Task DadoRepositorioComArquivosAtivosEExcluidos_QuandoObterTodos_EntaoDeveRetornarApenasAtivos()
        {
            // Arrange
            var arquivos = new List<Arquivo>
            {
                CriarArquivoValido(id: 1, excluido: false),
                CriarArquivoValido(id: 2, excluido: true),
                CriarArquivoValido(id: 3, excluido: false),
                CriarArquivoValido(id: 4, excluido: true)
            };

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(arquivos);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<ArquivoDTO>(It.IsAny<Arquivo>()))
                .Returns<Arquivo>(a => new ArquivoDTO
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    Codigo = a.Codigo,
                    TipoConteudo = a.TipoConteudo,
                    Tipo = a.Tipo,
                    Excluido = a.Excluido
                });

            // Act
            var resultado = await _servico.ObterTodos();

            // Assert
            resultado.Should().HaveCount(2);
            resultado.Should().AllSatisfy(a => a.Excluido.Should().BeFalse());
        }

        [Fact]
        public async Task DadoRepositorioSemArquivos_QuandoObterTodos_EntaoDeveRetornarListaVazia()
        {
            // Arrange
            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync([]);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<ArquivoDTO>(It.IsAny<Arquivo>()))
                .Returns<Arquivo>(a => new ArquivoDTO
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    Codigo = a.Codigo,
                    TipoConteudo = a.TipoConteudo,
                    Tipo = a.Tipo,
                    Excluido = a.Excluido
                });

            // Act
            var resultado = await _servico.ObterTodos();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
        }

        [Fact]
        public async Task DadoRepositorioComArquivos_QuandoObterTodos_EntaoDeveMapearTodosOsCampos()
        {
            // Arrange
            var codigo = Guid.NewGuid();
            var arquivo = CriarArquivoValido(id: 1, codigo: codigo, excluido: false);
            var arquivos = new List<Arquivo> { arquivo };

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(arquivos);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<ArquivoDTO>(arquivo))
                .Returns(new ArquivoDTO
                {
                    Id = arquivo.Id,
                    Nome = arquivo.Nome,
                    Codigo = arquivo.Codigo,
                    TipoConteudo = arquivo.TipoConteudo,
                    Tipo = arquivo.Tipo,
                    Excluido = arquivo.Excluido
                });

            // Act
            var resultado = await _servico.ObterTodos();

            // Assert
            var arquivoDTO = resultado.First();
            arquivoDTO.Id.Should().Be(arquivo.Id);
            arquivoDTO.Nome.Should().Be(arquivo.Nome);
            arquivoDTO.Codigo.Should().Be(arquivo.Codigo);
            arquivoDTO.TipoConteudo.Should().Be(arquivo.TipoConteudo);
            arquivoDTO.Tipo.Should().Be(arquivo.Tipo);
        }

        #endregion

        #region Testes de Alterar

        [Fact]
        public async Task DadoArquivoDTOExistente_QuandoAlterar_EntaoDeveRetornarArquivoDTOAlterado()
        {
            // Arrange
            var arquivoDTO = CriarArquivoDTOValido(id: 5);
            var arquivoExistente = CriarArquivoValido(id: 5, excluido: false);
            var arquivoAlterado = CriarArquivoValido(id: 5, excluido: false);
            var arquivoDTOAlterado = CriarArquivoDTOValido(id: 5);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.ObterPorId(5))
                .ReturnsAsync(arquivoExistente);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.Atualizar(It.IsAny<Arquivo>()))
                .ReturnsAsync(arquivoAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Arquivo>(It.IsAny<ArquivoDTO>()))
                .Returns(arquivoAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<ArquivoDTO>(arquivoAlterado))
                .Returns(arquivoDTOAlterado);

            // Act
            var resultado = await _servico.Alterar(arquivoDTO);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<ArquivoDTO>();
            resultado.Id.Should().Be(5);
        }

        [Fact]
        public async Task DadoArquivoDTOParaAlterar_QuandoAlterar_EntaoDeveMantercamposDeCriacaoOriginais()
        {
            // Arrange
            var arquivoDTO = CriarArquivoDTOValido(id: 5);
            var dataCriacaoOriginal = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Local);
            var nomeCriadorOriginal = "CriadorOriginal";
            var loginCriadorOriginal = "login.criador";

            var arquivoExistente = CriarArquivoValido(
                id: 5,
                criadoEm: dataCriacaoOriginal,
                criadoPor: nomeCriadorOriginal,
                criadoLogin: loginCriadorOriginal,
                excluido: false
            );

            var arquivoAlterado = CriarArquivoValido(
                id: 5,
                criadoEm: dataCriacaoOriginal,
                criadoPor: nomeCriadorOriginal,
                criadoLogin: loginCriadorOriginal,
                excluido: false
            );

            var arquivoDTOAlterado = CriarArquivoDTOValido(id: 5);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.ObterPorId(5))
                .ReturnsAsync(arquivoExistente);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.Atualizar(It.IsAny<Arquivo>()))
                .ReturnsAsync(arquivoAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Arquivo>(It.IsAny<ArquivoDTO>()))
                .Returns(arquivoAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<ArquivoDTO>(arquivoAlterado))
                .Returns(arquivoDTOAlterado);

            // Act
            await _servico.Alterar(arquivoDTO);

            // Assert
            _mocker.GetMock<IRepositorioArquivo>()
                .Verify(r => r.Atualizar(It.Is<Arquivo>(a =>
                    a.CriadoEm == dataCriacaoOriginal &&
                    a.CriadoPor == nomeCriadorOriginal &&
                    a.CriadoLogin == loginCriadorOriginal
                )), Times.Once);
        }

        [Fact]
        public async Task DadoArquivoDTOParaAlterar_QuandoAlterar_EntaoDeveDefinirAlteradoEmComHorarioBrasilia()
        {
            // Arrange
            var arquivoDTO = CriarArquivoDTOValido(id: 5);
            var arquivoExistente = CriarArquivoValido(id: 5, excluido: false);
            var arquivoAlterado = CriarArquivoValido(id: 5, excluido: false);
            var arquivoDTOAlterado = CriarArquivoDTOValido(id: 5);

            var horarioAntes = DateTimeExtension.HorarioBrasilia();

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.ObterPorId(5))
                .ReturnsAsync(arquivoExistente);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.Atualizar(It.IsAny<Arquivo>()))
                .ReturnsAsync(arquivoAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Arquivo>(It.IsAny<ArquivoDTO>()))
                .Returns(arquivoAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<ArquivoDTO>(arquivoAlterado))
                .Returns(arquivoDTOAlterado);

            // Act
            await _servico.Alterar(arquivoDTO);

            var horarioDepois = DateTimeExtension.HorarioBrasilia();

            // Assert
            _mocker.GetMock<IRepositorioArquivo>()
                .Verify(r => r.Atualizar(It.Is<Arquivo>(a =>
                    a.AlteradoEm >= horarioAntes && a.AlteradoEm <= horarioDepois
                )), Times.Once);
        }

        [Fact]
        public async Task DadoArquivoDTOParaAlterar_QuandoAlterar_EntaoDeveDefinirAlteradoPorComNomeUsuarioDoContexto()
        {
            // Arrange
            var arquivoDTO = CriarArquivoDTOValido(id: 5);
            var arquivoExistente = CriarArquivoValido(id: 5, excluido: false);
            var arquivoAlterado = CriarArquivoValido(id: 5, excluido: false);
            var arquivoDTOAlterado = CriarArquivoDTOValido(id: 5);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.ObterPorId(5))
                .ReturnsAsync(arquivoExistente);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.Atualizar(It.IsAny<Arquivo>()))
                .ReturnsAsync(arquivoAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Arquivo>(It.IsAny<ArquivoDTO>()))
                .Returns(arquivoAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<ArquivoDTO>(arquivoAlterado))
                .Returns(arquivoDTOAlterado);

            // Act
            await _servico.Alterar(arquivoDTO);

            // Assert
            _mocker.GetMock<IRepositorioArquivo>()
                .Verify(r => r.Atualizar(It.Is<Arquivo>(a =>
                    a.AlteradoPor == "UsuarioTeste"
                )), Times.Once);
        }

        [Fact]
        public async Task DadoArquivoDTOParaAlterar_QuandoAlterar_EntaoDeveDefinirAlteradoLoginComUsuarioLogadoDoContexto()
        {
            // Arrange
            var arquivoDTO = CriarArquivoDTOValido(id: 5);
            var arquivoExistente = CriarArquivoValido(id: 5, excluido: false);
            var arquivoAlterado = CriarArquivoValido(id: 5, excluido: false);
            var arquivoDTOAlterado = CriarArquivoDTOValido(id: 5);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.ObterPorId(5))
                .ReturnsAsync(arquivoExistente);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.Atualizar(It.IsAny<Arquivo>()))
                .ReturnsAsync(arquivoAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Arquivo>(It.IsAny<ArquivoDTO>()))
                .Returns(arquivoAlterado);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<ArquivoDTO>(arquivoAlterado))
                .Returns(arquivoDTOAlterado);

            // Act
            await _servico.Alterar(arquivoDTO);

            // Assert
            _mocker.GetMock<IRepositorioArquivo>()
                .Verify(r => r.Atualizar(It.Is<Arquivo>(a =>
                    a.AlteradoLogin == "login.teste"
                )), Times.Once);
        }

        #endregion

        #region Testes de ObterPorId

        [Fact]
        public async Task DadoArquivoExistenteNaoExcluido_QuandoObterPorId_EntaoDeveRetornarArquivoDTO()
        {
            // Arrange
            var arquivo = CriarArquivoValido(id: 10, excluido: false);
            var arquivoDTO = CriarArquivoDTOValido(id: 10);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.ObterPorId(10))
                .ReturnsAsync(arquivo);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<ArquivoDTO>(arquivo))
                .Returns(arquivoDTO);

            // Act
            var resultado = await _servico.ObterPorId(10);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<ArquivoDTO>();
            resultado.Id.Should().Be(10);
        }

        [Fact]
        public async Task DadoArquivoExcluido_QuandoObterPorId_EntaoDeveRetornarNull()
        {
            // Arrange
            var arquivo = CriarArquivoValido(id: 10, excluido: true);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.ObterPorId(10))
                .ReturnsAsync(arquivo);

            // Act
            var resultado = await _servico.ObterPorId(10);

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DadoIdInexistente_QuandoObterPorId_EntaoDeveRetornarNull()
        {
            // Arrange
            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync((Arquivo)null!);

            // Act
            var resultado = await _servico.ObterPorId(999);

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DadoArquivoExistenteNaoExcluido_QuandoObterPorId_EntaoDeveMapearTodosOsCampos()
        {
            // Arrange
            var codigo = Guid.NewGuid();
            var arquivo = CriarArquivoValido(id: 10, codigo: codigo, excluido: false);
            var arquivoDTO = new ArquivoDTO
            {
                Id = arquivo.Id,
                Nome = arquivo.Nome,
                Codigo = arquivo.Codigo,
                TipoConteudo = arquivo.TipoConteudo,
                Tipo = arquivo.Tipo,
                Excluido = arquivo.Excluido
            };

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.ObterPorId(10))
                .ReturnsAsync(arquivo);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<ArquivoDTO>(arquivo))
                .Returns(arquivoDTO);

            // Act
            var resultado = await _servico.ObterPorId(10);

            // Assert
            resultado.Id.Should().Be(arquivo.Id);
            resultado.Nome.Should().Be(arquivo.Nome);
            resultado.Codigo.Should().Be(arquivo.Codigo);
            resultado.TipoConteudo.Should().Be(arquivo.TipoConteudo);
            resultado.Tipo.Should().Be(arquivo.Tipo);
        }

        #endregion

        #region Testes de Excluir

        [Fact]
        public async Task DadoArquivoExistente_QuandoExcluir_EntaoDeveRetornarVerdadeiro()
        {
            // Arrange
            var arquivo = CriarArquivoValido(id: 15, excluido: false);
            var arquivoExcluido = CriarArquivoValido(id: 15, excluido: true);
            var arquivoDTORetorno = CriarArquivoDTOValido(id: 15);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.ObterPorId(15))
                .ReturnsAsync(arquivo);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<ArquivoDTO>(arquivo))
                .Returns(arquivoDTORetorno);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.Atualizar(It.IsAny<Arquivo>()))
                .ReturnsAsync(arquivoExcluido);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Arquivo>(It.IsAny<ArquivoDTO>()))
                .Returns(arquivoExcluido);

            // Act
            var resultado = await _servico.Excluir(15);

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact]
        public async Task DadoArquivoParaExcluir_QuandoExcluir_EntaoDeveMarcarComoExcluido()
        {
            // Arrange
            var arquivo = CriarArquivoValido(id: 15, excluido: false);
            var arquivoExcluido = CriarArquivoValido(id: 15, excluido: true);
            var arquivoDTORetorno = CriarArquivoDTOValido(id: 15);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.ObterPorId(15))
                .ReturnsAsync(arquivo);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<ArquivoDTO>(arquivo))
                .Returns(arquivoDTORetorno);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.Atualizar(It.IsAny<Arquivo>()))
                .ReturnsAsync(arquivoExcluido);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Arquivo>(It.IsAny<ArquivoDTO>()))
                .Returns(arquivoExcluido);

            // Act
            await _servico.Excluir(15);

            // Assert
            _mocker.GetMock<IRepositorioArquivo>()
                .Verify(r => r.Atualizar(It.Is<Arquivo>(a =>
                    a.Excluido
                )), Times.Once);
        }

        [Fact]
        public async Task DadoArquivoParaExcluir_QuandoExcluir_EntaoDeveAtualizar()
        {
            // Arrange
            var arquivo = CriarArquivoValido(id: 15, excluido: false);
            var arquivoExcluido = CriarArquivoValido(id: 15, excluido: true);
            var arquivoDTORetorno = CriarArquivoDTOValido(id: 15);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.ObterPorId(15))
                .ReturnsAsync(arquivo);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<ArquivoDTO>(arquivo))
                .Returns(arquivoDTORetorno);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.Atualizar(It.IsAny<Arquivo>()))
                .ReturnsAsync(arquivoExcluido);

            _mocker.GetMock<AutoMapper.IMapper>()
                .Setup(m => m.Map<Arquivo>(It.IsAny<ArquivoDTO>()))
                .Returns(arquivoExcluido);

            // Act
            await _servico.Excluir(15);

            // Assert
            _mocker.GetMock<IRepositorioArquivo>()
                .Verify(r => r.Atualizar(It.IsAny<Arquivo>()), Times.Once);
        }

        #endregion

        #region Métodos Auxiliares

        private ArquivoDTO CriarArquivoDTOValido(long id = 0)
        {
            return new ArquivoDTO
            {
                Id = id,
                Nome = _faker.System.FileName(),
                Codigo = Guid.NewGuid(),
                TipoConteudo = "application/pdf",
                Tipo = TipoArquivo.Temp,
                Excluido = false
            };
        }

        private Arquivo CriarArquivoValido(
            long id = 0,
            Guid? codigo = null,
            string nome = null!,
            string tipoConteudo = null!,
            TipoArquivo tipo = TipoArquivo.Temp,
            DateTime? criadoEm = null,
            string criadoPor = null!,
            string criadoLogin = null!,
            bool excluido = false)
        {
            return new Arquivo
            {
                Id = id,
                Nome = nome ?? _faker.System.FileName(),
                Codigo = codigo ?? Guid.NewGuid(),
                TipoConteudo = tipoConteudo ?? "application/pdf",
                Tipo = tipo,
                CriadoEm = criadoEm ?? DateTime.Now,
                CriadoPor = criadoPor ?? "SistemaTest",
                CriadoLogin = criadoLogin ?? "sistema",
                AlteradoEm = null,
                AlteradoPor = null,
                AlteradoLogin = null,
                Excluido = excluido
            };
        }

        #endregion
    }
}
