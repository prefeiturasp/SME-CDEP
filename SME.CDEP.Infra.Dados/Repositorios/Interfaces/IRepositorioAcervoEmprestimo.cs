using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoEmprestimo : IRepositorioBaseAuditavel<AcervoEmprestimo>
    {
        Task<IEnumerable<AcervoEmprestimo>> ObterUltimoEmprestimoPorAcervoSolicitacaoItemIds(long[] acervoSolicitacaoItemIds);
        Task<AcervoEmprestimo> ObterUltimoEmprestimoPorAcervoSolicitacaoItemId(long acervoSolicitacaoItemId);
    }
}