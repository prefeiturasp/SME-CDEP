using RabbitMQ.Client;
using SME.CDEP.Infra.Servicos.Mensageria;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SSME.CDEP.TesteIntegracao.ServicosFakes
{
    public class ServicoMensageriaCDEPFake : IServicoMensageriaCDEP
    {
        public Task<bool> Publicar(MensagemRabbit mensagem, string rota, string exchange, string nomeAcao, IModel canalRabbit = null)
        {
            return Task.FromResult(true);
        }
    }
}
