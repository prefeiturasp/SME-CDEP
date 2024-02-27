
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoNotificacaoEmail
    {
        Task<bool> Enviar(string nomeDestinatario, string emailDestinatario, string assunto, string mensagem);
        Task NotificarCancelamentoAtendimento(IEnumerable<AcervoSolicitacaoItemDetalhe> detalhesAcervo);
    }
}
