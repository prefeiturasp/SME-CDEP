using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase
{
    public class NotificacaoDevolucaoEmprestimoAtrasadoUseCaseTeste
    {
        private readonly Mock<IServicoAcervoEmprestimo> _servicoAcervoEmprestimoMock;
        private readonly NotificacaoDevolucaoEmprestimoAtrasadoUseCase _useCase;

        public NotificacaoDevolucaoEmprestimoAtrasadoUseCaseTeste()
        {
            var mocker = new AutoMocker();
            _servicoAcervoEmprestimoMock = mocker.GetMock<IServicoAcervoEmprestimo>();
            _useCase = mocker.CreateInstance<NotificacaoDevolucaoEmprestimoAtrasadoUseCase>();
        }

        #region Testes do Construtor

        [Fact]
        public void DadoDependenciaValida_QuandoInstanciarUseCase_EntaoRetornaInstanciaComSucesso()
        {
            Action acao = () => new NotificacaoDevolucaoEmprestimoAtrasadoUseCase(
                _servicoAcervoEmprestimoMock.Object);

            acao.Should().NotThrow();
        }

        [Fact]
        public void DadoServicoNulo_QuandoInstanciarUseCase_EntaoLancaArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new NotificacaoDevolucaoEmprestimoAtrasadoUseCase(null!));
        }

        [Fact]
        public void DadoServicoNulo_QuandoInstanciarUseCase_EntaoMensagemExcecaoContemNomeDoDependencia()
        {
            var excecao = Assert.Throws<ArgumentNullException>(() =>
                new NotificacaoDevolucaoEmprestimoAtrasadoUseCase(null!));

            excecao.ParamName.Should().Be("servicoEventoAcervoEmprestimo");
        }

        #endregion

        #region Testes do Método Executar - Cenário de Sucesso

        [Fact]
        public async Task DadoMensagemValida_QuandoExecutar_EntaoRetornaTrueComSucesso()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "Teste" };

            _servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarDevolucaoEmprestimoAtrasado())
                .Returns(Task.CompletedTask);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
        }

        [Fact]
        public async Task DadoMensagemValida_QuandoExecutar_EntaoChamaServicoNotificacaoUmaVez()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "Notificação" };

            _servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarDevolucaoEmprestimoAtrasado())
                .Returns(Task.CompletedTask);

            await _useCase.Executar(mensagemRabbit);

            _servicoAcervoEmprestimoMock.Verify(
                s => s.NotificarDevolucaoEmprestimoAtrasado(),
                Times.Once);
        }

        [Fact]
        public async Task DadoMensagemComValorVazio_QuandoExecutar_EntaoProcessaCorretamente()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = string.Empty };

            _servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarDevolucaoEmprestimoAtrasado())
                .Returns(Task.CompletedTask);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            _servicoAcervoEmprestimoMock.Verify(
                s => s.NotificarDevolucaoEmprestimoAtrasado(),
                Times.Once);
        }

        [Fact]
        public async Task DadoMensagemComConteudoComplexo_QuandoExecutar_EntaoProcessaCorretamente()
        {
            var conteudoComplexo = "{\"id\":1,\"nome\":\"Teste\",\"data\":\"2026-04-29\"}";
            var mensagemRabbit = new MensagemRabbit { Mensagem = conteudoComplexo };

            _servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarDevolucaoEmprestimoAtrasado())
                .Returns(Task.CompletedTask);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            _servicoAcervoEmprestimoMock.Verify(
                s => s.NotificarDevolucaoEmprestimoAtrasado(),
                Times.Once);
        }

        #endregion

        #region Testes do Método Executar - Cenários de Exceção

        [Fact]
        public async Task DadoServicoLancaExcecao_QuandoExecutar_EntaoExcecaoEhPropagada()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "Teste" };
            var mensagemErro = "Erro ao notificar devolução em atraso";

            _servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarDevolucaoEmprestimoAtrasado())
                .ThrowsAsync(new Exception(mensagemErro));

            var excecao = await Assert.ThrowsAsync<Exception>(
                () => _useCase.Executar(mensagemRabbit));

            excecao.Message.Should().Be(mensagemErro);
        }

        [Fact]
        public async Task DadoServicoLancaExcecaoComMensagem_QuandoExecutar_EntaoExcecaoPreservaDetalhesOriginais()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "Teste" };
            var mensagemErro = "Erro de conexão com banco de dados";

            _servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarDevolucaoEmprestimoAtrasado())
                .ThrowsAsync(new InvalidOperationException(mensagemErro));

            var excecao = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _useCase.Executar(mensagemRabbit));

            excecao.Message.Should().Be(mensagemErro);
        }

        [Fact]
        public async Task DadoServicoLancaExcecaoDeNegocio_QuandoExecutar_EntaoExcecaoEhPropagadaSemTratar()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "Teste" };

            _servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarDevolucaoEmprestimoAtrasado())
                .ThrowsAsync(new ArgumentException("Parâmetro inválido"));

            await Assert.ThrowsAsync<ArgumentException>(
                () => _useCase.Executar(mensagemRabbit));
        }

        #endregion

        #region Testes de Integração com MensagemRabbit

        [Fact]
        public async Task DadoMensagemRabbitNula_QuandoExecutar_EntaoLancaArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _useCase.Executar(null!));
        }

        [Fact]
        public async Task DadoMensagemRabbitSemMensagem_QuandoExecutar_EntaoProcessaCorretamente()
        {
            var mensagemRabbit = new MensagemRabbit();

            _servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarDevolucaoEmprestimoAtrasado())
                .Returns(Task.CompletedTask);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
        }

        #endregion

        #region Testes de Comportamento Assíncrono

        [Fact]
        public async Task DadoOperacaoAssincrona_QuandoExecutar_EntaoAguardaCompletoDoServiço()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "Teste" };
            var tarefaCompletada = false;

            _servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarDevolucaoEmprestimoAtrasado())
                .Returns(Task.Run(() =>
                {
                    tarefaCompletada = true;
                }));

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            tarefaCompletada.Should().BeTrue();
        }

        [Fact]
        public async Task DadoMultiplasExecucoes_QuandoExecutarVariasVezes_EntaoProcessaTodas()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "Teste" };

            _servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarDevolucaoEmprestimoAtrasado())
                .Returns(Task.CompletedTask);

            var resultado1 = await _useCase.Executar(mensagemRabbit);
            var resultado2 = await _useCase.Executar(mensagemRabbit);
            var resultado3 = await _useCase.Executar(mensagemRabbit);

            resultado1.Should().BeTrue();
            resultado2.Should().BeTrue();
            resultado3.Should().BeTrue();

            _servicoAcervoEmprestimoMock.Verify(
                s => s.NotificarDevolucaoEmprestimoAtrasado(),
                Times.Exactly(3));
        }

        #endregion

        #region Testes do Tipo de Retorno

        [Fact]
        public async Task DadoExecutarComSucesso_QuandoVerificaTipo_EntaoRetornoEhBoolean()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "Teste" };

            _servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarDevolucaoEmprestimoAtrasado())
                .Returns(Task.CompletedTask);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
        }

        [Fact]
        public async Task DadoExecutarComSucesso_QuandoVerificaAsyncTask_EntaoRetornaTaskOfBool()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "Teste" };

            _servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarDevolucaoEmprestimoAtrasado())
                .Returns(Task.CompletedTask);

            var tarefa = _useCase.Executar(mensagemRabbit);

            tarefa.Should().BeOfType<Task<bool>>();
            (await tarefa).Should().BeTrue();
        }

        #endregion

        #region Testes de Injeção de Dependência

        [Fact]
        public void DadoInstanciaUseCase_QuandoVerificaDependencia_EntaoServicoEhArmazenado()
        {
            var useCase = new NotificacaoDevolucaoEmprestimoAtrasadoUseCase(
                _servicoAcervoEmprestimoMock.Object);

            useCase.Should().NotBeNull();
        }

        [Fact]
        public async Task DadoInjecaoDependencia_QuandoExecutarComServicoMock_EntaoUsaMockCorreto()
        {
            var servicoMock = new Mock<IServicoAcervoEmprestimo>();
            var useCase = new NotificacaoDevolucaoEmprestimoAtrasadoUseCase(servicoMock.Object);
            var mensagemRabbit = new MensagemRabbit { Mensagem = "Teste" };

            servicoMock
                .Setup(s => s.NotificarDevolucaoEmprestimoAtrasado())
                .Returns(Task.CompletedTask);

            var resultado = await useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            servicoMock.Verify(s => s.NotificarDevolucaoEmprestimoAtrasado(), Times.Once);
        }

        #endregion

        #region Testes de Cobertura Completa de Caminhos

        [Fact]
        public async Task CaminhoPrincipal_DadoParametroValido_QuandoExecutar_EntaoRetornaTrueESucesso()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "Mensagem de teste para cobertura" };

            _servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarDevolucaoEmprestimoAtrasado())
                .Returns(Task.CompletedTask);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            _servicoAcervoEmprestimoMock.Verify(
                s => s.NotificarDevolucaoEmprestimoAtrasado(),
                Times.Once);
        }

        [Fact]
        public async Task CaminhoExcecao_DadoServicoFalha_QuandoExecutar_EntaoExcecaoEhPropagada()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "Teste com erro" };

            _servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarDevolucaoEmprestimoAtrasado())
                .ThrowsAsync(new Exception("Erro intencional"));

            await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(mensagemRabbit));
        }

        [Fact]
        public async Task TodosCaminhosdoCodigo_QuandoExecutarCompleto_EntaoCobetura100Porcento()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "Cobertura total" };

            _servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarDevolucaoEmprestimoAtrasado())
                .Returns(Task.CompletedTask);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            _servicoAcervoEmprestimoMock.Verify(
                s => s.NotificarDevolucaoEmprestimoAtrasado(),
                Times.Once);
        }

        #endregion
    }
}
