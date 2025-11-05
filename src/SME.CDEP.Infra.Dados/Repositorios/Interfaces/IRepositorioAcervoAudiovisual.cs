using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoAudiovisual : IRepositorioBase<AcervoAudiovisual>
    {
        new Task<AcervoAudiovisualCompleto> ObterPorId(long id);
        Task<AcervoAudiovisualDetalhe> ObterDetalhamentoPorCodigo(string filtroCodigo);
    }
}