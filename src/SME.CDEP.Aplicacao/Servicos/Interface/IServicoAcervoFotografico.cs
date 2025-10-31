using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoFotografico : IServicoAcervoAuditavel
    {
        Task<long> Inserir(AcervoFotograficoCadastroDTO acervoFotograficoCadastroDto);
        Task<IEnumerable<AcervoFotograficoDTO>> ObterTodos();
        Task<AcervoFotograficoDTO> Alterar(AcervoFotograficoAlteracaoDTO acervoFotograficoAlteracaoDto);
        Task<AcervoFotograficoDTO> ObterPorId(long id);
        Task<bool> Excluir(long id);
    }
}
