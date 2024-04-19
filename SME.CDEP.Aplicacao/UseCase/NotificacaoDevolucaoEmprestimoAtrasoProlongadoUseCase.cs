using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.Aplicacao
{
    public class NotificacaoDevolucaoEmprestimoAtrasoProlongadoUseCase : INotificacaoDevolucaoEmprestimoAtrasoProlongadoUseCase
    {
        private IServicoAcervoEmprestimo servicoEventoAcervoEmprestimo;
        
        public NotificacaoDevolucaoEmprestimoAtrasoProlongadoUseCase(IServicoAcervoEmprestimo servicoEventoAcervoEmprestimo)
        {
            this.servicoEventoAcervoEmprestimo = servicoEventoAcervoEmprestimo ?? throw new ArgumentNullException(nameof(servicoEventoAcervoEmprestimo));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await servicoEventoAcervoEmprestimo.NotificarDevolucaoEmprestimoAtrasoProlongado();
            return true;
        }
    }
}