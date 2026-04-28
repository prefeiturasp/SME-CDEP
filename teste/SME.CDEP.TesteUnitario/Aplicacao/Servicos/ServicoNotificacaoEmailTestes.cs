using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoNotificacaoEmailTestes
    {
        private readonly Mock<IRepositorioParametroSistema> repositorioParametroSistemaMock;
        private readonly ServicoNotificacaoEmail sut;

        public ServicoNotificacaoEmailTestes()
        {
            var mocker = new AutoMocker();

            repositorioParametroSistemaMock = mocker.GetMock<IRepositorioParametroSistema>();

            sut = mocker.CreateInstance<ServicoNotificacaoEmail>();
        }

        [Fact]
        public async Task DadoParametrosValidos_QuandoEnviar_EntaoBuscaParametrosETentaConectarLancandoExcecaoDeRede()
        {
            // Arrange
            ConfigurarMocksPadroes();
            var nomeDestinatario = "Usuário Teste";
            var emailDestinatario = "teste@teste.com.br";
            var assunto = "Assunto Teste";
            var mensagem = "<p>Mensagem Teste</p>";

            // Act
            Func<Task> acao = async () => await sut.Enviar(nomeDestinatario, emailDestinatario, assunto, mensagem);

            // Assert
            await acao.Should().ThrowAsync<Exception>();

            repositorioParametroSistemaMock.Verify(r => r.ObterParametroPorTipoEAno(TipoParametroSistema.EmailRemetente, It.IsAny<int>()), Times.Once);
            repositorioParametroSistemaMock.Verify(r => r.ObterParametroPorTipoEAno(TipoParametroSistema.EnderecoSMTP, It.IsAny<int>()), Times.Once);
            repositorioParametroSistemaMock.Verify(r => r.ObterParametroPorTipoEAno(TipoParametroSistema.UsuarioRemetenteEmail, It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DadoParametroPortaInvalido_QuandoEnviar_EntaoLancaFormatException()
        {
            // Arrange
            ConfigurarMocksPadroes();
            MockarParametro(TipoParametroSistema.PortaEnvioEmail, "PORTA_INVALIDA");

            // Act
            Func<Task> acao = async () => await sut.Enviar("Destinatario", "dest@teste.com", "Assunto", "Mensagem");

            // Assert
            await acao.Should().ThrowAsync<FormatException>();
        }

        [Fact]
        public async Task DadoParametroUsarTlsInvalido_QuandoEnviar_EntaoLancaFormatException()
        {
            // Arrange
            ConfigurarMocksPadroes();
            MockarParametro(TipoParametroSistema.UsarTLSEmail, "VALOR_BOOLEANO_INVALIDO");

            // Act
            Func<Task> acao = async () => await sut.Enviar("Destinatario", "dest@teste.com", "Assunto", "Mensagem");

            // Assert
            await acao.Should().ThrowAsync<FormatException>();
        }

        // ================= MÉTODOS PRIVADOS AUXILIARES ================= //

        private void ConfigurarMocksPadroes()
        {
            MockarParametro(TipoParametroSistema.EmailRemetente, "remetente@sistema.com.br");
            MockarParametro(TipoParametroSistema.NomeRemetenteEmail, "Sistema CDEP");
            MockarParametro(TipoParametroSistema.EnderecoSMTP, "smtp.servidor-ficticio.local");
            MockarParametro(TipoParametroSistema.UsuarioRemetenteEmail, "usuario_smtp");
            MockarParametro(TipoParametroSistema.SenhaRemetenteEmail, "senha_super_secreta");
            MockarParametro(TipoParametroSistema.UsarTLSEmail, "true");
            MockarParametro(TipoParametroSistema.PortaEnvioEmail, "587");
        }

        private void MockarParametro(TipoParametroSistema tipo, string valorMockado)
        {
            repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(tipo, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = valorMockado });
        }
    }
}