using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoSolicitacao : IServicoAplicacao
    {
        Task<long> Inserir(AcervoSolicitacaoItemCadastroDTO[] acervosSolicitacaoItensCadastroDTO);
        Task<IEnumerable<AcervoSolicitacaoItemRetornoCadastroDTO>> ObterPorId(long acervoSolicitacaoId);
        Task<bool> Remover(long acervoSolicitacaoId);
        Task<IEnumerable<AcervoTipoTituloAcervoIdCreditosAutoresDTO>> ObterItensDoAcervoPorFiltros(long[] acervosIds);
        Task<bool> Excluir(long acervoSolicitacaoId);
        Task<PaginacaoResultadoDTO<MinhaSolicitacaoDTO>> ObterMinhasSolicitacoes();
        Task<IEnumerable<SituacaoItemDTO>> ObterSituacoesAtendimentosItem();
        Task<PaginacaoResultadoDTO<SolicitacaoDTO>> ObterSolicitacoesPorFiltro(FiltroSolicitacaoDTO filtroSolicitacaoDto);
        Task<AcervoSolicitacaoDetalheDTO> ObterDetalhesPorId(long acervoSolicitacaoId);
        IEnumerable<IdNomeDTO> ObterTiposDeAtendimentos();
        Task<bool> ConfirmarAtendimento(AcervoSolicitacaoConfirmarDTO acervoSolicitacaoConfirmar);
        Task<bool> FinalizarAtendimento(long acervoSolicitacaoId);
        Task<bool> CancelarAtendimento(long acervoSolicitacaoId);
        Task<bool> CancelarItemAtendimento(long acervoSolicitacaoItemId);
    }
}
