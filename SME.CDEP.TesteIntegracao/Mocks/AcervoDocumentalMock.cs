using Bogus;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteIntegracao;

public class AcervoDocumentalMock : AcervoMock
{
    private static AcervoDocumentalMock _instance;
    public static AcervoDocumentalMock Instance => _instance ??= new();
    
    public Faker<AcervoDocumental> Gerar()
    {
        var random = new Random();
        var faker = new Faker<AcervoDocumental>("pt_BR");
            
        faker.RuleFor(x => x.Largura, f => "10.20");
        faker.RuleFor(x => x.Altura, f => "50,45");
        faker.RuleFor(x => x.ConservacaoId, f => random.Next(1,5));
        faker.RuleFor(x => x.Localizacao, f => f.Company.Locale);
        faker.RuleFor(x => x.CopiaDigital, f => true);
        faker.RuleFor(x => x.TipoAnexo, f => "PDF");
        faker.RuleFor(x => x.MaterialId, f => random.Next(1,5));
        faker.RuleFor(x => x.IdiomaId, f => random.Next(1,5));
        faker.RuleFor(x => x.NumeroPagina, f => random.Next(1,2000));
        faker.RuleFor(x => x.Volume, f => f.Address.Random.ToString().Limite(15));
        faker.RuleFor(x => x.TamanhoArquivo, f => f.Address.Random.ToString().Limite(15));
        faker.RuleFor(x => x.Acervo, f => Gerar(TipoAcervo.DocumentacaoHistorica).Generate());
        return faker;
    }
}