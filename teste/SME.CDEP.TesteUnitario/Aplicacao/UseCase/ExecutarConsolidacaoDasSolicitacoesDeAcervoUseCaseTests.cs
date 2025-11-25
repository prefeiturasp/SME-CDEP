using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Aplicacao.UseCase;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase
{
    public class ExecutarConsolidacaoDasSolicitacoesDeAcervoUseCaseTests
    {
        private readonly Mock<IServicoDeConsolidacao> _servicoDeConsolidacaoMock;
        private readonly ExecutarConsolidacaoDasSolicitacoesDeAcervoUseCase _useCase;

        public ExecutarConsolidacaoDasSolicitacoesDeAcervoUseCaseTests()
        {
            var mocker = new AutoMocker();
            _servicoDeConsolidacaoMock = mocker.GetMock<IServicoDeConsolidacao>();
            _useCase = mocker.CreateInstance<ExecutarConsolidacaoDasSolicitacoesDeAcervoUseCase>();
        }

        [Fact]
        public async Task DadoQueMensagemRabbitSejaValida_QuandoExecutar_EntaoDeveChamarServicoDeConsolidacaoERetornarVerdadeiro()
        {
            // Act
            await _useCase.Executar(new());
            // Assert
            _servicoDeConsolidacaoMock.Verify(s => s.ConsolidarMesDasSolicitacoesDeAcervosDoDiaAnteriorAsync(), Times.Once);
        }
    }
}
