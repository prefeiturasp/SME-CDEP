using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoPesquisaAcervo
    {
        Task<PaginacaoResultadoDTO<PesquisaAcervoDTO>> ObterPorTextoLivreETipoAcervo(FiltroTextoLivreTipoAcervoDTO filtroTextoLivreTipoAcervo);
        Task<IEnumerable<string>> ObterAutocompletacaoTituloAcervosBaixadosAsync(string termoPesquisado);
    }
}
