using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervo : IRepositorioBase<Acervo>
    {
        Task<IList<Acervo>> PesquisarPorFiltro(int? tipoAcervo, string titulo, long? creditoAutorId, string codigo);
    }
}