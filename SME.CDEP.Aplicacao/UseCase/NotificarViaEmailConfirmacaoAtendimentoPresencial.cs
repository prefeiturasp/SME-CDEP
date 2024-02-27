
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.Aplicacao
{
    public class NotificarViaEmailConfirmacaoAtendimentoPresencial : INotificarViaEmailConfirmacaoAtendimentoPresencial
    {
        private IServicoEvento servicoEvento;
        
        public NotificarViaEmailConfirmacaoAtendimentoPresencial(IServicoEvento servicoEvento)
        {
            this.servicoEvento = servicoEvento ?? throw new ArgumentNullException(nameof(servicoEvento));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            // await servicoEvento.GerarEventosFixos();
            // await servicoEvento.GerarEventosMoveis();
            return true;
        }
    }
}