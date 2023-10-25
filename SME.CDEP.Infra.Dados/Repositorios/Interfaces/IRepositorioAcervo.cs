using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervo : IRepositorioBaseAuditavel<Acervo>
    {
        Task<IEnumerable<Acervo>> PesquisarPorFiltro(int? tipoAcervo, string titulo, long? creditoAutorId, string codigo);
        Task<bool> ExisteCodigo(string codigo, long id);
        Task<bool> ExisteTitulo(string titulo, long id, string codigo, string codigoNovo);
        Task<IEnumerable<Acervo>> ObterPorTextoLivreETipoAcervo(string? textoLivre, TipoAcervo? tipoAcervo);
    }
}