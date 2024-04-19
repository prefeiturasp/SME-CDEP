using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using SME.CDEP.Infra.Servicos.Options;

namespace SME.CDEP.Infra.Servicos.Mensageria
{
    public class ConexoesRabbit : IConexoesRabbit
    {
        public ObjectPool<IModel> pool { get; set; }

        protected ConexoesRabbit(ConfiguracaoRabbit configuracaoRabbit, ObjectPoolProvider poolProvider)
        {
            var policy = new RabbitModelPooledObjectPolicy(configuracaoRabbit);

            pool = poolProvider.Create(policy);
        }

        public IModel Get()
            => pool.Get();

        public void Return(IModel conexao)
            => pool.Return(conexao);
    }


    public class ConexoesRabbitAcessos : ConexoesRabbit, IConexoesRabbitAcessos
    {
        public ConexoesRabbitAcessos(ConfiguracaoRabbitOptions configuracaoRabbit, ObjectPoolProvider poolProvider) : base(configuracaoRabbit, poolProvider)
        { }
    }

    public class ConexoesRabbitLogs : ConexoesRabbit, IConexoesRabbitLogs
    {
        public ConexoesRabbitLogs(ConfiguracaoRabbitLogsOptions configuracaoRabbit, ObjectPoolProvider poolProvider) : base(configuracaoRabbit, poolProvider)
        { }
    }

}
