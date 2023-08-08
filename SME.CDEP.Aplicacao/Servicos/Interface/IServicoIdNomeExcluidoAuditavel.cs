using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoIdNomeExcluidoAuditavel
    {
        Task<long> Inserir(IdNomeExcluidoAuditavelDTO idNomeExcluidoAuditavelDTO);
        Task<IList<IdNomeExcluidoAuditavelDTO>> ObterTodos();
        Task<IdNomeExcluidoAuditavelDTO> Alterar(IdNomeExcluidoAuditavelDTO idNomeExcluidoAuditavelDTO);
        Task<IdNomeExcluidoAuditavelDTO> ObterPorId(long Id);
        Task<bool> Excluir(long Id);
        Task<IList<IdNomeExcluidoAuditavelDTO>> PesquisarPorNome(string nome);
        Task<PaginacaoResultadoDTO<IdNomeExcluidoAuditavelDTO>> ObterPaginado();
    }
}
