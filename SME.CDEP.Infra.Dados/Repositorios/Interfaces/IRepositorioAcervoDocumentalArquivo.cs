using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoDocumentalArquivo : IRepositorioBaseSomenteId<AcervoDocumentalArquivo>
    {
        Task<IEnumerable<AcervoDocumentalArquivo>> ObterPorAcervoDocumentalId(long id);
        Task Excluir(long[] arquivosIdsExcluir, long acervoDocumentalId);
    }
}