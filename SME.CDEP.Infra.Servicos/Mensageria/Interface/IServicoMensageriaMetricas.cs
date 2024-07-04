using RabbitMQ.Client;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.Infra.Servicos.Mensageria
{
    public interface IServicoMensageriaMetricas
    {
        Task<bool> Publicar(MetricaMensageria mensagem, string rota, string exchange, string nomeAcao, IModel canalRabbit = null);
        Task Publicado(string rota);
        Task Obtido(string rota);
        Task Concluido(string rota);
        Task Erro(string rota);
    }
}
