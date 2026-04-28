using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase
{
    public class NotificarViaEmailConfirmacaoAtendimentoPresencialUseCaseTestes
    {
        private readonly Mock<IRepositorioAcervoSolicitacaoItem> repositorioAcervoSolicitacaoItemMock;
        private readonly Mock<IServicoNotificacaoEmail> servicoNotificacaoEmailMock;
        private readonly Mock<IRepositorioParametroSistema> repositorioParametroSistemaMock;
        private readonly NotificarViaEmailConfirmacaoAtendimentoPresencialUseCaseUseCase sut;

        public NotificarViaEmailConfirmacaoAtendimentoPresencialUseCaseTestes()
        {
            var mocker = new AutoMocker();

            repositorioAcervoSolicitacaoItemMock = mocker.GetMock<IRepositorioAcervoSolicitacaoItem>();
            servicoNotificacaoEmailMock = mocker.GetMock<IServicoNotificacaoEmail>();
            repositorioParametroSistemaMock = mocker.GetMock<IRepositorioParametroSistema>();

            sut = mocker.CreateInstance<NotificarViaEmailConfirmacaoAtendimentoPresencialUseCaseUseCase>();

            ConfigurarMocksPadroes();
        }

        [Fact]
        public async Task DadoMensagemRabbitComDtoNulo_QuandoExecutar_EntaoLancaNegocioException()
        {
            // Arrange
            var mensagemRabbit = new MensagemRabbit { Mensagem = "null" };

            // Act
            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.PARAMETROS_INVALIDOS);
        }

        [Fact]
        public async Task DadoMensagemRabbitComIdInvalido_QuandoExecutar_EntaoLancaNegocioException()
        {
            // Arrange
            var dtoInvalido = new ConfirmarAtendimentoDTO { Id = -1, ItemId = 0 };
            var mensagemRabbit = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(dtoInvalido) };

            // Act
            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.PARAMETROS_INVALIDOS);
        }

        [Fact]
        public async Task DadoSolicitanteSemEmail_QuandoExecutar_EntaoLancaNegocioException()
        {
            // Arrange
            var dto = new ConfirmarAtendimentoDTO { Id = 10L, ItemId = 10 };
            var mensagemRabbit = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(dto) };

            var detalhes = new List<AcervoSolicitacaoItemDetalhe>
            {
                new() { Email = string.Empty }
            };

            repositorioAcervoSolicitacaoItemMock
                .Setup(r => r.ObterDetalhamentoDosItensPorSolicitacaoOuItem(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(detalhes);

            // Act
            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.SOLICITANTE_NAO_POSSUI_EMAIL);
        }

        [Fact]
        public async Task DadoSolicitacaoVazia_QuandoExecutar_EntaoLancaNegocioException()
        {
            // Arrange
            var dto = new ConfirmarAtendimentoDTO { Id = 10L, ItemId = 10 };
            var mensagemRabbit = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(dto) };

            repositorioAcervoSolicitacaoItemMock
                .Setup(r => r.ObterDetalhamentoDosItensPorSolicitacaoOuItem(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(new List<AcervoSolicitacaoItemDetalhe>());

            // Act
            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_CONTEM_ACERVOS);
        }

        [Fact]
        public async Task DadoSolicitacaoSemAtendimentoPresencial_QuandoExecutar_EntaoLancaNegocioException()
        {
            // Arrange
            var dto = new ConfirmarAtendimentoDTO { Id = 10L, ItemId = 10 };
            var mensagemRabbit = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(dto) };

            var detalhes = new List<AcervoSolicitacaoItemDetalhe>
            {
                new AcervoSolicitacaoItemDetalhe
                {
                    Email = "teste@teste.com",
                    TipoAtendimento = TipoAtendimento.Email
                }
            };

            repositorioAcervoSolicitacaoItemMock
                .Setup(r => r.ObterDetalhamentoDosItensPorSolicitacaoOuItem(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(detalhes);

            // Act
            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_CONTEM_ACERVOS);
        }

        [Fact]
        public async Task DadoSolicitacaoPresencialValida_QuandoExecutar_EntaoEnviaEmailComParametrosSubstituidosERetornaTrue()
        {
            // Arrange
            var dto = new ConfirmarAtendimentoDTO { Id = 10L, ItemId = 10 };
            var mensagemRabbit = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(dto) };

            var detalhes = new List<AcervoSolicitacaoItemDetalhe>
            {
                new AcervoSolicitacaoItemDetalhe
                {
                    AcervoSolicitacaoId = 10,
                    Id = 1,
                    Solicitante = "Maria Oliveira",
                    Email = "maria@email.com",
                    Titulo = "Documento Histórico",
                    TipoAcervo = TipoAcervo.DocumentacaoTextual,
                    TipoAtendimento = TipoAtendimento.Presencial,
                    Codigo = "DOC01",
                    DataVisita = new DateTime(2026, 12, 01, 10, 0, 0, DateTimeKind.Local)
                }
            };

            repositorioAcervoSolicitacaoItemMock
                .Setup(r => r.ObterDetalhamentoDosItensPorSolicitacaoOuItem(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(detalhes);

            // Act
            var resultado = await sut.Executar(mensagemRabbit);

            // Assert
            resultado.Should().BeTrue();

            servicoNotificacaoEmailMock.Verify(s => s.Enviar(
                "Maria Oliveira",
                "maria@email.com",
                "CDEP - Confirmação de Atendimento",
                It.Is<string>(html =>
                    html.Contains("Olá Maria Oliveira") &&
                    html.Contains("http://cdep.com/contato") &&
                    html.Contains("Rua Sede CDEP, 123") &&
                    html.Contains("08:00 as 17:00") &&
                    html.Contains("Documento Histórico") &&
                    html.Contains("01/12 10:00") &&
                    html.Contains("DOC01")
                )), Times.Once);
        }

        // ================= MÉTODOS PRIVADOS AUXILIARES ================= //

        private void ConfigurarMocksPadroes()
        {
            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.ModeloEmailConfirmacaoSolicitacao, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "Olá #NOME. Resumo: #CONTEUDO_TABELA. Contato: #LINK_FORMULARIO_CDEP. Endereço: #ENDERECO_SEDE_CDEP_VISITA. Horário: #HORARIO_FUNCIONAMENTO_SEDE_CDEP" });

            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "http://cdep.com/contato" });

            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.EnderecoSedeCDEPVisita, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "Rua Sede CDEP, 123" });

            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.HorarioFuncionamentoSedeCDEPVisita, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "08:00 as 17:00" });

            servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
        }
    }
}