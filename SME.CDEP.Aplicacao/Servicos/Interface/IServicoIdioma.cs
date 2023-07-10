using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoIdioma : IServicoAplicacao
    {
        Task<long> Inserir(IdiomaDTO idiomaDTO);
        Task<IList<IdiomaDTO>> ObterTodos();
        Task<IdiomaDTO> Alterar(IdiomaDTO idiomaDTO);
        Task<IdiomaDTO> ObterPorId(long idiomaId);
        Task<bool> Excluir(long idiomaId);
    }
}
