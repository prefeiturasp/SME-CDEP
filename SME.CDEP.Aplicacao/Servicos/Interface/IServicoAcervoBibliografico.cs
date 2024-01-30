
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoBibliografico : IServicoAcervoAuditavel
    {
        Task<long> Inserir(AcervoBibliograficoCadastroDTO acervoBibliograficoCadastroDto);
        Task<IEnumerable<AcervoBibliograficoDTO>> ObterTodos();
        Task<AcervoBibliograficoDTO> Alterar(AcervoBibliograficoAlteracaoDTO acervoBibliograficoAlteracaoDto);
        Task<AcervoBibliograficoDTO> ObterPorId(long id);
        Task<bool> Excluir(long id);
    }
}
