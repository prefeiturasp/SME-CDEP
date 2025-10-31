using Bogus;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteIntegracao;

public static class EventoCadastroDTO
{
    public static Faker<Aplicacao.DTOS.EventoCadastroDTO> GerarEventoDTO(TipoEvento tipoEvento)
    {
        var faker = new Faker<Aplicacao.DTOS.EventoCadastroDTO>("pt_BR");
            
        faker.RuleFor(x => x.Dia, f => f.Date.Future().Day);
        faker.RuleFor(x => x.Mes, f => f.Date.Future(). Month);
        faker.RuleFor(x => x.Descricao, f => f.Lorem.Text().Limite(200));
        faker.RuleFor(x => x.Justificativa, f => f.Address.FullAddress().Limite(200));
        faker.RuleFor(x => x.Tipo, f => tipoEvento);
        return faker;
    }
}