using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoArquivoAuditavel
    {
        Task<long> Inserir(ArquivoDTO arquivoDTO);
        Task<IEnumerable<ArquivoDTO>> ObterTodos();
        Task<ArquivoDTO> Alterar(ArquivoDTO arquivoDTO);
        Task<ArquivoDTO> ObterPorId(long Id);
        Task<bool> Excluir(long Id);
    }
}
