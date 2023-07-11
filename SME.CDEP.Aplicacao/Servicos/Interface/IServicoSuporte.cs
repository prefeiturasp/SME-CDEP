using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoSuporte : IServicoAplicacao
    {
        Task<long> Inserir(SuporteDTO suporteDTO);
        Task<IList<SuporteDTO>> ObterTodos();
        Task<SuporteDTO> Alterar(SuporteDTO suporteDTO);
        Task<SuporteDTO> ObterPorId(long suporteId);
        Task<bool> Excluir(long suporteId);
    }
}
