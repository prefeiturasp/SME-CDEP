using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;
using Xunit;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase
{
    public class NotificacaoVencimentoEmprestimoUsuarioUseCaseTeste
    {
        private readonly Mock<IRepositorioParametroSistema> repositorioParametroSistemaMock;
        private readonly Mock<IServicoNotificacaoEmail> servicoNotificacaoEmailMock;
        private readonly NotificacaoVencimentoEmprestimoUsuarioUseCase sut;

        public NotificacaoVencimentoEmprestimoUsuarioUseCaseTeste()
        {
            var mocker = new AutoMocker();

            repositorioParametroSistemaMock = mocker.GetMock<IRepositorioParametroSistema>();
            servicoNotificacaoEmailMock = mocker.GetMock<IServicoNotificacaoEmail>();

            sut = mocker.CreateInstance<NotificacaoVencimentoEmprestimoUsuarioUseCase>();
        }

        [Fact]
        public void DadoDependenciasValidas_QuandoInstanciarUseCase_EntaoRetornaInstanciaComSucesso()
        {
            Action acao = () => new NotificacaoVencimentoEmprestimoUsuarioUseCase(
                repositorioParametroSistemaMock.Object,
                servicoNotificacaoEmailMock.Object);

            acao.Should().NotThrow();
            sut.Should().NotBeNull();
        }

        [Fact]
        public async Task DadoMensagemRabbitComParametroNulo_QuandoExecutar_EntaoLancaNegocioException()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = null! };
            
            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            await acao.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task DadoMensagemRabbitComDadosValidos_QuandoExecutar_EntaoEnviaEmailComSucesso()
        {
            var dataDevolucao = DateTime.Now.AddDays(5);
            var dataEmprestimo = DateTime.Now.AddDays(-10);

            var acervoEmprestimoDevolucao = new AcervoEmprestimoDevolucao
            {
                AcervoSolicitacaoId = 1,
                AcervoSolicitacaoItemId = 1,
                Solicitante = "João Silva",
                Email = "joao.silva@example.com",
                Titulo = "Livro Test",
                Codigo = "LV-001",
                DataEmprestimo = dataEmprestimo,
                DataDevolucao = dataDevolucao
            };

            var mensagemJson = JsonConvert.SerializeObject(acervoEmprestimoDevolucao);
            var mensagemRabbit = new MensagemRabbit { Mensagem = mensagemJson };

            ConfigurarMocksParaExecutarComSucesso();

            var resultado = await sut.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(
                    "João Silva",
                    "joao.silva@example.com",
                    "CDEP - Aviso de vencimento do empréstimo",
                    It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task DadoAcervoEmprestimoDevolutionComDadosCompletos_QuandoExecutar_EntaoConteudoEmailContemTabela()
        {
            var dataDevolucao = DateTime.Now.AddDays(3);
            var dataEmprestimo = DateTime.Now.AddDays(-15);

            var acervoEmprestimoDevolucao = new AcervoEmprestimoDevolucao
            {
                AcervoSolicitacaoId = 123,
                AcervoSolicitacaoItemId = 456,
                Solicitante = "Maria Santos",
                Email = "maria.santos@example.com",
                Titulo = "Enciclopédia Ilustrada",
                Codigo = "ENC-002",
                DataEmprestimo = dataEmprestimo,
                DataDevolucao = dataDevolucao
            };

            var mensagemJson = JsonConvert.SerializeObject(acervoEmprestimoDevolucao);
            var mensagemRabbit = new MensagemRabbit { Mensagem = mensagemJson };

            ConfigurarMocksParaExecutarComSucesso();

            await sut.Executar(mensagemRabbit);

            servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.Is<string>(content =>
                        content.Contains("<table>") &&
                        content.Contains("123") &&
                        content.Contains("456") &&
                        content.Contains("ENC-002") &&
                        content.Contains("Enciclopédia Ilustrada") &&
                        content.Contains(dataEmprestimo.ToString("dd/MM HH:mm")) &&
                        content.Contains(dataDevolucao.ToString("dd/MM")))),
                Times.Once);
        }

        [Fact]
        public async Task DadoDataDevolutaoComFormatoDDSomente_QuandoExecutar_EntaoSubstituiPlaceholderDataDevolucao()
        {
            var dataDevolucao = new DateTime(2026, 06, 15);
            var dataEmprestimo = new DateTime(2026, 05, 01);

            var acervoEmprestimoDevolucao = new AcervoEmprestimoDevolucao
            {
                AcervoSolicitacaoId = 1,
                AcervoSolicitacaoItemId = 1,
                Solicitante = "Carlos Mendes",
                Email = "carlos.mendes@example.com",
                Titulo = "Atlas Histórico",
                Codigo = "ATL-001",
                DataEmprestimo = dataEmprestimo,
                DataDevolucao = dataDevolucao
            };

            var mensagemJson = JsonConvert.SerializeObject(acervoEmprestimoDevolucao);
            var mensagemRabbit = new MensagemRabbit { Mensagem = mensagemJson };

            ConfigurarMocksParaExecutarComSucesso();

            await sut.Executar(mensagemRabbit);

            servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.Is<string>(content => content.Contains("15/06"))),
                Times.Once);
        }

        [Fact]
        public async Task DadoMensagemComCaracteresEspeciais_QuandoExecutar_EntaoGeraTabelaComDadosEscapados()
        {
            var acervoEmprestimoDevolucao = new AcervoEmprestimoDevolucao
            {
                AcervoSolicitacaoId = 1,
                AcervoSolicitacaoItemId = 1,
                Solicitante = "João & Maria",
                Email = "joao@example.com",
                Titulo = "Livro <Especial>",
                Codigo = "LV-001\"teste",
                DataEmprestimo = DateTime.Now.AddDays(-5),
                DataDevolucao = DateTime.Now.AddDays(5)
            };

            var mensagemJson = JsonConvert.SerializeObject(acervoEmprestimoDevolucao);
            var mensagemRabbit = new MensagemRabbit { Mensagem = mensagemJson };

            ConfigurarMocksParaExecutarComSucesso();

            await sut.Executar(mensagemRabbit);

            servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.Is<string>(content =>
                        content.Contains("João & Maria") &&
                        content.Contains("Livro <Especial>") &&
                        content.Contains("LV-001\"teste"))),
                Times.Once);
        }

        [Fact]
        public async Task DadoAcervoEmprestimoDevolutionNula_QuandoExecutar_EntaoLancaNegocioException()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(null) };

            ConfigurarMocksParaExecutarComSucesso();

            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.PARAMETROS_INVALIDOS);
        }

        [Fact]
        public async Task DadoAcervoEmprestimoDevolutionComValorPadraoNull_QuandoExecutar_EntaoLancaNegocioException()
        {
            var mensagemRabbit = new MensagemRabbit();

            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            await acao.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task DadoMensagemRabbitComJsonInvalido_QuandoExecutar_EntaoLancaExcecao()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "Json inválido { [ } ]" };

            ConfigurarMocksParaExecutarComSucesso();

            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            await acao.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task DadoCarregarParametrosComSucesso_QuandoExecutar_EntaoCarregaTodosParametros()
        {
            var acervoEmprestimoDevolucao = new AcervoEmprestimoDevolucao
            {
                AcervoSolicitacaoId = 1,
                AcervoSolicitacaoItemId = 1,
                Solicitante = "Test User",
                Email = "test@example.com",
                Titulo = "Test Book",
                Codigo = "TST-001",
                DataEmprestimo = DateTime.Now.AddDays(-5),
                DataDevolucao = DateTime.Now.AddDays(5)
            };

            var mensagemJson = JsonConvert.SerializeObject(acervoEmprestimoDevolucao);
            var mensagemRabbit = new MensagemRabbit { Mensagem = mensagemJson };

            ConfigurarMocksParaExecutarComSucesso();

            await sut.Executar(mensagemRabbit);

            repositorioParametroSistemaMock.Verify(
                r => r.ObterParametroPorTipoEAno(
                    It.IsAny<TipoParametroSistema>(),
                    It.IsAny<int>()),
                Times.AtLeastOnce);
        }

        [Fact]
        public async Task DadoTabelaComMultiplosItens_QuandoGerarConteudoTabela_EntaoFormatacaoCorreta()
        {
            var acervoEmprestimoDevolucao = new AcervoEmprestimoDevolucao
            {
                AcervoSolicitacaoId = 100,
                AcervoSolicitacaoItemId = 200,
                Solicitante = "Solicitante Teste",
                Email = "solicitante@test.com",
                Titulo = "Título do Livro",
                Codigo = "COD-100",
                DataEmprestimo = new DateTime(2026, 4, 15, 10, 30, 0),
                DataDevolucao = new DateTime(2026, 5, 15, 0, 0, 0)
            };

            var mensagemJson = JsonConvert.SerializeObject(acervoEmprestimoDevolucao);
            var mensagemRabbit = new MensagemRabbit { Mensagem = mensagemJson };

            ConfigurarMocksParaExecutarComSucesso();

            await sut.Executar(mensagemRabbit);

            servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(
                    "Solicitante Teste",
                    "solicitante@test.com",
                    "CDEP - Aviso de vencimento do empréstimo",
                    It.Is<string>(content =>
                        content.Contains("<thead>") &&
                        content.Contains("<tbody>") &&
                        content.Contains("<th>Solicitação</th>") &&
                        content.Contains("<th>Item</th>") &&
                        content.Contains("<th>Código</th>") &&
                        content.Contains("<th>Acervo</th>") &&
                        content.Contains("<th>Data do empréstimo</th>") &&
                        content.Contains("<th>Data da devolução</th>") &&
                        content.Contains("<td>100</td>") &&
                        content.Contains("<td>200</td>") &&
                        content.Contains("<td>COD-100</td>") &&
                        content.Contains("<td>Título do Livro</td>") &&
                        content.Contains("<td>15/04 10:30</td>") &&
                        content.Contains("<td>15/05</td>"))),
                Times.Once);
        }

        [Fact]
        public async Task DadoTabelaComEspacosEmBranco_QuandoExecutar_EntaoPreservaEspacos()
        {
            var acervoEmprestimoDevolucao = new AcervoEmprestimoDevolucao
            {
                AcervoSolicitacaoId = 1,
                AcervoSolicitacaoItemId = 1,
                Solicitante = "João  Silva",
                Email = "joao@example.com",
                Titulo = "  Livro com Espaços  ",
                Codigo = "  LV-001  ",
                DataEmprestimo = DateTime.Now.AddDays(-5),
                DataDevolucao = DateTime.Now.AddDays(5)
            };

            var mensagemJson = JsonConvert.SerializeObject(acervoEmprestimoDevolucao);
            var mensagemRabbit = new MensagemRabbit { Mensagem = mensagemJson };

            ConfigurarMocksParaExecutarComSucesso();

            await sut.Executar(mensagemRabbit);

            servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(
                    "João  Silva",
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task DadoAssuntoFixo_QuandoExecutar_EntaoAssuntoEhAlwaysCDEPAvisoDevolucao()
        {
            var acervoEmprestimoDevolucao = new AcervoEmprestimoDevolucao
            {
                AcervoSolicitacaoId = 1,
                AcervoSolicitacaoItemId = 1,
                Solicitante = "Test User",
                Email = "test@example.com",
                Titulo = "Test Book",
                Codigo = "TST-001",
                DataEmprestimo = DateTime.Now.AddDays(-5),
                DataDevolucao = DateTime.Now.AddDays(5)
            };

            var mensagemJson = JsonConvert.SerializeObject(acervoEmprestimoDevolucao);
            var mensagemRabbit = new MensagemRabbit { Mensagem = mensagemJson };

            ConfigurarMocksParaExecutarComSucesso();

            await sut.Executar(mensagemRabbit);

            servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    "CDEP - Aviso de vencimento do empréstimo",
                    It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task DadoRetornoExecutarTrue_QuandoEmailEnviado_EntaoRetornaTrueComSucesso()
        {
            var acervoEmprestimoDevolucao = new AcervoEmprestimoDevolucao
            {
                AcervoSolicitacaoId = 1,
                AcervoSolicitacaoItemId = 1,
                Solicitante = "Test User",
                Email = "test@example.com",
                Titulo = "Test Book",
                Codigo = "TST-001",
                DataEmprestimo = DateTime.Now.AddDays(-5),
                DataDevolucao = DateTime.Now.AddDays(5)
            };

            var mensagemJson = JsonConvert.SerializeObject(acervoEmprestimoDevolucao);
            var mensagemRabbit = new MensagemRabbit { Mensagem = mensagemJson };

            ConfigurarMocksParaExecutarComSucesso();

            var resultado = await sut.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
        }

        // ================= MÉTODOS AUXILIARES ================= //

        private void ConfigurarMocksParaExecutarComSucesso()
        {
            var modeloEmail = "Olá #NOME, #CONTEUDO_TABELA Data de devolução: #DATA_DEVOLUCAO_PROGRAMADA Contato: #LINK_FORMULARIO_CDEP Endereço: #ENDERECO_SEDE_CDEP_VISITA Horário: #HORARIO_FUNCIONAMENTO_SEDE_CDEP";

            // Configurar o setup genérico PRIMEIRO
            repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "Valor Padrão" });

            // Depois configurar os setups específicos (estes terão precedência)
            repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(TipoParametroSistema.ModeloEmailAvisoDevolucaoEmprestimo, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = modeloEmail });

            repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "www.formulario.cdep.com" });

            repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(TipoParametroSistema.EnderecoSedeCDEPVisita, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "Rua Principal, 123" });

            repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(TipoParametroSistema.HorarioFuncionamentoSedeCDEPVisita, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "09:00 - 17:00" });

            servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
        }
    }
}
