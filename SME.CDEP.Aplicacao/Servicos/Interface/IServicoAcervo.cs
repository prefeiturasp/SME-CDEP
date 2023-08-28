using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervo : IServicoAcervoTipos, IServicoAcervoPesquisa
    {
        Task<long> Inserir(AcervoDTO acervoDto);
        Task<IList<AcervoDTO>> ObterTodos();
        Task<AcervoDTO> Alterar(AcervoDTO acervoDto);
        Task<AcervoDTO> ObterPorId(long acervoId);
        Task<PaginacaoResultadoDTO<IdTipoTituloCreditoAutoriaCodigoAcervoDTO>> ObterPorFiltro(int? tipoAcervo, string titulo, long? creditoAutorId, string codigo);
        Task<bool> Excluir(long entidaId);
    }
}
