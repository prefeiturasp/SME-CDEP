using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoPesquisaAervo
    {
        Task<PaginacaoResultadoDTO<PesquisaAcervoDTO>> ObterPorTextoLivreETipoAcervo(FiltroTextoLivreTipoAcervoDTO filtroTextoLivreTipoAcervo);
    }
}
