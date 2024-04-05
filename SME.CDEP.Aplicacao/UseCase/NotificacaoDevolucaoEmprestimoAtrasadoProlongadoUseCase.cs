using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.Aplicacao
{
    public class NotificacaoDevolucaoEmprestimoAtrasadoProlongadoUseCase : INotificacaoDevolucaoEmprestimoAtrasadoProlongadoUseCase
    {
        private IServicoAcervoEmprestimo servicoEventoAcervoEmprestimo;
        
        public NotificacaoDevolucaoEmprestimoAtrasadoProlongadoUseCase(IServicoAcervoEmprestimo servicoEventoAcervoEmprestimo)
        {
            this.servicoEventoAcervoEmprestimo = servicoEventoAcervoEmprestimo ?? throw new ArgumentNullException(nameof(servicoEventoAcervoEmprestimo));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await servicoEventoAcervoEmprestimo.NotificarDevolucaoEmprestimoAtrasadoProlongado();
            return true;
        }
    }
}