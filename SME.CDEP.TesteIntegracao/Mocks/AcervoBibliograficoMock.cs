using Bogus;
using DocumentFormat.OpenXml.Math;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteIntegracao;

public class AcervoBibliograficoMock : AcervoMock
{
    private static AcervoBibliograficoMock _instance;
    public static AcervoBibliograficoMock Instance => _instance ??= new();
    
    public Faker<AcervoBibliografico> GerarAcervoBibliografico()
    {
        var random = new Random();
        var faker = new Faker<AcervoBibliografico>("pt_BR");
            
        faker.RuleFor(x => x.MaterialId, f => random.Next(1,5));
        faker.RuleFor(x => x.EditoraId, f => random.Next(1,5));
        faker.RuleFor(x => x.Edicao, f => f.Lorem.Sentence().Limite(15));
        faker.RuleFor(x => x.NumeroPagina, f => random.Next(15,55));
        faker.RuleFor(x => x.Largura, f => random.Next(15,55));
        faker.RuleFor(x => x.Altura, f => random.Next(15,55));
        faker.RuleFor(x => x.Volume, f => f.Lorem.Sentence().Limite(15));
        faker.RuleFor(x => x.IdiomaId, f => random.Next(1,5));
        faker.RuleFor(x => x.LocalizacaoCDD, f => f.Lorem.Sentence().Limite(50));
        faker.RuleFor(x => x.LocalizacaoPHA, f => f.Lorem.Sentence().Limite(50));
        faker.RuleFor(x => x.NotasGerais, f => f.Lorem.Text().Limite(500));
        faker.RuleFor(x => x.Isbn, f => f.Lorem.Sentence().Limite(50));
        faker.RuleFor(x => x.Acervo, f => GerarAcervo(TipoAcervo.Bibliografico).Generate());
        return faker;
    }
}