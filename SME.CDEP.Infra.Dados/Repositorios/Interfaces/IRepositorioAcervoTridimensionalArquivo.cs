using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoTridimensionalArquivo : IRepositorioBaseSomenteId<AcervoTridimensionalArquivo>
    {
        Task<IEnumerable<AcervoTridimensionalArquivo>> ObterPorAcervoTridimensionalId(long id);
        Task Excluir(long[] arquivosIdsExcluir, long acervoTridimensionalId);
    }
}