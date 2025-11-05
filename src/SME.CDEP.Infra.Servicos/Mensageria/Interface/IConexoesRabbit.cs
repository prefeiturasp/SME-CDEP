using RabbitMQ.Client;

namespace SME.CDEP.Infra.Servicos.Mensageria
{
    public interface IConexoesRabbit
    {
        IModel Get();
        void Return(IModel conexao);
    }

    public interface IConexoesRabbitAcessos : IConexoesRabbit { }
    public interface IConexoesRabbitLogs : IConexoesRabbit { }
}
