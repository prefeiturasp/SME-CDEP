using Bogus;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteIntegracao;

public class AcervoEmprestimoMock : AuditoriaMock
{
    private static AcervoEmprestimoMock _instance;
    public static AcervoEmprestimoMock Instance => _instance ??= new();
    
    public Faker<AcervoEmprestimo> Gerar(long acervoSolicitacaoItemId, DateTime  dataEmprestimo)
    {
        var faker = new Faker<AcervoEmprestimo>("pt_BR");
            
        faker.RuleFor(x => x.AcervoSolicitacaoItemId, f => acervoSolicitacaoItemId);
        faker.RuleFor(x => x.DataEmprestimo, f => dataEmprestimo);
        faker.RuleFor(x => x.Situacao, f => SituacaoEmprestimo.EMPRESTADO);
        faker.RuleFor(x => x.DataDevolucao, f => dataEmprestimo.AddDays(7));
        AuditoriaFaker(faker);
        return faker;
    }
}