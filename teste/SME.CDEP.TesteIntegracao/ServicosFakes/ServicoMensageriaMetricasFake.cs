using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using System.Text;
using RabbitMQ.Client;
using SME.CDEP.Infra.Servicos.Mensageria;
using SME.CDEP.Infra.Servicos.Polly;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SSME.CDEP.TesteIntegracao.ServicosFakes
{
    public class ServicoMensageriaMetricasFake : IServicoMensageriaMetricas
    {
        public Task<bool> Publicar(MetricaMensageria mensagem, string rota, string exchange, string nomeAcao, IModel canalRabbit = null)
        {
            return Task.FromResult(true);
        }

        public Task Publicado(string rota)
        {
            return Task.CompletedTask;
        }

        public Task Obtido(string rota)
        {
            return Task.CompletedTask;
        }

        public Task Concluido(string rota)
        {
            return Task.CompletedTask;
        }

        public Task Erro(string rota)
        {
            return Task.CompletedTask;
        }
    }
}
