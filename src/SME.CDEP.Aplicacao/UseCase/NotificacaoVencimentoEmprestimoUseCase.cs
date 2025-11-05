
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.Aplicacao
{
    public class NotificacaoVencimentoEmprestimoUseCase : INotificacaoVencimentoEmprestimoUseCase
    {
        private IServicoAcervoEmprestimo servicoEventoAcervoEmprestimo;
        
        public NotificacaoVencimentoEmprestimoUseCase(IServicoAcervoEmprestimo servicoEventoAcervoEmprestimo)
        {
            this.servicoEventoAcervoEmprestimo = servicoEventoAcervoEmprestimo ?? throw new ArgumentNullException(nameof(servicoEventoAcervoEmprestimo));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await servicoEventoAcervoEmprestimo.NotificarVencimentoEmprestimo();
            return true;
        }
    }
}