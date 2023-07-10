using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoCromia : IServicoAplicacao
    {
        Task<long> Inserir(CromiaDTO cromiaDTO);
        Task<IList<CromiaDTO>> ObterTodos();
        Task<CromiaDTO> Alterar(CromiaDTO cromiaDTO);
        Task<CromiaDTO> ObterPorId(long cromiaId);
        Task<bool> Excluir(long cromiaId);
    }
}
