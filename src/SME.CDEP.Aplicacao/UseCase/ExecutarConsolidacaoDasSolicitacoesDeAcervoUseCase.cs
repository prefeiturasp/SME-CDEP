using Microsoft.Extensions.Logging;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Aplicacao.UseCase.Interface;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.Aplicacao.UseCase
{
    public class ExecutarConsolidacaoDasSolicitacoesDeAcervoUseCase
        (IServicoDeConsolidacao servicoDeConsolidacao,
         ILogger<ExecutarConsolidacaoDasSolicitacoesDeAcervoUseCase> logger) : IExecutarConsolidacaoDasSolicitacoesDeAcervoUseCase
    {
        public async Task<bool> Executar(MensagemRabbit param)
        {
            logger.LogInformation("Iniciando a consolidação das solicitações de acervo do dia anterior.");
            await servicoDeConsolidacao.ConsolidarMesDasSolicitacoesDeAcervosDoDiaAnteriorAsync();
            logger.LogInformation("Consolidação das solicitações de acervo do dia anterior finalizada com sucesso.");
            return true;
        }
    }
}
