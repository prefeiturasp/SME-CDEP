using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoArteGraficaArquivo : IRepositorioBaseSomenteId<AcervoArteGraficaArquivo>
    {
        Task<IEnumerable<AcervoArteGraficaArquivo>> ObterPorAcervoArteGraficaId(long id);
        Task Excluir(long[] arquivosIdsExcluir, long acervoArteGraficaId);
    }
}