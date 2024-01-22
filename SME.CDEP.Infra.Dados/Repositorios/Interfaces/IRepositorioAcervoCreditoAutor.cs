using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoCreditoAutor : IRepositorioBaseSomenteId<AcervoCreditoAutor>
    {
        Task<IEnumerable<AcervoCreditoAutor>> ObterPorAcervoId(long id, bool incluirTipoAutoria = false);
        Task Excluir(long creditoAutorId, string tipoAutoria, long acervoId);
        Task Excluir(long[] creditosAutoresIdsExcluir, long acervoId);
        Task<IEnumerable<string>> ObterNomesPorAcervoId(long id);
    }
}