using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoSolicitacao : IServicoAplicacao
    {
        Task<IEnumerable<AcervoSolicitacaoItemRetornoCadastroDTO>> Inserir(AcervoSolicitacaoCadastroDTO acervoSolicitacaoCadastroDTO);
        Task<IEnumerable<AcervoSolicitacaoItemRetornoCadastroDTO>> ObterPorId(long acervoSolicitacaoId);
        Task<bool> Remover(long acervoSolicitacaoId);
        Task<IEnumerable<AcervoTipoTituloAcervoIdCreditosAutoresDTO>> ObterItensDoAcervoPorFiltros(long[] acervosIds);
        Task<bool> Excluir(long acervoSolicitacaoId);
        Task<IEnumerable<MinhaSolicitacaoDTO>> ObterMinhasSolicitacoes();
    }
}
