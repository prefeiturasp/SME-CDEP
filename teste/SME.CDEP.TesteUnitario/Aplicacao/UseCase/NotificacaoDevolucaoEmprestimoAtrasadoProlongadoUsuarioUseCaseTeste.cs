using Bogus;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase
{
    public class NotificacaoDevolucaoEmprestimoAtrasadoProlongadoUsuarioUseCaseTeste
    {
        private readonly Mock<IRepositorioParametroSistema> _repositorioParametroSistemaMock;
        private readonly Mock<IServicoNotificacaoEmail> _servicoNotificacaoEmailMock;
        private readonly NotificacaoDevolucaoEmprestimoAtrasadoProlongadoUsuarioUseCase _useCase;
        private readonly Faker _faker;

        public NotificacaoDevolucaoEmprestimoAtrasadoProlongadoUsuarioUseCaseTeste()
        {
            var mocker = new AutoMocker();
            _repositorioParametroSistemaMock = mocker.GetMock<IRepositorioParametroSistema>();
            _servicoNotificacaoEmailMock = mocker.GetMock<IServicoNotificacaoEmail>();
            _useCase = mocker.CreateInstance<NotificacaoDevolucaoEmprestimoAtrasadoProlongadoUsuarioUseCase>();
            _faker = new("pt_BR");
        }

        #region Testes do Construtor

        [Fact]
        public void DadoDependenciasValidas_QuandoInstanciarUseCase_EntaoRetornaInstanciaComSucesso()
        {
            Action acao = () => new NotificacaoDevolucaoEmprestimoAtrasadoProlongadoUsuarioUseCase(
                _repositorioParametroSistemaMock.Object,
                _servicoNotificacaoEmailMock.Object);

            acao.Should().NotThrow();
        }

        [Fact]
        public void DadoRepositorioParametroNulo_QuandoInstanciarUseCase_EntaoLancaArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new NotificacaoDevolucaoEmprestimoAtrasadoProlongadoUsuarioUseCase(null!, _servicoNotificacaoEmailMock.Object));
        }

        [Fact]
        public void DadoServicoNotificacaoNulo_QuandoInstanciarUseCase_EntaoLancaArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new NotificacaoDevolucaoEmprestimoAtrasadoProlongadoUsuarioUseCase(_repositorioParametroSistemaMock.Object, null!));
        }

        #endregion

        #region Testes do Método Executar - Cenários Válidos

        [Fact]
        public async Task DadoAcervoEmprestimoDevolucaoValido_QuandoExecutar_EntaoEnviaEmailComSucesso()
        {
            var acervoEmprestimoDevolucao = CriarAcervoEmprestimoDevolucaoValido();
            var mensagemRabbit = CriarMensagemRabbitComAcervo(acervoEmprestimoDevolucao);

            ConfigurarMocksParaExecucaoValida();

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            _servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task DadoAcervoValido_QuandoExecutar_EntaoCarregaParametrosCorretamente()
        {
            var acervoEmprestimoDevolucao = CriarAcervoEmprestimoDevolucaoValido();
            var mensagemRabbit = CriarMensagemRabbitComAcervo(acervoEmprestimoDevolucao);

            ConfigurarMocksParaExecucaoValida();

            await _useCase.Executar(mensagemRabbit);

            _repositorioParametroSistemaMock.Verify(
                r => r.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()),
                Times.AtLeastOnce);
        }

        [Fact]
        public async Task DadoAcervoValido_QuandoExecutar_EntaoMontaDadosNoTemplateEmailCorretamente()
        {
            var acervoEmprestimoDevolucao = CriarAcervoEmprestimoDevolucaoValido();
            var mensagemRabbit = CriarMensagemRabbitComAcervo(acervoEmprestimoDevolucao);
            var modeloEmail = CriarModeloEmailComPlaceholders();

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = modeloEmail });

            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            _servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(
                    acervoEmprestimoDevolucao.Solicitante,
                    acervoEmprestimoDevolucao.Email,
                    "CDEP - Aviso de devolução de empréstimo em atraso prolongado",
                    It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task DadoAcervoValido_QuandoExecutar_EntaoSubstituiDataDevolucaoProgramadaCorretamente()
        {
            var acervoEmprestimoDevolucao = CriarAcervoEmprestimoDevolucaoValido();
            var mensagemRabbit = CriarMensagemRabbitComAcervo(acervoEmprestimoDevolucao);

            ConfigurarMocksParaExecucaoValida();

            var conteudoCapturado = string.Empty;
            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string, string>((nome, email, assunto, conteudo) =>
                {
                    conteudoCapturado = conteudo;
                })
                .ReturnsAsync(true);

            await _useCase.Executar(mensagemRabbit);

            conteudoCapturado.Should().Contain(acervoEmprestimoDevolucao.DataDevolucao.ToString("dd/MM"));
        }

        [Fact]
        public async Task DadoDadosAcervoCompletos_QuandoExecutar_EntaoGeraConteudoTabelaComTodosOsCampos()
        {
            var acervoEmprestimoDevolucao = CriarAcervoEmprestimoDevolucaoValido();
            var mensagemRabbit = CriarMensagemRabbitComAcervo(acervoEmprestimoDevolucao);

            ConfigurarMocksParaExecucaoValida();

            var conteudoCapturado = string.Empty;
            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string, string>((nome, email, assunto, conteudo) =>
                {
                    conteudoCapturado = conteudo;
                })
                .ReturnsAsync(true);

            await _useCase.Executar(mensagemRabbit);

            conteudoCapturado.Should()
                .Contain(acervoEmprestimoDevolucao.AcervoSolicitacaoId.ToString())
                .And.Contain(acervoEmprestimoDevolucao.AcervoSolicitacaoItemId.ToString())
                .And.Contain(acervoEmprestimoDevolucao.Codigo)
                .And.Contain(acervoEmprestimoDevolucao.Titulo)
                .And.Contain(acervoEmprestimoDevolucao.DataEmprestimo.ToString("dd/MM HH:mm"))
                .And.Contain(acervoEmprestimoDevolucao.DataDevolucao.ToString("dd/MM"));
        }

        [Fact]
        public async Task DadoAcervoValido_QuandoExecutar_EntaoEnviaEmailComAssuntoCorreto()
        {
            var acervoEmprestimoDevolucao = CriarAcervoEmprestimoDevolucaoValido();
            var mensagemRabbit = CriarMensagemRabbitComAcervo(acervoEmprestimoDevolucao);

            ConfigurarMocksParaExecucaoValida();

            var assuntoCapturado = string.Empty;
            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string, string>((nome, email, assunto, conteudo) =>
                {
                    assuntoCapturado = assunto;
                })
                .ReturnsAsync(true);

            await _useCase.Executar(mensagemRabbit);

            assuntoCapturado.Should().Be("CDEP - Aviso de devolução de empréstimo em atraso prolongado");
        }

        #endregion

        #region Testes do Método Executar - Cenários de Erro

        [Fact]
        public async Task DadoAcervoEmMensagemNulo_QuandoExecutar_EntaoLancaNegocioException()
        {
            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = null
            };

            ConfigurarMocksParaExecucaoValida();

            await Assert.ThrowsAsync<NullReferenceException>(() => _useCase.Executar(mensagemRabbit));
        }

        [Fact]
        public async Task DadoMensagemRabbitNula_QuandoExecutar_EntaoLancaNegocioException()
        {
            var mensagemRabbit = new MensagemRabbit();

            ConfigurarMocksParaExecucaoValida();

            await Assert.ThrowsAsync<NullReferenceException>(() => _useCase.Executar(mensagemRabbit));
        }

        [Fact]
        public async Task DadoServicoNotificacaoLancaExcecao_QuandoExecutar_EntaoExcecaoEhPropagada()
        {
            var acervoEmprestimoDevolucao = CriarAcervoEmprestimoDevolucaoValido();
            var mensagemRabbit = CriarMensagemRabbitComAcervo(acervoEmprestimoDevolucao);
            var mensagemErro = "Erro ao enviar email";

            ConfigurarMocksParaExecucaoValida();

            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception(mensagemErro));

            await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(mensagemRabbit));
        }

        [Fact]
        public async Task DadoRepositorioLancaExcecaoAoCarregarParametros_QuandoExecutar_EntaoExcecaoEhPropagada()
        {
            var acervoEmprestimoDevolucao = CriarAcervoEmprestimoDevolucaoValido();
            var mensagemRabbit = CriarMensagemRabbitComAcervo(acervoEmprestimoDevolucao);
            var mensagemErro = "Erro ao obter parâmetro";

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception(mensagemErro));

            await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(mensagemRabbit));
        }

        #endregion

        #region Testes da Estrutura HTML da Tabela

        [Fact]
        public async Task DadoAcervoValido_QuandoExecutar_EntaoGeraHTMLComEstruturaTabelaCorreta()
        {
            var acervoEmprestimoDevolucao = CriarAcervoEmprestimoDevolucaoValido();
            var mensagemRabbit = CriarMensagemRabbitComAcervo(acervoEmprestimoDevolucao);

            ConfigurarMocksParaExecucaoValida();

            var conteudoCapturado = string.Empty;
            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string, string>((nome, email, assunto, conteudo) =>
                {
                    conteudoCapturado = conteudo;
                })
                .ReturnsAsync(true);

            await _useCase.Executar(mensagemRabbit);

            conteudoCapturado.Should()
                .Contain("<table>")
                .And.Contain("</table>")
                .And.Contain("<thead>")
                .And.Contain("</thead>")
                .And.Contain("<tbody>")
                .And.Contain("</tbody>")
                .And.Contain("<tr>")
                .And.Contain("</tr>")
                .And.Contain("<th>")
                .And.Contain("</th>")
                .And.Contain("<td>")
                .And.Contain("</td>");
        }

        [Fact]
        public async Task DadoAcervoValido_QuandoExecutar_EntaoTabelaPossuiTodosOsCabecalhos()
        {
            var acervoEmprestimoDevolucao = CriarAcervoEmprestimoDevolucaoValido();
            var mensagemRabbit = CriarMensagemRabbitComAcervo(acervoEmprestimoDevolucao);

            ConfigurarMocksParaExecucaoValida();

            var conteudoCapturado = string.Empty;
            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string, string>((nome, email, assunto, conteudo) =>
                {
                    conteudoCapturado = conteudo;
                })
                .ReturnsAsync(true);

            await _useCase.Executar(mensagemRabbit);

            conteudoCapturado.Should()
                .Contain("Solicitação")
                .And.Contain("Item")
                .And.Contain("Código")
                .And.Contain("Acervo")
                .And.Contain("Data do empréstimo")
                .And.Contain("Data da devolução");
        }

        #endregion

        #region Testes de Formatação de Data

        [Fact]
        public async Task DadoDataEmprestimo_QuandoExecutar_EntaoFormataDataComHoraCorretamente()
        {
            var dataEmprestimo = new DateTime(2026, 4, 15, 10, 30, 0);
            var acervoEmprestimoDevolucao = CriarAcervoEmprestimoDevolucaoValido(dataEmprestimo: dataEmprestimo);
            var mensagemRabbit = CriarMensagemRabbitComAcervo(acervoEmprestimoDevolucao);

            ConfigurarMocksParaExecucaoValida();

            var conteudoCapturado = string.Empty;
            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string, string>((nome, email, assunto, conteudo) =>
                {
                    conteudoCapturado = conteudo;
                })
                .ReturnsAsync(true);

            await _useCase.Executar(mensagemRabbit);

            conteudoCapturado.Should().Contain("15/04 10:30");
        }

        [Fact]
        public async Task DadoDataDevolucao_QuandoExecutar_EntaoFormataDataSemHoraCorretamente()
        {
            var dataDevolucao = new DateTime(2026, 5, 20, 14, 45, 0);
            var acervoEmprestimoDevolucao = CriarAcervoEmprestimoDevolucaoValido(dataDevolucao: dataDevolucao);
            var mensagemRabbit = CriarMensagemRabbitComAcervo(acervoEmprestimoDevolucao);

            ConfigurarMocksParaExecucaoValida();

            var conteudoCapturado = string.Empty;
            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string, string>((nome, email, assunto, conteudo) =>
                {
                    conteudoCapturado = conteudo;
                })
                .ReturnsAsync(true);

            await _useCase.Executar(mensagemRabbit);

            conteudoCapturado.Should().Contain("20/05");
        }

        #endregion

        #region Testes de Valores Especiais

        [Fact]
        public async Task DadoAcervoComValoresVazios_QuandoExecutar_EntaoExibeVaziosCorretamente()
        {
            var acervoEmprestimoDevolucao = new AcervoEmprestimoDevolucao
            {
                AcervoSolicitacaoId = 1,
                AcervoSolicitacaoItemId = 2,
                Solicitante = "João Silva",
                Titulo = string.Empty,
                Codigo = string.Empty,
                Email = "joao@example.com",
                DataEmprestimo = DateTime.Now,
                DataDevolucao = DateTime.Now.AddDays(5)
            };
            var mensagemRabbit = CriarMensagemRabbitComAcervo(acervoEmprestimoDevolucao);

            ConfigurarMocksParaExecucaoValida();

            var conteudoCapturado = string.Empty;
            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string, string>((nome, email, assunto, conteudo) =>
                {
                    conteudoCapturado = conteudo;
                })
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            conteudoCapturado.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task DadoAcervoComValoresNumerosAltos_QuandoExecutar_EntaoProcessaCorretamente()
        {
            var acervoEmprestimoDevolucao = new AcervoEmprestimoDevolucao
            {
                AcervoSolicitacaoId = long.MaxValue,
                AcervoSolicitacaoItemId = long.MaxValue,
                Solicitante = "Maria Santos",
                Titulo = "Livro Importante",
                Codigo = "COD-9999999",
                Email = "maria@example.com",
                DataEmprestimo = DateTime.Now,
                DataDevolucao = DateTime.Now.AddDays(5)
            };
            var mensagemRabbit = CriarMensagemRabbitComAcervo(acervoEmprestimoDevolucao);

            ConfigurarMocksParaExecucaoValida();

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
        }

        #endregion

        #region Métodos Auxiliares

        private AcervoEmprestimoDevolucao CriarAcervoEmprestimoDevolucaoValido(
            DateTime? dataEmprestimo = null,
            DateTime? dataDevolucao = null)
        {
            return new AcervoEmprestimoDevolucao
            {
                AcervoSolicitacaoId = _faker.Random.Long(1, 1000),
                AcervoSolicitacaoItemId = _faker.Random.Long(1, 1000),
                Solicitante = _faker.Person.FirstName,
                Titulo = _faker.Lorem.Sentence(),
                Codigo = _faker.Random.AlphaNumeric(10),
                Email = _faker.Internet.Email(),
                DataEmprestimo = dataEmprestimo ?? DateTime.Now.AddDays(-15),
                DataDevolucao = dataDevolucao ?? DateTime.Now.AddDays(5)
            };
        }

        private MensagemRabbit CriarMensagemRabbitComAcervo(AcervoEmprestimoDevolucao acervo)
        {
            return new MensagemRabbit
            {
                Mensagem = System.Text.Json.JsonSerializer.Serialize(acervo)
            };
        }

        private string CriarModeloEmailComPlaceholders()
        {
            return @"
                <html>
                    <body>
                        <p>Olá #NOME,</p>
                        <p>Segue abaixo os dados do empréstimo em atraso prolongado:</p>
                        #CONTEUDO_TABELA
                        <p>Data de devolução programada: #DATA_DEVOLUCAO_PROGRAMADA</p>
                        <p>Contato: #LINK_FORMULARIO_CDEP</p>
                        <p>Endereço: #ENDERECO_SEDE_CDEP_VISITA</p>
                        <p>Horário: #HORARIO_FUNCIONAMENTO_SEDE_CDEP</p>
                    </body>
                </html>";
        }

        private void ConfigurarMocksParaExecucaoValida()
        {
            var modeloEmail = CriarModeloEmailComPlaceholders();

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = modeloEmail });

            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
        }

        #endregion
    }
}
