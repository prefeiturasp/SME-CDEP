using Bogus;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteIntegracao;

public class EventoMock : AuditoriaMock
{
    private static EventoMock _instance;
    public static EventoMock Instance => _instance ??= new();
    
    public Faker<SME.CDEP.Dominio.Entidades.Evento> GerarEvento(TipoEvento tipoEvento)
    {
        var faker = new Faker<SME.CDEP.Dominio.Entidades.Evento>("pt_BR");
            
        faker.RuleFor(x => x.Data, f => f.Date.Future().Date);
        faker.RuleFor(x => x.Descricao, f => f.Lorem.Text().Limite(200));
        faker.RuleFor(x => x.Justificativa, f => f.Address.FullAddress().Limite(200));
        faker.RuleFor(x => x.Tipo, f => tipoEvento);
        AuditoriaFaker(faker);
        return faker;
    }
}