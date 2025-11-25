using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoDeConsolidacao(IRepositorioDeConsolidacao repositorioDeConsolidacao,
        TimeProvider timeProvider) : IServicoDeConsolidacao
    {

        public async Task ConsolidarMesDoHistoricoDeConsultasDoDiaAnteriorAsync()
        {
            var (inicioMes, fimMes) = ObterIntervaloDoMesDeOntem();

            await repositorioDeConsolidacao.ConsolidarMesDoHistoricoDeConsultasAsync(inicioMes, fimMes);
        }
        public async Task ConsolidarMesDasSolicitacoesDeAcervosDoDiaAnteriorAsync()
        {
            var (inicioMes, fimMes) = ObterIntervaloDoMesDeOntem();
            await repositorioDeConsolidacao.ConsolidarMesDasSolicitacoesDeAcervosAsync(inicioMes, fimMes);
        }
        private (DateTime Inicio, DateTime Fim) ObterIntervaloDoMesDeOntem()
        {
            var dataOntem = timeProvider.GetUtcNow().UtcDateTime.AddDays(-1);
            var inicioMes = new DateTime(dataOntem.Year, dataOntem.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var fimMes = inicioMes.AddMonths(1);

            return (inicioMes, fimMes);
        }
    }
}
