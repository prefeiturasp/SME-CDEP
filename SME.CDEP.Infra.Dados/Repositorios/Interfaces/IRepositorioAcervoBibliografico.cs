using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoBibliografico : IRepositorioBase<AcervoBibliografico>
    {
        Task<AcervoBibliograficoCompleto> ObterAcervoBibliograficoCompletoPorId(long id);
        Task<AcervoBibliograficoDetalhe> ObterDetalhamentoPorCodigo(string filtroCodigo);
        Task<AcervoBibliografico> ObterPorAcervoId(long acervoId);
    }
}