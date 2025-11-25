using Microsoft.Extensions.Time.Testing;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoDeConsolidacaoTests
    {
        private readonly Mock<IRepositorioDeConsolidacao> _repositorioDeConsultaMock;
        private readonly FakeTimeProvider _fakeTimeProvider;
        private readonly ServicoDeConsolidacao _servicoDeConsolidacao;

        public ServicoDeConsolidacaoTests()
        {
            var mocker = new AutoMocker();
            _fakeTimeProvider = new();
            _fakeTimeProvider.SetLocalTimeZone(TimeZoneInfo.Local);
            _repositorioDeConsultaMock = mocker.GetMock<IRepositorioDeConsolidacao>();
            mocker.Use<TimeProvider>(_fakeTimeProvider);
            _servicoDeConsolidacao = mocker.CreateInstance<ServicoDeConsolidacao>();
        }

        [Fact]
        public async Task DadoQueDataAtualSeja15DeNovembro_QuandoConsolidarHistoricoDeConsultas_EntaoDeveProcessarMesDeNovembro()
        {
            // Arrange
            var dataAtual = new DateTimeOffset(2025, 11, 15, 10, 0, 0, TimeSpan.Zero);
            _fakeTimeProvider.SetUtcNow(dataAtual);

            var inicioEsperado = new DateTime(2025, 11, 1, 0, 0, 0, DateTimeKind.Utc);
            var fimEsperado = new DateTime(2025, 12, 1, 0, 0, 0, DateTimeKind.Utc);

            // Act
            await _servicoDeConsolidacao.ConsolidarMesDoHistoricoDeConsultasDoDiaAnteriorAsync();

            // Assert
            _repositorioDeConsultaMock.Verify(r => r.ConsolidarMesDoHistoricoDeConsultasAsync(inicioEsperado, fimEsperado), Times.Once);
        }

        [Fact]
        public async Task DadoQueDataAtualSejaPrimeiroDeJaneiro_QuandoConsolidarSolicitacoesDeAcervos_EntaoDeveProcessarMesDeDezembroDoAnoAnterior()
        {
            // Arrange
            var dataSimulada = new DateTimeOffset(2024, 01, 01, 10, 0, 0, TimeSpan.Zero);
            _fakeTimeProvider.SetUtcNow(dataSimulada);

            var inicioEsperado = new DateTime(2023, 12, 1, 0, 0, 0, DateTimeKind.Utc);
            var fimEsperado = new DateTime(2024, 01, 1, 0, 0, 0, DateTimeKind.Utc);

            // Act
            await _servicoDeConsolidacao.ConsolidarMesDasSolicitacoesDeAcervosDoDiaAnteriorAsync();

            // Assert
            _repositorioDeConsultaMock
                .Verify(r => r.ConsolidarMesDasSolicitacoesDeAcervosAsync(inicioEsperado, fimEsperado), Times.Once);
        }
    }
}
