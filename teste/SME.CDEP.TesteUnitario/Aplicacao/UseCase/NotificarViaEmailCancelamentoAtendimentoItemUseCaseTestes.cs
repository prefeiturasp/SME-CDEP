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
    public class NotificarViaEmailCancelamentoAtendimentoItemUseCaseTestes
    {
        private readonly Mock<IRepositorioAcervoSolicitacaoItem> repositorioAcervoSolicitacaoItemMock;
        private readonly Mock<IServicoNotificacaoEmail> servicoNotificacaoEmailMock;
        private readonly Mock<IRepositorioParametroSistema> repositorioParametroSistemaMock;
        private readonly NotificarViaEmailCancelamentoAtendimentoItemUseCaseUseCase sut;

        public NotificarViaEmailCancelamentoAtendimentoItemUseCaseTestes()
        {
            var mocker = new AutoMocker();

            repositorioAcervoSolicitacaoItemMock = mocker.GetMock<IRepositorioAcervoSolicitacaoItem>();
            servicoNotificacaoEmailMock = mocker.GetMock<IServicoNotificacaoEmail>();
            repositorioParametroSistemaMock = mocker.GetMock<IRepositorioParametroSistema>();

            sut = mocker.CreateInstance<NotificarViaEmailCancelamentoAtendimentoItemUseCaseUseCase>();

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
        public async Task DadoSolicitacaoVazia_QuandoExecutar_EntaoLancaNegocioException()
        {
            // Arrange
            var acervoSolicitacaoItemId = 10L;
            var mensagemRabbit = new MensagemRabbit { Mensagem = acervoSolicitacaoItemId.ToString() };

            repositorioAcervoSolicitacaoItemMock
                .Setup(r => r.ObterDetalhamentoDosItensPorSolicitacaoOuItem(null, acervoSolicitacaoItemId))
                .ReturnsAsync(new List<AcervoSolicitacaoItemDetalhe>());

            // Act
            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_CONTEM_ACERVOS);
        }

        [Fact]
        public async Task DadoItemCujoSolicitanteNaoPossuiEmail_QuandoExecutar_EntaoLancaNegocioException()
        {
            // Arrange
            var acervoSolicitacaoItemId = 10L;
            var mensagemRabbit = new MensagemRabbit { Mensagem = acervoSolicitacaoItemId.ToString() };

            var detalhes = new List<AcervoSolicitacaoItemDetalhe>
            {
                new AcervoSolicitacaoItemDetalhe { Email = "   " } // Testando string.IsNullOrWhiteSpace
            };

            repositorioAcervoSolicitacaoItemMock
                .Setup(r => r.ObterDetalhamentoDosItensPorSolicitacaoOuItem(null, acervoSolicitacaoItemId))
                .ReturnsAsync(detalhes);

            // Act
            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.SOLICITANTE_NAO_POSSUI_EMAIL);
        }

        [Fact]
        public async Task DadoItemCanceladoValido_QuandoExecutar_EntaoConstroiEnviaEmailERetornaTrue()
        {
            // Arrange
            var acervoSolicitacaoItemId = 1L;
            var mensagemRabbit = new MensagemRabbit { Mensagem = acervoSolicitacaoItemId.ToString() };

            var detalhes = new List<AcervoSolicitacaoItemDetalhe>
            {
                new AcervoSolicitacaoItemDetalhe
                {
                    AcervoSolicitacaoId = 10,
                    Id = acervoSolicitacaoItemId,
                    Solicitante = "Carlos Almeida",
                    Email = "carlos@email.com",
                    Titulo = "Fotografia Antiga",
                    TipoAcervo = TipoAcervo.Fotografico,
                    Codigo = "FOTO01",
                    CodigoNovo = "NFOTO01",
                    DataVisita = new DateTime(2026, 08, 15, 10, 0, 0)
                }
            };

            repositorioAcervoSolicitacaoItemMock
                .Setup(r => r.ObterDetalhamentoDosItensPorSolicitacaoOuItem(null, acervoSolicitacaoItemId))
                .ReturnsAsync(detalhes);

            // Act
            var resultado = await sut.Executar(mensagemRabbit);

            // Assert
            resultado.Should().BeTrue();

            servicoNotificacaoEmailMock.Verify(s => s.Enviar(
                "Carlos Almeida",
                "carlos@email.com",
                "CDEP - Atendimento item cancelado",
                It.Is<string>(html =>
                    html.Contains("Olá Carlos Almeida") &&
                    html.Contains("http://cdep.com/contato") &&
                    html.Contains("Fotografia Antiga") &&
                    html.Contains("Fotográfico") &&
                    html.Contains("15/08 10:00") &&
                    html.Contains("FOTO01/NFOTO01")
                )), Times.Once);
        }

        [Fact]
        public async Task DadoItemSemDataDeVisita_QuandoExecutar_EntaoTabelaDeEmailExibeTracoNaData()
        {
            // Arrange
            var acervoSolicitacaoItemId = 2L;
            var mensagemRabbit = new MensagemRabbit { Mensagem = acervoSolicitacaoItemId.ToString() };

            var detalhes = new List<AcervoSolicitacaoItemDetalhe>
            {
                new AcervoSolicitacaoItemDetalhe
                {
                    AcervoSolicitacaoId = 10,
                    Id = acervoSolicitacaoItemId,
                    Solicitante = "Ana Lima",
                    Email = "ana@email.com",
                    Titulo = "Peça de Museu",
                    TipoAcervo = TipoAcervo.Tridimensional,
                    Codigo = "TRID01",
                    DataVisita = null // Data Nula para forçar o traço
                }
            };

            repositorioAcervoSolicitacaoItemMock
                .Setup(r => r.ObterDetalhamentoDosItensPorSolicitacaoOuItem(null, acervoSolicitacaoItemId))
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
                .Setup(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.ModeloEmailCancelamentoSolicitacaoItem, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "<html>Olá #NOME, seu item foi cancelado. Resumo: #CONTEUDO_TABELA Dúvidas? #LINK_FORMULARIO_CDEP</html>" });

            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "http://cdep.com/contato" });

            servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
        }
    }
}