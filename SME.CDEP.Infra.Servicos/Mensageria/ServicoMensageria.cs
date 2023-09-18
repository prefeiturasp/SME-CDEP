using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using System.Text;
using SME.CDEP.Infra.Servicos.Polly;

namespace SME.CDEP.Infra.Servicos.Mensageria
{
    public class ServicoMensageria : IServicoMensageria
    {
        private readonly IConexoesRabbitLogs conexoesRabbit;
        private readonly IAsyncPolicy policy;

        public ServicoMensageria(IConexoesRabbitLogs conexoesRabbit, IReadOnlyPolicyRegistry<string> registry)
        {
            this.conexoesRabbit = conexoesRabbit ?? throw new ArgumentNullException(nameof(conexoesRabbit));
            this.policy = registry.Get<IAsyncPolicy>(ConstsPoliticaPolly.PublicaFila);
        }

        public async Task Enviar(string mensagem, string rota, string exchange)
        {
            var logMensagem = JsonConvert.SerializeObject(new MensagemRabbit(mensagem),
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var body = Encoding.UTF8.GetBytes(logMensagem);

            await policy.ExecuteAsync(async () 
                => await PublicarMensagem(rota, body, exchange));
        }

        private Task PublicarMensagem(string rota, byte[] body, string exchange = null)
        {
            var channel = conexoesRabbit.Get();
            try
            {
                var props = channel.CreateBasicProperties();
                props.Persistent = true;

                channel.BasicPublish(exchange, rota, true, props, body);
            }
            finally
            {
                conexoesRabbit.Return(channel);
            }

            return Task.CompletedTask;
        }
    }
}
