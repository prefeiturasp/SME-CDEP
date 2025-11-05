using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoFotografico : IRepositorioBase<AcervoFotografico>
    {
        new Task<AcervoFotograficoCompleto> ObterPorId(long id);
        Task<AcervoFotograficoDetalhe> ObterDetalhamentoPorCodigo(string filtroCodigo);
    }
}