using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoIdNomeTipoExcluido
    {
        Task<long> Inserir(IdNomeTipoExcluidoDTO idNomeTipoExcluidoDto);
        Task<IList<IdNomeTipoExcluidoDTO>> ObterTodos();
        Task<IdNomeTipoExcluidoDTO> Alterar(IdNomeTipoExcluidoDTO idNomeTipoExcluidoDto);
        Task<IdNomeTipoExcluidoDTO> ObterPorId(long Id);
        Task<bool> Excluir(long Id);
    }
}
