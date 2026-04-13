using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoMenuTestes
    {
        private readonly Mock<IServicoUsuario> servicoUsuarioMock;
        private readonly ServicoMenu sut;

        public ServicoMenuTestes()
        {
            var mocker = new AutoMocker();

            servicoUsuarioMock = mocker.GetMock<IServicoUsuario>();

            sut = mocker.CreateInstance<ServicoMenu>();
        }

        [Fact]
        public void DadoDependenciasValidas_QuandoInstanciarServico_EntaoRetornaInstanciaComSucesso()
        {
            // Arrange
            Action acao = () => new ServicoMenu(servicoUsuarioMock.Object);

            // Act & Assert
            acao.Should().NotThrow();
            sut.Should().NotBeNull();
        }

        [Fact]
        public void DadoServicoUsuarioNulo_QuandoInstanciarServico_EntaoLancaArgumentNullException()
        {
            // Arrange
            Action acao = () => new ServicoMenu(null!);

            // Act & Assert
            acao.Should().Throw<ArgumentNullException>().WithParameterName("servicoUsuario");
        }

        [Fact]
        public async Task DadoUsuarioSemPermissoes_QuandoObterMenu_EntaoRetornaListaVazia()
        {
            // Arrange
            servicoUsuarioMock
                .Setup(s => s.ObterPermissoes())
                .Returns([]);

            // Act
            var resultado = await sut.ObterMenu();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
            servicoUsuarioMock.Verify(s => s.ObterPermissoes(), Times.Once);
        }

        [Fact]
        public async Task DadoPermissoesInvalidasForaDoEnum_QuandoObterMenu_EntaoFiltraIgnorandoAsInvalidas()
        {
            // Arrange
            var permissaoInvalida = (Permissao)9999;

            var permissoes = new List<Permissao>
            {
                permissaoInvalida
            };

            servicoUsuarioMock
                .Setup(s => s.ObterPermissoes())
                .Returns(permissoes);

            // Act
            var resultado = await sut.ObterMenu();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
            servicoUsuarioMock.Verify(s => s.ObterPermissoes(), Times.Once);
        }
    }
}