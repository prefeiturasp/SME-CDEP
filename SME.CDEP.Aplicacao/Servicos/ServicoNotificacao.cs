using SME.CDEP.Aplicacao.Servicos.Interface;

namespace SME.CDEP.Aplicacao.Servicos
{
    public abstract class ServicoNotificacao : IServicoNotificacao
    {
        public abstract Task<bool> Enviar();
    }
}
