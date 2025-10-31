using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoDocumental : IRepositorioBase<AcervoDocumental>
    {
        Task<AcervoDocumentalCompleto> ObterComDetalhesPorId(long id);
        Task<AcervoDocumentalDetalhe> ObterDetalhamentoPorCodigo(string filtroCodigo);
    }
}