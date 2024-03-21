using Bogus;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteIntegracao;

public class AcervoFotograficoMock : AcervoMock
{
    private static AcervoFotograficoMock _instance;
    public static AcervoFotograficoMock Instance => _instance ??= new();
    
    public Faker<AcervoFotografico> Gerar()
    {
        var random = new Random();
        var faker = new Faker<AcervoFotografico>("pt_BR");
            
        faker.RuleFor(x => x.Procedencia, f => f.Lorem.Text().Limite(200));
        faker.RuleFor(x => x.Largura, f => "10.20");
        faker.RuleFor(x => x.Altura, f => "50,45");
        faker.RuleFor(x => x.ConservacaoId, f => random.Next(1,5));
        faker.RuleFor(x => x.Quantidade, f => random.Next(15,55));
        faker.RuleFor(x => x.Localizacao, f => f.Company.Locale);
        faker.RuleFor(x => x.CopiaDigital, f => true);
        faker.RuleFor(x => x.PermiteUsoImagem, f => true);
        faker.RuleFor(x => x.SuporteId, f => random.Next(1,5));
        faker.RuleFor(x => x.FormatoId, f => random.Next(1,5));
        faker.RuleFor(x => x.CromiaId, f => random.Next(1,2));
        faker.RuleFor(x => x.Resolucao, f => f.Address.Random.ToString().Limite(15));
        faker.RuleFor(x => x.TamanhoArquivo, f => f.Address.Random.ToString().Limite(15));
        faker.RuleFor(x => x.Acervo, f => Gerar(TipoAcervo.Fotografico).Generate());
        return faker;
    }
}