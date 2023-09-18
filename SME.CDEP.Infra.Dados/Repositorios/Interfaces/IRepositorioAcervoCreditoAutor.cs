using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoCreditoAutor : IRepositorioBaseSomenteId<AcervoCreditoAutor>
    {
        Task<IEnumerable<AcervoCreditoAutor>> ObterPorAcervoId(long id);
        Task Excluir(long[] creditosAutoresIdsExcluir, long acervoId);
    }
}