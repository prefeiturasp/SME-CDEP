using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoBibliograficoAssunto : IRepositorioBaseSomenteId<AcervoBibliograficoAssunto>
    {
        Task<IEnumerable<AcervoBibliograficoAssunto>> ObterPorAcervoBibliograficoId(long id);
        Task Excluir(long[] acessoBibliograficoAssuntoIdsExcluir, long acervobibliograficoId);
    }
}