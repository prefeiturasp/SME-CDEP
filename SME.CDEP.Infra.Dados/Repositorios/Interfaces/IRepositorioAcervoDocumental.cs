using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoDocumental : IRepositorioBase<AcervoDocumental>
    {
        Task<AcervoDocumentalCompleto> ObterPorId(long id);
        Task<AcervoDocumentalCompleto> ObterDetalhamentoPorCodigo(string filtroCodigo);
    }
}