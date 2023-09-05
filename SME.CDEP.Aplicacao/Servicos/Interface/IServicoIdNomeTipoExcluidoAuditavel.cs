using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoIdNomeTipoExcluidoAuditavel
    {
        Task<long> Inserir(IdNomeTipoExcluidoAuditavelDTO idNomeExcluidoAuditavelDTO);
        Task<IEnumerable<IdNomeTipoExcluidoAuditavelDTO>> ObterTodos();
        Task<IdNomeTipoExcluidoAuditavelDTO> Alterar(IdNomeTipoExcluidoAuditavelDTO idNomeExcluidoAuditavelDTO);
        Task<IdNomeTipoExcluidoAuditavelDTO> ObterPorId(long Id);
        Task<bool> Excluir(long Id);
        Task<PaginacaoResultadoDTO<IdNomeTipoExcluidoAuditavelDTO>> ObterPaginado(string? nome = null);
    }
}
