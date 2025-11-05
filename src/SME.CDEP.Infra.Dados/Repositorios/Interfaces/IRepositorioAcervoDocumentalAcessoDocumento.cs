using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoDocumentalAcessoDocumento : IRepositorioBaseSomenteId<AcervoDocumentalAcessoDocumento>
    {
        Task<IEnumerable<AcervoDocumentalAcessoDocumento>> ObterPorAcervoDocumentalId(long id);
        Task Excluir(long[] acessoDocumentosIdsExcluir, long acervoDocumentalId);
    }
}