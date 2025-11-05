using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoDeConsolidacao(IRepositorioDeConsolidacao repositorioDeConsolidacao) : IServicoDeConsolidacao
    {
        public async Task ConsolidarMesDoHistoricoDeConsultasDoDiaAnteriorAsync()
        {
            var dataOntem = DateTime.UtcNow.AddDays(-1);
            var inicioMes = new DateTime(dataOntem.Year, dataOntem.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var fimMes = inicioMes.AddMonths(1);

            await repositorioDeConsolidacao.ConsolidarMesDoHistoricoDeConsultasAsync(inicioMes, fimMes);
        }
    }
}
