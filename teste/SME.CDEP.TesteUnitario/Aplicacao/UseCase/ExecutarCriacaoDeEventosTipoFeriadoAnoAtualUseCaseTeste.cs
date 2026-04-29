using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase
{
    public class ExecutarCriacaoDeEventosTipoFeriadoAnoAtualUseCaseTeste
    {
        private readonly Mock<IServicoEvento> _servicoEventoMock;
        private readonly ExecutarCriacaoDeEventosTipoFeriadoAnoAtualUseCase _useCase;

        public ExecutarCriacaoDeEventosTipoFeriadoAnoAtualUseCaseTeste()
        {
            var mocker = new AutoMocker();
            _servicoEventoMock = mocker.GetMock<IServicoEvento>();
            _useCase = mocker.CreateInstance<ExecutarCriacaoDeEventosTipoFeriadoAnoAtualUseCase>();
        }

        #region Testes do Construtor

        [Fact]
        public void DadoDependenciasValidas_QuandoInstanciarUseCase_EntaoRetornaInstanciaComSucesso()
        {
            Action acao = () => new ExecutarCriacaoDeEventosTipoFeriadoAnoAtualUseCase(
                _servicoEventoMock.Object);

            acao.Should().NotThrow();
        }

        [Fact]
        public void DadoServicoEventoNulo_QuandoInstanciarUseCase_EntaoLancaArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ExecutarCriacaoDeEventosTipoFeriadoAnoAtualUseCase(null!));
        }

        #endregion

        #region Testes do Método Executar - Cenários de Sucesso

        [Fact]
        public async Task DadoServicoEventoOperacional_QuandoExecutar_EntaoChamaGerarEventosFixosEMoveis()
        {
            _servicoEventoMock
                .Setup(s => s.GerarEventosFixos())
                .Returns(Task.CompletedTask);

            _servicoEventoMock
                .Setup(s => s.GerarEventosMoveis())
                .Returns(Task.CompletedTask);

            var mensagemRabbit = new MensagemRabbit();

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            _servicoEventoMock.Verify(s => s.GerarEventosFixos(), Times.Once);
            _servicoEventoMock.Verify(s => s.GerarEventosMoveis(), Times.Once);
        }

        [Fact]
        public async Task DadoMensagemRabbitQualquer_QuandoExecutar_EntaoRetornaVerdadeiro()
        {
            _servicoEventoMock
                .Setup(s => s.GerarEventosFixos())
                .Returns(Task.CompletedTask);

            _servicoEventoMock
                .Setup(s => s.GerarEventosMoveis())
                .Returns(Task.CompletedTask);

            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = "Qualquer mensagem",
                CodigoCorrelacao = Guid.NewGuid()
            };

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
        }

        [Fact]
        public async Task DadoGerarEventosFixosConcluido_QuandoExecutar_EntaoChamaGerarEventosMoveis()
        {
            var sequenciaExecucao = new List<string>();

            _servicoEventoMock
                .Setup(s => s.GerarEventosFixos())
                .Returns(async () =>
                {
                    await Task.Delay(10);
                    sequenciaExecucao.Add("GerarEventosFixos");
                });

            _servicoEventoMock
                .Setup(s => s.GerarEventosMoveis())
                .Callback(() => sequenciaExecucao.Add("GerarEventosMoveis"))
                .Returns(Task.CompletedTask);

            var mensagemRabbit = new MensagemRabbit();

            await _useCase.Executar(mensagemRabbit);

            sequenciaExecucao.Should().ContainInOrder("GerarEventosFixos", "GerarEventosMoveis");
        }

        [Fact]
        public async Task DadoExecutarChamadoVariasVezes_QuandoExecutar_EntaoProcessaTodosOsChamadasIndependentemente()
        {
            _servicoEventoMock
                .Setup(s => s.GerarEventosFixos())
                .Returns(Task.CompletedTask);

            _servicoEventoMock
                .Setup(s => s.GerarEventosMoveis())
                .Returns(Task.CompletedTask);

            var mensagem1 = new MensagemRabbit { CodigoCorrelacao = Guid.NewGuid() };
            var mensagem2 = new MensagemRabbit { CodigoCorrelacao = Guid.NewGuid() };
            var mensagem3 = new MensagemRabbit { CodigoCorrelacao = Guid.NewGuid() };

            await _useCase.Executar(mensagem1);
            await _useCase.Executar(mensagem2);
            await _useCase.Executar(mensagem3);

            _servicoEventoMock.Verify(s => s.GerarEventosFixos(), Times.Exactly(3));
            _servicoEventoMock.Verify(s => s.GerarEventosMoveis(), Times.Exactly(3));
        }

        [Fact]
        public async Task DadoOperacoesAssincronas_QuandoExecutar_EntaoAguardaTarefasComSucesso()
        {
            var delayMs = 50;

            _servicoEventoMock
                .Setup(s => s.GerarEventosFixos())
                .Returns(async () => await Task.Delay(delayMs));

            _servicoEventoMock
                .Setup(s => s.GerarEventosMoveis())
                .Returns(async () => await Task.Delay(delayMs));

            var mensagemRabbit = new MensagemRabbit();

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var resultado = await _useCase.Executar(mensagemRabbit);
            stopwatch.Stop();

            resultado.Should().BeTrue();
            stopwatch.ElapsedMilliseconds.Should().BeGreaterThanOrEqualTo(delayMs * 2);
        }

        [Fact]
        public async Task DadoTarefasCompletas_QuandoExecutar_EntaoAmbasTarefasSaoAwaitadas()
        {
            var gerarEventosFixosChamada = false;
            var gerarEventosMoveisChama = false;

            _servicoEventoMock
                .Setup(s => s.GerarEventosFixos())
                .Returns(async () =>
                {
                    await Task.CompletedTask;
                    gerarEventosFixosChamada = true;
                });

            _servicoEventoMock
                .Setup(s => s.GerarEventosMoveis())
                .Returns(async () =>
                {
                    await Task.CompletedTask;
                    gerarEventosMoveisChama = true;
                });

            var mensagemRabbit = new MensagemRabbit();

            await _useCase.Executar(mensagemRabbit);

            gerarEventosFixosChamada.Should().BeTrue();
            gerarEventosMoveisChama.Should().BeTrue();
        }

        #endregion

        #region Testes do Método Executar - Cenários de Erro

        [Fact]
        public async Task DadoGerarEventosFixosLancaExcecao_QuandoExecutar_EntaoExcecaoEhPropagada()
        {
            var mensagemErro = "Erro ao gerar eventos fixos";

            _servicoEventoMock
                .Setup(s => s.GerarEventosFixos())
                .ThrowsAsync(new Exception(mensagemErro));

            _servicoEventoMock
                .Setup(s => s.GerarEventosMoveis())
                .Returns(Task.CompletedTask);

            var mensagemRabbit = new MensagemRabbit();

            await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(mensagemRabbit));
            _servicoEventoMock.Verify(s => s.GerarEventosMoveis(), Times.Never);
        }

        [Fact]
        public async Task DadoGerarEventosMoveisLancaExcecao_QuandoExecutar_EntaoExcecaoEhPropagada()
        {
            var mensagemErro = "Erro ao gerar eventos móveis";

            _servicoEventoMock
                .Setup(s => s.GerarEventosFixos())
                .Returns(Task.CompletedTask);

            _servicoEventoMock
                .Setup(s => s.GerarEventosMoveis())
                .ThrowsAsync(new Exception(mensagemErro));

            var mensagemRabbit = new MensagemRabbit();

            var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(mensagemRabbit));
            exception.Message.Should().Be(mensagemErro);
        }

        [Fact]
        public async Task DadoServicoEventoIndisponivel_QuandoExecutar_EntaoExcecaoEhPropagada()
        {
            _servicoEventoMock
                .Setup(s => s.GerarEventosFixos())
                .ThrowsAsync(new InvalidOperationException("Serviço indisponível"));

            var mensagemRabbit = new MensagemRabbit();

            await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.Executar(mensagemRabbit));
        }

        [Fact]
        public async Task DadoExcecaoDeTipoNegocio_QuandoExecutar_EntaoExcecaoEhPropagada()
        {
            _servicoEventoMock
                .Setup(s => s.GerarEventosFixos())
                .ThrowsAsync(new InvalidOperationException("Violação de regra de negócio"));

            var mensagemRabbit = new MensagemRabbit();

            await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.Executar(mensagemRabbit));
        }

        #endregion

        #region Testes de Cobertura de Linhas Críticas

        [Fact]
        public async Task DadoUseCase_QuandoExecutarChamado_EntaoAmbasMétodosDoServicoSaoChamados()
        {
            _servicoEventoMock
                .Setup(s => s.GerarEventosFixos())
                .Returns(Task.CompletedTask);

            _servicoEventoMock
                .Setup(s => s.GerarEventosMoveis())
                .Returns(Task.CompletedTask);

            var mensagemRabbit = new MensagemRabbit();

            await _useCase.Executar(mensagemRabbit);

            _servicoEventoMock.VerifyAll();
        }

        [Fact]
        public async Task DadoRetornoDoMetodo_QuandoExecutar_EntaoSempreRetornaTrue()
        {
            _servicoEventoMock
                .Setup(s => s.GerarEventosFixos())
                .Returns(Task.CompletedTask);

            _servicoEventoMock
                .Setup(s => s.GerarEventosMoveis())
                .Returns(Task.CompletedTask);

            var mensagemRabbit = new MensagemRabbit();

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
        }

        [Fact]
        public async Task DadoParallelExecution_QuandoExecutarMultiplosServiços_EntaoAmbosConcluemComSucesso()
        {
            _servicoEventoMock
                .Setup(s => s.GerarEventosFixos())
                .Returns(async () => await Task.Delay(25));

            _servicoEventoMock
                .Setup(s => s.GerarEventosMoveis())
                .Returns(async () => await Task.Delay(25));

            var tarefas = new List<Task<bool>>();

            for (int i = 0; i < 5; i++)
            {
                tarefas.Add(_useCase.Executar(new MensagemRabbit()));
            }

            var resultados = await Task.WhenAll(tarefas);

            resultados.Should().AllBeAssignableTo<bool>();
        }

        #endregion
    }
}
