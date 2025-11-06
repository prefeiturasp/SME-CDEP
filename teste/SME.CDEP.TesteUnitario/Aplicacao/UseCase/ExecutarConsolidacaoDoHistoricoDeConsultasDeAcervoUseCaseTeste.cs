using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao;
using SME.CDEP.Aplicacao.Servicos.Interface;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase
{
    public class ExecutarConsolidacaoDoHistoricoDeConsultasDeAcervoUseCaseTeste
    {
        private readonly Mock<IServicoDeConsolidacao> _servicoDeConsolidacaoMock;
        private readonly ExecutarConsolidacaoDoHistoricoDeConsultasDeAcervoUseCase _useCase;

        public ExecutarConsolidacaoDoHistoricoDeConsultasDeAcervoUseCaseTeste()
        {
            var mocker = new AutoMocker();
            _servicoDeConsolidacaoMock = mocker.GetMock<IServicoDeConsolidacao>();
            _useCase = mocker.CreateInstance<ExecutarConsolidacaoDoHistoricoDeConsultasDeAcervoUseCase>();
        }

        [Fact]
        public async Task DadoQualquerCenario_QuandoExecutarAsync_EntaoDeveChamarServicoUmaVez()
        {
            // Act
            await _useCase.Executar(new());
            // Assert
            _servicoDeConsolidacaoMock.Verify(s => s.ConsolidarMesDoHistoricoDeConsultasDoDiaAnteriorAsync(), Times.Once);
        }
    }
}
