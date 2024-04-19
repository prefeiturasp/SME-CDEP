using SME.CDEP.Infra.Servicos.Mensageria;

namespace SSME.CDEP.TesteIntegracao.ServicosFakes
{
    public class ServicoMensageriaLogsFake : IServicoMensageriaLogs
    {
        public async Task Enviar(string mensagem, string rota, string exchange)
        {
            await PublicarMensagem(rota, null, exchange);
        }

        private Task PublicarMensagem(string rota, byte[] body, string exchange = null)
        {
            return Task.CompletedTask;
        }
    }
}
