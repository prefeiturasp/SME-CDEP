using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcessoDocumento : IRepositorioBase<AcessoDocumento>
    {
        Task<IEnumerable<AcessoDocumento>> ObterPorIds(long[] ids);
    }
}