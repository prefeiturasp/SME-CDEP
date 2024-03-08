using Bogus;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteIntegracao;

public class AcervoSolicitacaoMock : AuditoriaMock
{
    private static AcervoSolicitacaoMock _instance;
    public static AcervoSolicitacaoMock Instance => _instance ??= new();
    
    public Faker<AcervoSolicitacao> Gerar()
    {
        var random = new Random();
        var faker = new Faker<AcervoSolicitacao>("pt_BR");
            
        faker.RuleFor(x => x.UsuarioId, f => random.Next(1,5));
        faker.RuleFor(x => x.Situacao, f => SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO);
        faker.RuleFor(x => x.Origem, f => Origem.Portal);
        faker.RuleFor(x => x.DataSolicitacao, f => DateTimeExtension.HorarioBrasilia());
        AuditoriaFaker(faker);
        return faker;
    }
}