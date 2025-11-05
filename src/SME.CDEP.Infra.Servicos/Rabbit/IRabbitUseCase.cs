
using SME.CDEP.Dominio;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.Infra
{
    public interface IRabbitUseCase : IUseCase<MensagemRabbit, bool>
    {
    }
}
