using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoDocumental : IServicoAcervoAuditavel
    {
        Task<long> Inserir(AcervoDocumentalCadastroDTO acervoDocumentalCadastroDto);
        Task<IEnumerable<AcervoDocumentalDTO>> ObterTodos();
        Task<AcervoDocumentalDTO> Alterar(AcervoDocumentalAlteracaoDTO acervoDocumentalAlteracaoDto);
        Task<AcervoDocumentalDTO> ObterPorId(long id);
        Task<bool> Excluir(long id);
    }
}
