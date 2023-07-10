using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoFormato : IServicoAplicacao
    {
        Task<long> Inserir(FormatoDTO formatoDto);
        Task<IList<FormatoDTO>> ObterTodos();
        Task<FormatoDTO> Alterar(FormatoDTO formatoDTO);
        Task<FormatoDTO> ObterPorId(long formatoId);
        Task<bool> Excluir(long formatoId);
    }
}
