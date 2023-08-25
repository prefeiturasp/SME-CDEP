using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoPesquisa : IServicoAplicacao, IServicoAcervoDTO
    {
        Task<PaginacaoResultadoDTO<IdTipoTituloCreditoAutoriaCodigoAcervoDTO>> ObterPorFiltro(int? tipoAcervo, string titulo, long? creditoAutorId, string codigo);
    }
}
