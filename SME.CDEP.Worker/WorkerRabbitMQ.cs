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
            
            Comandos.Add(RotasRabbit.NotificarViaEmailCancelamentoAtendimento, new ComandoRabbit("Notifica usuário solicitate do acervo sobre o cancelamento do atendimento", typeof(INotificarViaEmailCancelamentoAtendimentoUseCase), true));
            Comandos.Add(RotasRabbit.NotificarViaEmailCancelamentoAtendimentoItem, new ComandoRabbit("Notifica usuário solicitate do acervo sobre o cancelamento do item do atendimento", typeof(INotificarViaEmailCancelamentoAtendimentoItemUseCase), true));
            Comandos.Add(RotasRabbit.NotificarViaEmailConfirmacaoAtendimentoPresencial, new ComandoRabbit("Notifica usuário solicitate do acervo sobre a confirmação de atendimentos presenciais", typeof(INotificarViaEmailConfirmacaoAtendimentoPresencialUseCase), true));
            
            Comandos.Add(RotasRabbit.ExecutarAtualizacaoSituacaoParaEmprestimoComDevolucaoEmAtraso, new ComandoRabbit("Atualizar situação para empréstimo com devolução em atraso", typeof(IExecutarAtualizacaoSituacaoParaEmprestimoComDevolucaoEmAtrasoUseCase), true));
            
            Comandos.Add(RotasRabbit.NotificacaoVencimentoEmprestimo, new ComandoRabbit("Notificação de vencimento de empréstimo", typeof(INotificacaoVencimentoEmprestimoUseCase), true));
            Comandos.Add(RotasRabbit.NotificacaoVencimentoEmprestimoUsuario, new ComandoRabbit("Notificação usuário sobre o vencimento de empréstimo", typeof(INotificacaoVencimentoEmprestimoUsuarioUseCase), true));
            
            Comandos.Add(RotasRabbit.NotificacaoDevolucaoEmprestimoAtrasado, new ComandoRabbit("Notificação de empréstimo atrasado", typeof(INotificacaoDevolucaoEmprestimoAtrasadoUseCase), true));
            Comandos.Add(RotasRabbit.NotificacaoDevolucaoEmprestimoAtrasadoUsuario, new ComandoRabbit("Notificação usuário sobre o empréstimo atrasado", typeof(INotificacaoDevolucaoEmprestimoAtrasadoUsuarioUseCase), true));
            
            Comandos.Add(RotasRabbit.NotificacaoDevolucaoEmprestimoAtrasoProlongado, new ComandoRabbit("Notificação de empréstimo com atraso prolongado", typeof(INotificacaoDevolucaoEmprestimoAtrasoProlongadoUseCase), true));
            Comandos.Add(RotasRabbit.NotificacaoDevolucaoEmprestimoAtrasoProlongadoUsuario, new ComandoRabbit("Notificação usuário sobre o empréstimo em atraso prolongado", typeof(INotificacaoDevolucaoEmprestimoAtrasadoProlongadoUsuarioUseCase), true));
        }
    }
}