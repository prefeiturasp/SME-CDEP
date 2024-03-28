using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.CDEP.Aplicacao;
using SME.CDEP.Infra;
using SME.CDEP.Infra.Servicos.Mensageria;
using SME.CDEP.Infra.Servicos.Telemetria;
using SME.CDEP.Infra.Servicos.Telemetria.Options;

namespace SME.CDEP.Worker
{
    public sealed class WorkerRabbitMQ : WorkerRabbitCDEP
    {
        public WorkerRabbitMQ(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaCDEP servicoMensageria,
            IServicoMensageriaMetricas servicoMensageriaMetricas,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory)
            : base(serviceScopeFactory, servicoTelemetria, servicoMensageria, servicoMensageriaMetricas, telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitMQ", typeof(RotasRabbit))
        {
            RegistrarUseCases();
        }

        protected override void RegistrarUseCases()
        {
            Comandos.Add(RotasRabbit.ExecutarCriacaoDeEventosTipoFeriadoAnoAtual, new ComandoRabbit("Executar a criação de eventos do tipo feriado para o ano atual", typeof(IExecutarCriacaoDeEventosTipoFeriadoAnoAtualUseCase), true));
            Comandos.Add(RotasRabbit.ExecutarCriacaoDeEventosTipoFeriadoAnoAtualPorData, new ComandoRabbit("Executar a criação de eventos do tipo feriado para o ano atual por data", typeof(IExecutarCriacaoDeEventosTipoFeriadoAnoAtualPorDataUseCase), true));
            
            Comandos.Add(RotasRabbit.NotificarViaEmailCancelamentoAtendimento, new ComandoRabbit("Notifica usuário solicitate do acervo sobre o cancelamento do atendimento", typeof(INotificarViaEmailCancelamentoAtendimento), true));
            Comandos.Add(RotasRabbit.NotificarViaEmailCancelamentoAtendimentoItem, new ComandoRabbit("Notifica usuário solicitate do acervo sobre o cancelamento do item do atendimento", typeof(INotificarViaEmailCancelamentoAtendimentoItem), true));
            Comandos.Add(RotasRabbit.NotificarViaEmailConfirmacaoAtendimentoPresencial, new ComandoRabbit("Notifica usuário solicitate do acervo sobre a confirmação de atendimentos presenciais", typeof(INotificarViaEmailConfirmacaoAtendimentoPresencial), true));
            
            Comandos.Add(RotasRabbit.ExecutarAtualizacaoSituacaoParaEmprestimoComDevolucaoEmAtraso, new ComandoRabbit("Atualizar situação para empréstimo com devolução em atraso", typeof(IExecutarAtualizacaoSituacaoParaEmprestimoComDevolucaoEmAtrasoUseCase), true));
        }
    }
}