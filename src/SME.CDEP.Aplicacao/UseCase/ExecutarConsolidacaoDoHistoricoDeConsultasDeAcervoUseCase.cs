
using Microsoft.Extensions.Logging;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.Aplicacao
{
    public class ExecutarConsolidacaoDoHistoricoDeConsultasDeAcervoUseCase 
        (IServicoDeConsolidacao servicoDeConsolidacao, 
         ILogger<ExecutarConsolidacaoDoHistoricoDeConsultasDeAcervoUseCase> logger) : 
        IExecutarConsolidacaoDoHistoricoDeConsultasDeAcervoUseCase
    {
        public async Task<bool> Executar(MensagemRabbit param)
        {
            logger.LogInformation("Iniciando a consolidação do histórico de consultas de acervo do dia anterior.");
            await servicoDeConsolidacao.ConsolidarMesDoHistoricoDeConsultasDoDiaAnteriorAsync();
            logger.LogInformation("Consolidação do histórico de consultas de acervo do dia anterior finalizada com sucesso.");
            return true;
        }
    }
}