using RabbitMQ.Client;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.Infra.Servicos.Mensageria
{
    public interface IServicoMensageriaCDEP
    {
        Task<bool> Publicar(MensagemRabbit mensagem, string rota, string exchange, string nomeAcao, IModel canalRabbit = null);
    }
}
