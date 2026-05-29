using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Excecoes;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoPerfilUsuarioTeste
    {
        private readonly AutoMocker _mocker;
        private readonly ServicoPerfilUsuario _sut;

        public ServicoPerfilUsuarioTeste()
        {
            _mocker = new AutoMocker();
            _sut = _mocker.CreateInstance<ServicoPerfilUsuario>();
        }

        #region Testes ObterPerfisUsuario - Caso de Sucesso

        [Fact]
        public async Task DadoLoginValidoComPerfisExistentes_QuandoObterPerfisUsuario_EntaoRetornaRetornoPerfilUsuarioDTOPreenchido()
        {
            // Arrange
            const string login = "usuario.teste";
            var perfilRetorno = CriarRetornoPerfilUsuarioDTOValido(login: login);

            _mocker.GetMock<IServicoAcessos>()
                .Setup(s => s.ObterPerfisUsuario(login))
                .ReturnsAsync(perfilRetorno);

            // Act
            var resultado = await _sut.ObterPerfisUsuario(login);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<RetornoPerfilUsuarioDTO>();
            resultado.UsuarioLogin.Should().Be(login);
            resultado.Autenticado.Should().BeTrue();
            resultado.PerfilUsuario.Should().NotBeNull();

            _mocker.GetMock<IServicoAcessos>()
                .Verify(s => s.ObterPerfisUsuario(login), Times.Once);
        }

        [Fact]
        public async Task DadoLoginValidoComPerfisExistentes_QuandoObterPerfisUsuario_EntaoNaoVinculaPerfilExterno()
        {
            // Arrange
            const string login = "usuario.teste";
            var perfilRetorno = CriarRetornoPerfilUsuarioDTOValido(login: login);

            _mocker.GetMock<IServicoAcessos>()
                .Setup(s => s.ObterPerfisUsuario(login))
                .ReturnsAsync(perfilRetorno);

            // Act
            await _sut.ObterPerfisUsuario(login);

            // Assert
            _mocker.GetMock<IServicoAcessos>()
                .Verify(s => s.VincularPerfilExternoCoreSSO(It.IsAny<string>(), It.IsAny<Guid>()), Times.Never);
        }

        #endregion

        #region Testes ObterPerfisUsuario - Sem Perfis

        [Fact]
        public async Task DadoLoginSemPerfisVinculados_QuandoObterPerfisUsuario_EntaoVinculaPerfilExternoERetornaRetorno()
        {
            // Arrange
            const string login = "usuario.sem.perfil";
            var perfilRetornoSemPerfis = CriarRetornoPerfilUsuarioDTOSemPerfis(login: login);
            var perfilRetornoComPerfis = CriarRetornoPerfilUsuarioDTOValido(login: login);
            var perfilExternoGuid = new Guid(Constantes.PERFIL_EXTERNO_GUID);

            _mocker.GetMock<IServicoAcessos>()
                .SetupSequence(s => s.ObterPerfisUsuario(login))
                .ReturnsAsync(perfilRetornoSemPerfis)
                .ReturnsAsync(perfilRetornoComPerfis);

            _mocker.GetMock<IServicoAcessos>()
                .Setup(s => s.VincularPerfilExternoCoreSSO(login, perfilExternoGuid))
                .ReturnsAsync(true);

            // Act
            var resultado = await _sut.ObterPerfisUsuario(login);

            // Assert
            resultado.Should().NotBeNull();
            resultado.PerfilUsuario.Should().NotBeEmpty();
            resultado.UsuarioLogin.Should().Be(login);

            _mocker.GetMock<IServicoAcessos>()
                .Verify(s => s.VincularPerfilExternoCoreSSO(login, perfilExternoGuid), Times.Once);

            _mocker.GetMock<IServicoAcessos>()
                .Verify(s => s.ObterPerfisUsuario(login), Times.Exactly(2));
        }

        [Fact]
        public async Task DadoLoginSemPerfisVinculados_QuandoObterPerfisUsuario_EntaoVinculaPerfilExternoComGuidCorreto()
        {
            // Arrange
            const string login = "usuario.sem.perfil";
            var perfilRetornoSemPerfis = CriarRetornoPerfilUsuarioDTOSemPerfis(login: login);
            var perfilRetornoComPerfis = CriarRetornoPerfilUsuarioDTOValido(login: login);
            var perfilExternoGuid = new Guid(Constantes.PERFIL_EXTERNO_GUID);

            _mocker.GetMock<IServicoAcessos>()
                .SetupSequence(s => s.ObterPerfisUsuario(login))
                .ReturnsAsync(perfilRetornoSemPerfis)
                .ReturnsAsync(perfilRetornoComPerfis);

            _mocker.GetMock<IServicoAcessos>()
                .Setup(s => s.VincularPerfilExternoCoreSSO(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(true);

            // Act
            await _sut.ObterPerfisUsuario(login);

            // Assert
            _mocker.GetMock<IServicoAcessos>()
                .Verify(s => s.VincularPerfilExternoCoreSSO(login, perfilExternoGuid), Times.Once);
        }

        #endregion

        #region Testes ObterPerfisUsuario - Vinculação Falha

        [Fact]
        public async Task DadoLoginSemPerfisEVinculacaoFalha_QuandoObterPerfisUsuario_EntaoLancaNegocioException()
        {
            // Arrange
            const string login = "usuario.sem.perfil";
            var perfilRetornoSemPerfis = CriarRetornoPerfilUsuarioDTOSemPerfis(login: login);
            var perfilRetornoComPerfisNulo = new RetornoPerfilUsuarioDTO
            {
                UsuarioLogin = login,
                UsuarioNome = "Usuario Teste",
                Autenticado = true,
                Email = "teste@teste.com",
                Token = Guid.NewGuid().ToString(),
                PerfilUsuario = null! // Nulo, não apenas vazio
            };
            var perfilExternoGuid = new Guid(Constantes.PERFIL_EXTERNO_GUID);

            _mocker.GetMock<IServicoAcessos>()
                .SetupSequence(s => s.ObterPerfisUsuario(login))
                .ReturnsAsync(perfilRetornoSemPerfis)
                .ReturnsAsync(perfilRetornoComPerfisNulo);

            _mocker.GetMock<IServicoAcessos>()
                .Setup(s => s.VincularPerfilExternoCoreSSO(login, perfilExternoGuid))
                .ReturnsAsync(true);

            // Act
            Func<Task> acao = async () => await _sut.ObterPerfisUsuario(login);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.NAO_FOI_POSSIVEL_VINCULAR_PERFIL_EXTERNO_CORESSO_USUARIO_SEM_PERFIL);
        }

        [Fact]
        public async Task DadoLoginSemPerfisEPerfilesNulo_QuandoObterPerfisUsuario_EntaoLancaNegocioException()
        {
            // Arrange
            const string login = "usuario.sem.perfil";
            var perfilRetornoSemPerfis = CriarRetornoPerfilUsuarioDTOSemPerfis(login: login);
            var perfilRetornoComPerfisNulo = new RetornoPerfilUsuarioDTO
            {
                UsuarioLogin = login,
                UsuarioNome = "Usuario Teste",
                Autenticado = true,
                Email = "teste@teste.com",
                Token = Guid.NewGuid().ToString(),
                PerfilUsuario = null!
            };
            var perfilExternoGuid = new Guid(Constantes.PERFIL_EXTERNO_GUID);

            _mocker.GetMock<IServicoAcessos>()
                .SetupSequence(s => s.ObterPerfisUsuario(login))
                .ReturnsAsync(perfilRetornoSemPerfis)
                .ReturnsAsync(perfilRetornoComPerfisNulo);

            _mocker.GetMock<IServicoAcessos>()
                .Setup(s => s.VincularPerfilExternoCoreSSO(login, perfilExternoGuid))
                .ReturnsAsync(false); 

            // Act
            Func<Task> acao = async () => await _sut.ObterPerfisUsuario(login);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.NAO_FOI_POSSIVEL_VINCULAR_PERFIL_EXTERNO_CORESSO_USUARIO_SEM_PERFIL);
        }

        #endregion

        #region Testes VincularPerfilExternoCoreSSO

        [Fact]
        public async Task DadoLoginEPerfilValidosParaVincular_QuandoVincularPerfilExternoCoreSSO_EntaoRetornaVerdadeiro()
        {
            // Arrange
            const string login = "usuario.teste";
            var perfilId = new Guid(Constantes.PERFIL_EXTERNO_GUID);

            _mocker.GetMock<IServicoAcessos>()
                .Setup(s => s.VincularPerfilExternoCoreSSO(login, perfilId))
                .ReturnsAsync(true);

            // Act
            var resultado = await _sut.VincularPerfilExternoCoreSSO(login, perfilId);

            // Assert
            resultado.Should().BeTrue();

            _mocker.GetMock<IServicoAcessos>()
                .Verify(s => s.VincularPerfilExternoCoreSSO(login, perfilId), Times.Once);
        }

        [Fact]
        public async Task DadoLoginEPerfilValidosParaVincular_QuandoVincularPerfilExternoCoreSSO_EntaoRetornaFalso()
        {
            // Arrange
            const string login = "usuario.teste";
            var perfilId = Guid.NewGuid();

            _mocker.GetMock<IServicoAcessos>()
                .Setup(s => s.VincularPerfilExternoCoreSSO(login, perfilId))
                .ReturnsAsync(false);

            // Act
            var resultado = await _sut.VincularPerfilExternoCoreSSO(login, perfilId);

            // Assert
            resultado.Should().BeFalse();

            _mocker.GetMock<IServicoAcessos>()
                .Verify(s => s.VincularPerfilExternoCoreSSO(login, perfilId), Times.Once);
        }

        [Fact]
        public async Task DadoLoginEPerfilValidos_QuandoVincularPerfilExternoCoreSSO_EntaoPassaParametrosCorretos()
        {
            // Arrange
            const string login = "usuario.teste";
            var perfilId = Guid.NewGuid();

            _mocker.GetMock<IServicoAcessos>()
                .Setup(s => s.VincularPerfilExternoCoreSSO(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(true);

            // Act
            await _sut.VincularPerfilExternoCoreSSO(login, perfilId);

            // Assert
            _mocker.GetMock<IServicoAcessos>()
                .Verify(s => s.VincularPerfilExternoCoreSSO(login, perfilId), Times.Once);
        }

        #endregion

        #region Testes de Construtores e Validações

        [Fact]
        public void DadoServicoAcessosNulo_QuandoConstruir_EntaoLancaArgumentNullException()
        {
            // Act
            Action acao = () => _ = new ServicoPerfilUsuario(null!);

            // Assert
            acao.Should().Throw<ArgumentNullException>()
                .WithMessage("*servicoAcessos*");
        }

        [Fact]
        public void DadoServicoAcessosValido_QuandoConstruir_EntaoInstanciaComSucesso()
        {
            // Act
            var mockServicoAcessos = new Mock<IServicoAcessos>();
            var servico = new ServicoPerfilUsuario(mockServicoAcessos.Object);

            // Assert
            servico.Should().NotBeNull();
            servico.Should().BeOfType<ServicoPerfilUsuario>();
        }

        #endregion

        #region Testes de Integração de Fluxo

        [Fact]
        public async Task DadoFluxoCompletoDeObterPerfisComVinculacao_QuandoObterPerfisUsuario_EntaoExecutaTodosOsPaisos()
        {
            // Arrange
            const string login = "usuario.novo";
            var perfilRetornoSemPerfis = CriarRetornoPerfilUsuarioDTOSemPerfis(login: login);
            var perfilRetornoComPerfis = CriarRetornoPerfilUsuarioDTOValido(login: login);
            var perfilExternoGuid = new Guid(Constantes.PERFIL_EXTERNO_GUID);

            var sequenciaOrdenada = new List<string>();

            _mocker.GetMock<IServicoAcessos>()
                .Setup(s => s.ObterPerfisUsuario(login))
                .Returns((string l) =>
                {
                    sequenciaOrdenada.Add("ObterPerfis");
                    return sequenciaOrdenada.Count == 1 ? Task.FromResult(perfilRetornoSemPerfis) : Task.FromResult(perfilRetornoComPerfis);
                });

            _mocker.GetMock<IServicoAcessos>()
                .Setup(s => s.VincularPerfilExternoCoreSSO(login, perfilExternoGuid))
                .Callback(() => sequenciaOrdenada.Add("VincularPerfil"))
                .ReturnsAsync(true);

            // Act
            var resultado = await _sut.ObterPerfisUsuario(login);

            // Assert
            resultado.PerfilUsuario.Should().NotBeEmpty();
            sequenciaOrdenada.Should().HaveCount(3);
            sequenciaOrdenada[0].Should().Be("ObterPerfis");
            sequenciaOrdenada[1].Should().Be("VincularPerfil");
            sequenciaOrdenada[2].Should().Be("ObterPerfis");
        }

        #endregion

        #region Testes de Casos Extremos

        [Fact]
        public async Task DadoLoginComCaracteresEspeciais_QuandoObterPerfisUsuario_EntaoProcessaCorretamente()
        {
            // Arrange
            const string login = "usuario.teste+especial@dominio.com";
            var perfilRetorno = CriarRetornoPerfilUsuarioDTOValido(login: login);

            _mocker.GetMock<IServicoAcessos>()
                .Setup(s => s.ObterPerfisUsuario(login))
                .ReturnsAsync(perfilRetorno);

            // Act
            var resultado = await _sut.ObterPerfisUsuario(login);

            // Assert
            resultado.Should().NotBeNull();
            resultado.UsuarioLogin.Should().Be(login);
        }

        [Fact]
        public async Task DadoMultiplosPerfisPorUsuario_QuandoObterPerfisUsuario_EntaoRetornaTodosOsPerfis()
        {
            // Arrange
            const string login = "usuario.multi.perfil";
            var perfilRetorno = CriarRetornoPerfilUsuarioDTOComMultiplosPerfis(login: login, quantidadePerfis: 3);

            _mocker.GetMock<IServicoAcessos>()
                .Setup(s => s.ObterPerfisUsuario(login))
                .ReturnsAsync(perfilRetorno);

            // Act
            var resultado = await _sut.ObterPerfisUsuario(login);

            // Assert
            resultado.PerfilUsuario.Should().HaveCount(3);
        }

        #endregion

        #region Métodos Auxiliares

        private static RetornoPerfilUsuarioDTO CriarRetornoPerfilUsuarioDTOValido(string login = "usuario.teste")
        {
            return new RetornoPerfilUsuarioDTO
            {
                UsuarioLogin = login,
                UsuarioNome = "Usuario Teste",
                Autenticado = true,
                Email = "teste@teste.com",
                Token = Guid.NewGuid().ToString(),
                DataHoraExpiracao = DateTime.UtcNow.AddMinutes(20),
                PerfilUsuario =
                [
                    new()
                    {
                        Perfil = new Guid(Constantes.PERFIL_EXTERNO_GUID),
                        PerfilNome = "Perfil Externo"
                    }
                ]
            };
        }

        private static RetornoPerfilUsuarioDTO CriarRetornoPerfilUsuarioDTOSemPerfis(string login = "usuario.teste")
        {
            return new RetornoPerfilUsuarioDTO
            {
                UsuarioLogin = login,
                UsuarioNome = "Usuario Teste",
                Autenticado = true,
                Email = "teste@teste.com",
                Token = Guid.NewGuid().ToString(),
                DataHoraExpiracao = DateTime.UtcNow.AddMinutes(20),
                PerfilUsuario = [] // Lista vazia
            };
        }

        private static RetornoPerfilUsuarioDTO CriarRetornoPerfilUsuarioDTOComMultiplosPerfis(string login = "usuario.teste", int quantidadePerfis = 1)
        {
            var perfis = new List<PerfilUsuarioDTO>();

            for (int i = 0; i < quantidadePerfis; i++)
            {
                perfis.Add(new PerfilUsuarioDTO
                {
                    Perfil = Guid.NewGuid(),
                    PerfilNome = $"Perfil {i + 1}"
                });
            }

            return new RetornoPerfilUsuarioDTO
            {
                UsuarioLogin = login,
                UsuarioNome = "Usuario Teste",
                Autenticado = true,
                Email = "teste@teste.com",
                Token = Guid.NewGuid().ToString(),
                DataHoraExpiracao = DateTime.UtcNow.AddMinutes(20),
                PerfilUsuario = perfis
            };
        }

        #endregion
    }
}
