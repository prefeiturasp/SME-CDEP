using Bogus;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteIntegracao;

public class AcervoAudiovisualMock : AcervoMock
{
    private static AcervoAudiovisualMock _instance;
    public static AcervoAudiovisualMock Instance => _instance ??= new();
    
    public Faker<AcervoAudiovisual> Gerar()
    {
        var random = new Random();
        var faker = new Faker<AcervoAudiovisual>("pt_BR");
            
        faker.RuleFor(x => x.Localizacao, f => f.Lorem.Text().Limite(100));
        faker.RuleFor(x => x.Procedencia, f => f.Lorem.Text().Limite(200));
        faker.RuleFor(x => x.Copia, f => "Sim");
        faker.RuleFor(x => x.PermiteUsoImagem, f => true);
        faker.RuleFor(x => x.Duracao, f => "36 min");
        faker.RuleFor(x => x.TamanhoArquivo, f => "50MB");
        faker.RuleFor(x => x.ConservacaoId, f => random.Next(1,5));
        faker.RuleFor(x => x.CromiaId, f => random.Next(1,5));
        faker.RuleFor(x => x.Acessibilidade, f => "Sim");
        faker.RuleFor(x => x.Disponibilizacao, f => f.Lorem.Sentence().Limite(100));
        faker.RuleFor(x => x.SuporteId, f => random.Next(1,5));
        faker.RuleFor(x => x.Acervo, f => Gerar(TipoAcervo.Audiovisual).Generate());
        return faker;
    }
}