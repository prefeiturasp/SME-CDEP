using Bogus;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteIntegracao;

public class ArquivoMock : AuditoriaMock
{
    private static ArquivoMock _instance;
    public static ArquivoMock Instance => _instance ??= new();
    
    public Faker<Arquivo> GerarArquivo(TipoArquivo tipoArquivo)
    {
        var faker = new Faker<Arquivo>("pt_BR");
            
        faker.RuleFor(x => x.Codigo, f => Guid.NewGuid());
        faker.RuleFor(x => x.Nome, f => f.Lorem.Text().Limite(20));
        faker.RuleFor(x => x.Tipo, f => tipoArquivo);
        faker.RuleFor(x => x.TipoConteudo, f => "image/jpeg");
        AuditoriaFaker(faker);
        return faker;
    }
}