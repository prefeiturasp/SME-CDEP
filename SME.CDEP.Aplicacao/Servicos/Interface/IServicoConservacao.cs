using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoConservacao : IServicoAplicacao
    {
        Task<long> Inserir(ConservacaoDTO conservacaoDTO);
        Task<IList<ConservacaoDTO>> ObterTodos();
        Task<ConservacaoDTO> Alterar(ConservacaoDTO conservacaoDTO);
        Task<ConservacaoDTO> ObterPorId(long conservacaoId);
        Task<bool> Excluir(long conservacaoId);
    }
}
