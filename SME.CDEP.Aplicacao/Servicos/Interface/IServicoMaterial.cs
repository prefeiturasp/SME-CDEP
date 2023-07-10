using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoMaterial : IServicoAplicacao
    {
        Task<long> Inserir(MaterialDTO materialDTO);
        Task<IList<MaterialDTO>> ObterTodos();
        Task<MaterialDTO> Alterar(MaterialDTO materialDTO);
        Task<MaterialDTO> ObterPorId(long materialId);
        Task<bool> Excluir(long materialId);
    }
}
