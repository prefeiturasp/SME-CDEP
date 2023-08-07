using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoIdNomeExcluido
    {
        Task<long> Inserir(BaseComNomeDTO baseComNomeDto);
        Task<IList<BaseComNomeDTO>> ObterTodos();
        Task<BaseComNomeDTO> Alterar(BaseComNomeDTO baseComNomeDto);
        Task<BaseComNomeDTO> ObterPorId(long Id);
        Task<bool> Excluir(long Id);
    }
}
