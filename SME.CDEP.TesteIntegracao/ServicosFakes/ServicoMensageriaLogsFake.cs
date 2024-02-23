using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using System.Text;
using SME.CDEP.Infra.Servicos.Mensageria;
using SME.CDEP.Infra.Servicos.Polly;

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
