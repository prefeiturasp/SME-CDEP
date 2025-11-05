using Bogus;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteIntegracao;

public class AcervoTridimensionalMock : AcervoMock
{
    private static AcervoTridimensionalMock _instance;
    public static AcervoTridimensionalMock Instance => _instance ??= new();
    
    public Faker<AcervoTridimensional> Gerar()
    {
        var random = new Random();
        var faker = new Faker<AcervoTridimensional>("pt_BR");
            
        faker.RuleFor(x => x.Procedencia, f => f.Lorem.Text().Limite(200));
        faker.RuleFor(x => x.Largura, f => "10.20");
        faker.RuleFor(x => x.Altura, f => "50,45");
        faker.RuleFor(x => x.ConservacaoId, f => random.Next(1,5));
        faker.RuleFor(x => x.Diametro, f => "15,55");
        faker.RuleFor(x => x.Quantidade, f => random.Next(15,55));
        faker.RuleFor(x => x.Profundidade, f => "12,15");
        faker.RuleFor(x => x.Acervo, f => Gerar(TipoAcervo.Tridimensional).Generate());
        return faker;
    }
}