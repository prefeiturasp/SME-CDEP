using Bogus;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteIntegracao;

public class ParametroSistemaMock : AuditoriaMock
{
    private static ParametroSistemaMock _instance;
    public static ParametroSistemaMock Instance => _instance ??= new();
    
    public ParametroSistema GerarParametroSistema(TipoParametroSistema? tipoParametroSistema = null, string? valor = null)
    {
        var faker = new Faker<ParametroSistema>();

        faker.RuleFor(x => x.Nome, f => f.Name.FullName());
        faker.RuleFor(x => x.Descricao, f => f.Name.FullName());
        faker.RuleFor(x => x.Tipo, f => tipoParametroSistema.HasValue ? tipoParametroSistema : f.PickRandom<TipoParametroSistema>());
        faker.RuleFor(x => x.Valor, f => string.IsNullOrEmpty(valor) ? f.Random.Word() : valor);
        faker.RuleFor(x => x.Ano, f => DateTimeExtension.HorarioBrasilia().Year);
        faker.RuleFor(x => x.Ativo, true);

        AuditoriaFaker(faker);

        return faker.Generate();
    }
}