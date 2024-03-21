using Bogus;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteIntegracao;

public class AcervoArteGraficaMock : AcervoMock
{
    private static AcervoArteGraficaMock _instance;
    public static AcervoArteGraficaMock Instance => _instance ??= new();
    
    public Faker<AcervoArteGrafica> Gerar()
    {
        var random = new Random();
        var faker = new Faker<AcervoArteGrafica>("pt_BR");
            
        faker.RuleFor(x => x.Localizacao, f => f.Lorem.Text().Limite(100));
        faker.RuleFor(x => x.Procedencia, f => f.Lorem.Text().Limite(200));
        faker.RuleFor(x => x.CopiaDigital, f => true);
        faker.RuleFor(x => x.PermiteUsoImagem, f => true);
        faker.RuleFor(x => x.Largura, f => "10.20");
        faker.RuleFor(x => x.Altura, f => "50,45");
        faker.RuleFor(x => x.ConservacaoId, f => random.Next(1,5));
        faker.RuleFor(x => x.CromiaId, f => random.Next(1,5));
        faker.RuleFor(x => x.Diametro, f => "15,55");
        faker.RuleFor(x => x.Tecnica, f => f.Lorem.Sentence().Limite(100));
        faker.RuleFor(x => x.SuporteId, f => random.Next(1,5));
        faker.RuleFor(x => x.Quantidade, f => random.Next(15,55));
        faker.RuleFor(x => x.Acervo, f => Gerar(TipoAcervo.ArtesGraficas).Generate());
        return faker;
    }
}