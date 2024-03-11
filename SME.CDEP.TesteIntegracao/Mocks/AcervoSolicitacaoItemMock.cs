using Bogus;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteIntegracao;

public class AcervoSolicitacaoItemMock : AuditoriaMock
{
    private static AcervoSolicitacaoItemMock _instance;
    public static AcervoSolicitacaoItemMock Instance => _instance ??= new();
    
    public Faker<AcervoSolicitacaoItem> Gerar(long acervoSolicitacaoId, TipoAtendimento tipoAtendimento,DateTime dataVisita, SituacaoSolicitacaoItem situacaoSolicitacaoItem)
    {
        var random = new Random();
        var faker = new Faker<AcervoSolicitacaoItem>("pt_BR");
            
        faker.RuleFor(x => x.AcervoSolicitacaoId, f => acervoSolicitacaoId);
        faker.RuleFor(x => x.AcervoId, f => 1);
        faker.RuleFor(x => x.Situacao, f => situacaoSolicitacaoItem);
        faker.RuleFor(x => x.TipoAtendimento, f => tipoAtendimento);
        faker.RuleFor(x => x.DataVisita, f => dataVisita);
        AuditoriaFaker(faker);
        return faker;
    }
}