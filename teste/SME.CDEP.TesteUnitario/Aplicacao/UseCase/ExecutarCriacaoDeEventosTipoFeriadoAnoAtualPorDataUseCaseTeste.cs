using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase
{
    public class ExecutarCriacaoDeEventosTipoFeriadoAnoAtualPorDataUseCaseTeste
    {
        private readonly Mock<IServicoEvento> _servicoEventoMock;
        private readonly ExecutarCriacaoDeEventosTipoFeriadoAnoAtualPorDataUseCase _useCase;

        public ExecutarCriacaoDeEventosTipoFeriadoAnoAtualPorDataUseCaseTeste()
        {
            var mocker = new AutoMocker();
            _servicoEventoMock = mocker.GetMock<IServicoEvento>();
            _useCase = mocker.CreateInstance<ExecutarCriacaoDeEventosTipoFeriadoAnoAtualPorDataUseCase>();
        }

        #region Testes do Construtor

        [Fact]
        public void DadoDependenciaValida_QuandoInstanciarUseCase_EntaoRetornaInstanciaComSucesso()
        {
            Action acao = () => new ExecutarCriacaoDeEventosTipoFeriadoAnoAtualPorDataUseCase(
                _servicoEventoMock.Object);

            acao.Should().NotThrow();
        }

        [Fact]
        public void DadoServicoEventoNulo_QuandoInstanciarUseCase_EntaoLancaArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ExecutarCriacaoDeEventosTipoFeriadoAnoAtualPorDataUseCase(null!));
        }

        #endregion

        #region Testes do Método Executar - Cenários de Sucesso

        [Fact]
        public async Task DadoEventoCadastroValido_QuandoExecutar_EntaoInsereComSucessoERetornaVerdadeiro()
        {
            var eventoCadastro = CriarEventoCadastroDTO();
            var mensagemRabbit = CriarMensagemRabbit(eventoCadastro);

            _servicoEventoMock
                .Setup(s => s.Inserir(It.IsAny<EventoCadastroDTO>()))
                .ReturnsAsync(1L);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            _servicoEventoMock.Verify(s => s.Inserir(It.IsAny<EventoCadastroDTO>()), Times.Once);
        }

        [Fact]
        public async Task DadoEventoComTipoFeriado_QuandoExecutar_EntaoProcessaComSucesso()
        {
            var eventoCadastro = CriarEventoCadastroDTO(tipoEvento: TipoEvento.FERIADO, descricao: "Feriado Nacional");
            var mensagemRabbit = CriarMensagemRabbit(eventoCadastro);

            _servicoEventoMock
                .Setup(s => s.Inserir(It.IsAny<EventoCadastroDTO>()))
                .ReturnsAsync(10L);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            _servicoEventoMock.Verify(
                s => s.Inserir(It.Is<EventoCadastroDTO>(e => e.Tipo == TipoEvento.FERIADO)),
                Times.Once);
        }

        [Fact]
        public async Task DadoMensagemComDadosCompletos_QuandoExecutar_EntaoExtraiEventoCorretamente()
        {
            var eventoCadastro = new EventoCadastroDTO(
                data: new DateTime(2026, 12, 25, 10, 30, 0),
                tipoEvento: TipoEvento.FERIADO,
                descricao: "Natal");
            
            var mensagemRabbit = CriarMensagemRabbit(eventoCadastro);

            _servicoEventoMock
                .Setup(s => s.Inserir(It.IsAny<EventoCadastroDTO>()))
                .ReturnsAsync(5L);

            await _useCase.Executar(mensagemRabbit);

            _servicoEventoMock.Verify(
                s => s.Inserir(It.Is<EventoCadastroDTO>(
                    e => e.Dia == 25 && 
                         e.Mes == 12 && 
                         e.Ano == 2026 &&
                         e.Descricao == "Natal")),
                Times.Once);
        }

        [Fact]
        public async Task DadoEventoComIdDefinido_QuandoExecutar_EntaoMantémIdAoProcessar()
        {
            var eventoCadastro = CriarEventoCadastroDTO();
            eventoCadastro.Id = 999;
            
            var mensagemRabbit = CriarMensagemRabbit(eventoCadastro);

            _servicoEventoMock
                .Setup(s => s.Inserir(It.IsAny<EventoCadastroDTO>()))
                .ReturnsAsync(999L);

            await _useCase.Executar(mensagemRabbit);

            _servicoEventoMock.Verify(
                s => s.Inserir(It.Is<EventoCadastroDTO>(e => e.Id == 999)),
                Times.Once);
        }

        [Fact]
        public async Task DadoEventoComAcervoSolicitacaoItemId_QuandoExecutar_EntaoMantémIdAoProcessar()
        {
            var eventoCadastro = CriarEventoCadastroDTO();
            eventoCadastro.AcervoSolicitacaoItemId = 555;
            
            var mensagemRabbit = CriarMensagemRabbit(eventoCadastro);

            _servicoEventoMock
                .Setup(s => s.Inserir(It.IsAny<EventoCadastroDTO>()))
                .ReturnsAsync(1L);

            await _useCase.Executar(mensagemRabbit);

            _servicoEventoMock.Verify(
                s => s.Inserir(It.Is<EventoCadastroDTO>(e => e.AcervoSolicitacaoItemId == 555)),
                Times.Once);
        }

        [Fact]
        public async Task DadoMultiplosEventosSequencial_QuandoExecutarDiversasVezes_EntaoProcessaTodos()
        {
            var eventos = new List<EventoCadastroDTO>
            {
                CriarEventoCadastroDTO(descricao: "Evento 1"),
                CriarEventoCadastroDTO(descricao: "Evento 2"),
                CriarEventoCadastroDTO(descricao: "Evento 3")
            };

            _servicoEventoMock
                .Setup(s => s.Inserir(It.IsAny<EventoCadastroDTO>()))
                .ReturnsAsync(1L);

            foreach (var evento in eventos)
            {
                var mensagemRabbit = CriarMensagemRabbit(evento);
                await _useCase.Executar(mensagemRabbit);
            }

            _servicoEventoMock.Verify(s => s.Inserir(It.IsAny<EventoCadastroDTO>()), Times.Exactly(3));
        }

        #endregion

        #region Testes do Método Executar - Cenários de Erro

        [Fact]
        public async Task DadoServicoEventoLancaExcecao_QuandoExecutar_EntaoExcecaoEhPropagada()
        {
            var eventoCadastro = CriarEventoCadastroDTO();
            var mensagemRabbit = CriarMensagemRabbit(eventoCadastro);
            var mensagemErro = "Erro ao inserir evento";

            _servicoEventoMock
                .Setup(s => s.Inserir(It.IsAny<EventoCadastroDTO>()))
                .ThrowsAsync(new Exception(mensagemErro));

            await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(mensagemRabbit));
        }

        [Fact]
        public async Task DadoMensagemRabbitNula_QuandoExecutar_EntaoLancaExcecao()
        {
            await Assert.ThrowsAsync<NullReferenceException>(() => _useCase.Executar(null!));
        }

        [Fact]
        public async Task DadoEventoCadastroNuloNaMensagem_QuandoExecutar_EntaoLancaExcecao()
        {
            var mensagemRabbit = new MensagemRabbit();

            await Assert.ThrowsAsync<NullReferenceException>(() => _useCase.Executar(mensagemRabbit));
        }

        #endregion

        #region Testes de Integração com Método ObterObjetoMensagem

        [Fact]
        public async Task DadoMensagemComEventoSerializado_QuandoExecutar_EntaoDesserializaCorretamente()
        {
            var eventoCadastro = CriarEventoCadastroDTO(descricao: "Desserializado");
            var mensagemRabbit = CriarMensagemRabbit(eventoCadastro);

            _servicoEventoMock
                .Setup(s => s.Inserir(It.IsAny<EventoCadastroDTO>()))
                .ReturnsAsync(1L)
                .Callback<EventoCadastroDTO>(e => 
                {
                    e.Descricao.Should().Be("Desserializado");
                });

            await _useCase.Executar(mensagemRabbit);

            _servicoEventoMock.Verify(
                s => s.Inserir(It.Is<EventoCadastroDTO>(e => e.Descricao == "Desserializado")),
                Times.Once);
        }

        [Fact]
        public async Task DadoEventoComJustificativa_QuandoExecutar_EntaoMantémJustificativaAoProcessar()
        {
            var eventoCadastro = CriarEventoCadastroDTO();
            eventoCadastro.Justificativa = "Justificativa importante";
            
            var mensagemRabbit = CriarMensagemRabbit(eventoCadastro);

            _servicoEventoMock
                .Setup(s => s.Inserir(It.IsAny<EventoCadastroDTO>()))
                .ReturnsAsync(1L);

            await _useCase.Executar(mensagemRabbit);

            _servicoEventoMock.Verify(
                s => s.Inserir(It.Is<EventoCadastroDTO>(e => e.Justificativa == "Justificativa importante")),
                Times.Once);
        }

        #endregion

        #region Testes de Comportamento Assíncrono

        [Fact]
        public async Task DadoInsercaoAssincrona_QuandoExecutar_EntaoAguardaConclussao()
        {
            var eventoCadastro = CriarEventoCadastroDTO();
            var mensagemRabbit = CriarMensagemRabbit(eventoCadastro);
            var delayMs = 100;

            _servicoEventoMock
                .Setup(s => s.Inserir(It.IsAny<EventoCadastroDTO>()))
                .Returns(async () =>
                {
                    await Task.Delay(delayMs);
                    return 1L;
                });

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var resultado = await _useCase.Executar(mensagemRabbit);
            stopwatch.Stop();

            resultado.Should().BeTrue();
            stopwatch.ElapsedMilliseconds.Should().BeGreaterThanOrEqualTo(delayMs);
        }

        #endregion

        #region Métodos Auxiliares

        private EventoCadastroDTO CriarEventoCadastroDTO(
            TipoEvento tipoEvento = TipoEvento.FERIADO,
            string descricao = "Evento de Teste",
            int? dia = null,
            int? mes = null,
            int? ano = null,
            int? hora = null,
            int? minuto = null)
        {
            var dataAtual = DateTime.Now;
            return new EventoCadastroDTO
            {
                Dia = dia ?? dataAtual.Day,
                Mes = mes ?? dataAtual.Month,
                Ano = ano ?? dataAtual.Year,
                Hora = hora ?? dataAtual.Hour,
                Minuto = minuto ?? dataAtual.Minute,
                Tipo = tipoEvento,
                Descricao = descricao
            };
        }

        private MensagemRabbit CriarMensagemRabbit(EventoCadastroDTO eventoCadastro)
        {
            var mensagemSerializada = JsonConvert.SerializeObject(eventoCadastro);
            return new MensagemRabbit
            {
                Mensagem = mensagemSerializada,
                CodigoCorrelacao = Guid.NewGuid()
            };
        }

        #endregion
    }
}
