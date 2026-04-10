using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase
{
    public class NotificarViaEmailCancelamentoAtendimentoUseCaseTestes
    {
        private readonly Mock<IRepositorioAcervoSolicitacaoItem> repositorioAcervoSolicitacaoItemMock;
        private readonly Mock<IServicoNotificacaoEmail> servicoNotificacaoEmailMock;
        private readonly Mock<IRepositorioParametroSistema> repositorioParametroSistemaMock;
        private readonly NotificarViaEmailCancelamentoAtendimentoUseCaseUseCase sut;

        public NotificarViaEmailCancelamentoAtendimentoUseCaseTestes()
        {
            var mocker = new AutoMocker();

            repositorioAcervoSolicitacaoItemMock = mocker.GetMock<IRepositorioAcervoSolicitacaoItem>();
            servicoNotificacaoEmailMock = mocker.GetMock<IServicoNotificacaoEmail>();
            repositorioParametroSistemaMock = mocker.GetMock<IRepositorioParametroSistema>();

            sut = mocker.CreateInstance<NotificarViaEmailCancelamentoAtendimentoUseCaseUseCase>();

            ConfigurarMocksPadroes();
        }

        [Fact]
        public async Task DadoMensagemRabbitComParametroNulo_QuandoExecutar_EntaoLancaNegocioException()
        {
            // Arrange
            var mensagemRabbit = new MensagemRabbit { Mensagem = null! };

            // Act
            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.PARAMETROS_INVALIDOS);
        }

        [Fact]
        public async Task DadoMensagemRabbitComParametroNaoNumerico_QuandoExecutar_EntaoLancaNegocioException()
        {
            // Arrange
            var mensagemRabbit = new MensagemRabbit { Mensagem = "ID_INVALIDO" };

            // Act
            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.PARAMETROS_INVALIDOS);
        }

        [Fact]
        public async Task DadoSolicitacaoSemItensVinculados_QuandoExecutar_EntaoLancaNegocioException()
        {
            // Arrange
            var acervoSolicitacaoId = 10L;
            var mensagemRabbit = new MensagemRabbit { Mensagem = acervoSolicitacaoId.ToString() };

            repositorioAcervoSolicitacaoItemMock
                .Setup(r => r.ObterDetalhamentoDosItensPorSolicitacaoOuItem(acervoSolicitacaoId, null))
                .ReturnsAsync(new List<AcervoSolicitacaoItemDetalhe>());

            // Act
            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_CONTEM_ACERVOS);
        }

        [Fact]
        public async Task DadoSolicitacaoCujoSolicitanteNaoPossuiEmail_QuandoExecutar_EntaoLancaNegocioException()
        {
            // Arrange
            var acervoSolicitacaoId = 10L;
            var mensagemRabbit = new MensagemRabbit { Mensagem = acervoSolicitacaoId.ToString() };

            var detalhes = new List<AcervoSolicitacaoItemDetalhe>
            {
                new() { Email = null }
            };

            repositorioAcervoSolicitacaoItemMock
                .Setup(r => r.ObterDetalhamentoDosItensPorSolicitacaoOuItem(acervoSolicitacaoId, null))
                .ReturnsAsync(detalhes);

            // Act
            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.SOLICITANTE_NAO_POSSUI_EMAIL);
        }

        [Fact]
        public async Task DadoSolicitacaoValida_QuandoExecutar_EntaoConstroiEnviaEmailERetornaTrue()
        {
            // Arrange
            var acervoSolicitacaoId = 10L;
            var mensagemRabbit = new MensagemRabbit { Mensagem = acervoSolicitacaoId.ToString() };

            var detalhes = new List<AcervoSolicitacaoItemDetalhe>
            {
                new AcervoSolicitacaoItemDetalhe
                {
                    AcervoSolicitacaoId = 10,
                    Id = 1,
                    Solicitante = "João da Silva",
                    Email = "joao@email.com",
                    Titulo = "Livro de História",
                    TipoAcervo = TipoAcervo.Bibliografico,
                    Codigo = "LIV01",
                    CodigoNovo = "NLIV01",
                    DataVisita = new DateTime(2026, 05, 10, 14, 30, 0, DateTimeKind.Local)
                }
            };

            repositorioAcervoSolicitacaoItemMock
                .Setup(r => r.ObterDetalhamentoDosItensPorSolicitacaoOuItem(acervoSolicitacaoId, null))
                .ReturnsAsync(detalhes);

            // Act
            var resultado = await sut.Executar(mensagemRabbit);

            // Assert
            resultado.Should().BeTrue();

            servicoNotificacaoEmailMock.Verify(s => s.Enviar(
                "João da Silva",
                "joao@email.com",
                "CDEP - Atendimento cancelado",
                It.Is<string>(html =>
                    html.Contains("Olá João da Silva") &&
                    html.Contains("http://cdep.com/contato") &&
                    html.Contains("Livro de História") &&
                    html.Contains("Bibliográfico") &&
                    html.Contains("10/05 14:30") &&
                    html.Contains("LIV01/NLIV01")
                )), Times.Once);
        }

        [Fact]
        public async Task DadoItemSemDataDeVisita_QuandoExecutar_EntaoTabelaDeEmailExibeTracoNaData()
        {
            // Arrange
            var acervoSolicitacaoId = 10L;
            var mensagemRabbit = new MensagemRabbit { Mensagem = acervoSolicitacaoId.ToString() };

            var detalhes = new List<AcervoSolicitacaoItemDetalhe>
            {
                new AcervoSolicitacaoItemDetalhe
                {
                    AcervoSolicitacaoId = 10,
                    Id = 2,
                    Solicitante = "Maria Sousa",
                    Email = "maria@email.com",
                    Titulo = "Peça de Museu",
                    TipoAcervo = TipoAcervo.Tridimensional,
                    Codigo = "TRID01",
                    DataVisita = null // Data Nula para forçar o traço na extensão
                }
            };

            repositorioAcervoSolicitacaoItemMock
                .Setup(r => r.ObterDetalhamentoDosItensPorSolicitacaoOuItem(acervoSolicitacaoId, null))
                .ReturnsAsync(detalhes);

            // Act
            var resultado = await sut.Executar(mensagemRabbit);

            // Assert
            resultado.Should().BeTrue();

            servicoNotificacaoEmailMock.Verify(s => s.Enviar(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.Is<string>(html => html.Contains("<td>-</td>"))
            ), Times.Once);
        }

        // ================= MÉTODOS PRIVADOS AUXILIARES ================= //

        private void ConfigurarMocksPadroes()
        {
            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.ModeloEmailCancelamentoSolicitacao, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "<html>Olá #NOME, seu pedido foi cancelado. Resumo: #CONTEUDO_TABELA Dúvidas? #LINK_FORMULARIO_CDEP</html>" });

            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "http://cdep.com/contato" });

            servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
        }
    }
}