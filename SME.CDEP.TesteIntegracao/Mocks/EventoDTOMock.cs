using Bogus;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteIntegracao;

public static class EventoDTOMock
{
    public static Faker<EventoDTO> GerarEventoDTO(TipoEvento tipoEvento)
    {
        var faker = new Faker<EventoDTO>("pt_BR");
            
        faker.RuleFor(x => x.Data, f => f.Date.Future().Date);
        faker.RuleFor(x => x.Descricao, f => f.Lorem.Text().Limite(200));
        faker.RuleFor(x => x.Justificativa, f => f.Address.FullAddress().Limite(200));
        faker.RuleFor(x => x.Tipo, f => tipoEvento);
        return faker;
    }
}