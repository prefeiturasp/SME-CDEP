using FluentAssertions;
using Moq;
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
            _servicoEventoMock = new Mock<IServicoEvento>();
            _useCase = new ExecutarCriacaoDeEventosTipoFeriadoAnoAtualUseCase(_servicoEventoMock.Object);
        }

        [Fact(DisplayName = "ExecutarCriacaoDeEventosTipoFeriado - Construtor com serviço nulo deve lançar exceção")]
        public void Construtor_ComServicoNulo_DeveLancarArgumentNullException()
        {
            var acao = () => new ExecutarCriacaoDeEventosTipoFeriadoAnoAtualUseCase(null!);

            acao.Should()
                .Throw<ArgumentNullException>()
                .WithParameterName("servicoEvento");
        }

        [Fact(DisplayName = "ExecutarCriacaoDeEventosTipoFeriado - Construtor com serviço válido deve inicializar")]
        public void Construtor_ComServicoValido_DeveInicializar()
        {
            _useCase.Should().NotBeNull();
        }

        [Fact(DisplayName = "ExecutarCriacaoDeEventosTipoFeriado - Executar deve chamar GerarEventosFixos")]
        public async Task Executar_DeveCharmaarGerarEventosFixos()
        {
            var mensagemRabbit = new MensagemRabbit();
            _servicoEventoMock.Setup(s => s.GerarEventosFixos())
                .Returns(Task.CompletedTask);
            _servicoEventoMock.Setup(s => s.GerarEventosMoveis())
                .Returns(Task.CompletedTask);

            await _useCase.Executar(mensagemRabbit);

            _servicoEventoMock.Verify(s => s.GerarEventosFixos(), Times.Once);
        }

        [Fact(DisplayName = "ExecutarCriacaoDeEventosTipoFeriado - Executar deve chamar GerarEventosMoveis")]
        public async Task Executar_DeveChamamarGerarEventosMoveis()
        {
            var mensagemRabbit = new MensagemRabbit();
            _servicoEventoMock.Setup(s => s.GerarEventosFixos())
                .Returns(Task.CompletedTask);
            _servicoEventoMock.Setup(s => s.GerarEventosMoveis())
                .Returns(Task.CompletedTask);

            await _useCase.Executar(mensagemRabbit);

            _servicoEventoMock.Verify(s => s.GerarEventosMoveis(), Times.Once);
        }

        [Fact(DisplayName = "ExecutarCriacaoDeEventosTipoFeriado - Executar deve retornar true")]
        public async Task Executar_DeveRetornarTrue()
        {
            var mensagemRabbit = new MensagemRabbit();
            _servicoEventoMock.Setup(s => s.GerarEventosFixos())
                .Returns(Task.CompletedTask);
            _servicoEventoMock.Setup(s => s.GerarEventosMoveis())
                .Returns(Task.CompletedTask);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
        }

        [Fact(DisplayName = "ExecutarCriacaoDeEventosTipoFeriado - Executar deve chamar ambos os métodos em sequência")]
        public async Task Executar_DeveChamarAmbosMétodosEmSequência()
        {
            var sequenciaExecucao = new List<string>();
            var mensagemRabbit = new MensagemRabbit();

            _servicoEventoMock.Setup(s => s.GerarEventosFixos())
                .Callback(() => sequenciaExecucao.Add("GerarEventosFixos"))
                .Returns(Task.CompletedTask);

            _servicoEventoMock.Setup(s => s.GerarEventosMoveis())
                .Callback(() => sequenciaExecucao.Add("GerarEventosMoveis"))
                .Returns(Task.CompletedTask);

            await _useCase.Executar(mensagemRabbit);

            sequenciaExecucao.Should().HaveCount(2);
            sequenciaExecucao[0].Should().Be("GerarEventosFixos");
            sequenciaExecucao[1].Should().Be("GerarEventosMoveis");
        }

        [Fact(DisplayName = "ExecutarCriacaoDeEventosTipoFeriado - Executar com parâmetro null deve processar")]
        public async Task Executar_ComParâmetroNull_DeveProcessar()
        {
            _servicoEventoMock.Setup(s => s.GerarEventosFixos())
                .Returns(Task.CompletedTask);
            _servicoEventoMock.Setup(s => s.GerarEventosMoveis())
                .Returns(Task.CompletedTask);

            var resultado = await _useCase.Executar(null!);

            resultado.Should().BeTrue();
            _servicoEventoMock.Verify(s => s.GerarEventosFixos(), Times.Once);
            _servicoEventoMock.Verify(s => s.GerarEventosMoveis(), Times.Once);
        }

        [Fact(DisplayName = "ExecutarCriacaoDeEventosTipoFeriado - Executar múltiplas vezes deve chamar métodos múltiplas vezes")]
        public async Task Executar_MúltiplosVezes_DeveChamarMétodosMúltiplosVezes()
        {
            var mensagemRabbit = new MensagemRabbit();
            _servicoEventoMock.Setup(s => s.GerarEventosFixos())
                .Returns(Task.CompletedTask);
            _servicoEventoMock.Setup(s => s.GerarEventosMoveis())
                .Returns(Task.CompletedTask);

            var resultado1 = await _useCase.Executar(mensagemRabbit);
            var resultado2 = await _useCase.Executar(mensagemRabbit);
            var resultado3 = await _useCase.Executar(mensagemRabbit);

            resultado1.Should().BeTrue();
            resultado2.Should().BeTrue();
            resultado3.Should().BeTrue();
            _servicoEventoMock.Verify(s => s.GerarEventosFixos(), Times.Exactly(3));
            _servicoEventoMock.Verify(s => s.GerarEventosMoveis(), Times.Exactly(3));
        }

        [Fact(DisplayName = "ExecutarCriacaoDeEventosTipoFeriado - Executar com GerarEventosFixos falhando deve propagar exceção")]
        public async Task Executar_ComGerarEventosFixosFalhando_DevePropagareExceção()
        {
            var mensagemRabbit = new MensagemRabbit();
            var excecaoEsperada = new InvalidOperationException("Erro ao gerar eventos fixos");

            _servicoEventoMock.Setup(s => s.GerarEventosFixos())
                .ThrowsAsync(excecaoEsperada);

            var acao = () => _useCase.Executar(mensagemRabbit);

            await acao.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Erro ao gerar eventos fixos");
        }

        [Fact(DisplayName = "ExecutarCriacaoDeEventosTipoFeriado - Executar com GerarEventosMoveis falhando deve propagar exceção")]
        public async Task Executar_ComGerarEventosMóveisFalhando_DevePropagareExceção()
        {
            var mensagemRabbit = new MensagemRabbit();
            var excecaoEsperada = new InvalidOperationException("Erro ao gerar eventos móveis");

            _servicoEventoMock.Setup(s => s.GerarEventosFixos())
                .Returns(Task.CompletedTask);
            _servicoEventoMock.Setup(s => s.GerarEventosMoveis())
                .ThrowsAsync(excecaoEsperada);

            var acao = () => _useCase.Executar(mensagemRabbit);

            await acao.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Erro ao gerar eventos móveis");
        }

        [Fact(DisplayName = "ExecutarCriacaoDeEventosTipoFeriado - Executar deve aguardar conclusão de ambos os métodos")]
        public async Task Executar_DeveAguardarConclus‌ãoDosMetódos()
        {
            var mensagemRabbit = new MensagemRabbit();
            var tarefas = new List<Task>();

            _servicoEventoMock.Setup(s => s.GerarEventosFixos())
                .Callback(() => tarefas.Add(Task.Delay(10)))
                .Returns(() => tarefas[^1]);

            _servicoEventoMock.Setup(s => s.GerarEventosMoveis())
                .Callback(() => tarefas.Add(Task.Delay(10)))
                .Returns(() => tarefas[^1]);

            var tempoInicio = DateTime.Now;
            await _useCase.Executar(mensagemRabbit);
            var tempoFim = DateTime.Now;

            (tempoFim - tempoInicio).TotalMilliseconds.Should().BeGreaterThanOrEqualTo(20);
        }

        [Fact(DisplayName = "ExecutarCriacaoDeEventosTipoFeriado - Implementa interface IRabbitUseCase")]
        public void UseCase_DeveImplementarInterfaceIRabbitUseCase()
        {
            _useCase.Should().BeAssignableTo<IExecutarCriacaoDeEventosTipoFeriadoAnoAtualUseCase>();
        }

        [Fact(DisplayName = "ExecutarCriacaoDeEventosTipoFeriado - Executar com mensagem válida deve retornar sucesso")]
        public async Task Executar_ComMensagemValida_DeveRetornarSucesso()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "dados" };
            _servicoEventoMock.Setup(s => s.GerarEventosFixos())
                .Returns(Task.CompletedTask);
            _servicoEventoMock.Setup(s => s.GerarEventosMoveis())
                .Returns(Task.CompletedTask);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            _servicoEventoMock.Verify(s => s.GerarEventosFixos(), Times.Once);
            _servicoEventoMock.Verify(s => s.GerarEventosMoveis(), Times.Once);
        }
    }
}
