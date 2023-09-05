using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoFotograficoArquivo : IRepositorioBaseSomenteId<AcervoFotograficoArquivo>
    {
        Task<IEnumerable<AcervoFotograficoArquivo>> ObterPorAcervoFotograficoId(long id);
        Task Excluir(long[] arquivosIdsExcluir, long acervoFotograficoId);
    }
}