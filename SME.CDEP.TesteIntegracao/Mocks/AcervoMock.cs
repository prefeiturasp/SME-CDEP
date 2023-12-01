using Bogus;
using SME.CDEP.Dominio;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteIntegracao;

public class AcervoMock : AuditoriaMock
{
    private static AcervoMock _instance;
    public static AcervoMock Instance => _instance ??= new();
    
    public Faker<Acervo> GerarAcervo(TipoAcervo tipoAcervo)
    {
        var random = new Random();
        var faker = new Faker<Acervo>("pt_BR");
        faker.RuleFor(x => x.Titulo, f => f.Lorem.Text().Limite(500));
        faker.RuleFor(x => x.Descricao, f => f.Lorem.Text());
        faker.RuleFor(x => x.Codigo, f => random.Next(1,499).ToString());
            
        if (((long)tipoAcervo).EhAcervoDocumental())
            faker.RuleFor(x => x.CodigoNovo, f => random.Next(500,999).ToString());
            
        faker.RuleFor(x => x.SubTitulo, f => f.Lorem.Text().Limite(500));
        faker.RuleFor(x => x.TipoAcervoId, f => (int)tipoAcervo);
        AuditoriaFaker(faker);
        return faker;
    }
}