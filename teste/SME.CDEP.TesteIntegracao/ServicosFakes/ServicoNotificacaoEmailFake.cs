using SME.CDEP.Aplicacao.Servicos.Interface;

namespace SSME.CDEP.TesteIntegracao.ServicosFakes
{
    public class ServicoNotificacaoEmailFake : IServicoNotificacaoEmail
    {
       public Task<bool> Enviar(string nomeDestinatario, string emailDestinatario, string assunto, string mensagem)
        {
            return Task.FromResult(true);
        }
    }
}
