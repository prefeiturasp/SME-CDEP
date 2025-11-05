using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoDeConsolidacaoTestes
    {
        private readonly Mock<IRepositorioDeConsolidacao> _repositorioDeConsultaMock;
        private readonly ServicoDeConsolidacao _servicoDeConsolidacao;

        public ServicoDeConsolidacaoTestes()
        {
            var mocker = new AutoMocker();
            _repositorioDeConsultaMock = mocker.GetMock<IRepositorioDeConsolidacao>();
            _servicoDeConsolidacao = mocker.CreateInstance<ServicoDeConsolidacao>();
        }

        [Fact]
        public async Task DadoQualquerCenario_QuandoConsolidarMesDoHistoricoDeConsultasDoDiaAnteriorAsync_EntaoDeveChamarRepositorioUmaVez()
        {
            // Act
            await _servicoDeConsolidacao.ConsolidarMesDoHistoricoDeConsultasDoDiaAnteriorAsync();
            // Assert
            _repositorioDeConsultaMock.Verify(r => r.ConsolidarMesDoHistoricoDeConsultasAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }
    }
}
