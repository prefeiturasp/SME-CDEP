
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.Aplicacao
{
    public class NotificacaoDevolucaoEmprestimoAtrasadoUseCase : INotificacaoDevolucaoEmprestimoAtrasadoUseCase
    {
        private IServicoAcervoEmprestimo servicoEventoAcervoEmprestimo;
        
        public NotificacaoDevolucaoEmprestimoAtrasadoUseCase(IServicoAcervoEmprestimo servicoEventoAcervoEmprestimo)
        {
            this.servicoEventoAcervoEmprestimo = servicoEventoAcervoEmprestimo ?? throw new ArgumentNullException(nameof(servicoEventoAcervoEmprestimo));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await servicoEventoAcervoEmprestimo.NotificarDevolucaoEmprestimoAtrasado();
            return true;
        }
    }
}