using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using SME.CDEP.Infra.Servicos.Options;

namespace SME.CDEP.Infra.Servicos.Mensageria
{
    public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly IConnection conexao;

        public RabbitModelPooledObjectPolicy(ConfiguracaoRabbit configuracaoRabbitOptions)
        {
            conexao = GetConnection(configuracaoRabbitOptions ?? throw new ArgumentNullException(nameof(configuracaoRabbitOptions)));
        }
        private IConnection GetConnection(ConfiguracaoRabbit configuracaoRabbit)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "10.50.1.209",
                UserName = "usr_amcom",
                Password = "AMcom20anos",
                VirtualHost = "hom"
            };

            return factory.CreateConnection();
        }

        public IModel Create()
        {
            var channel = conexao.CreateModel();
            channel.ConfirmSelect();
            return channel;
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
                return true;
            else
            {
                obj?.Dispose();
                return false;
            }
        }
    }
}
