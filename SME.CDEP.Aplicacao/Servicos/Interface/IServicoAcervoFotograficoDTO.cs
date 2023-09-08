using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoFotograficoDTO
    {
        Task<long> Inserir(AcervoFotograficoCadastroDTO acervoFotograficoCadastroDto);
        Task<IEnumerable<AcervoFotograficoDTO>> ObterTodos();
        Task<AcervoFotograficoDTO> Alterar(AcervoFotograficoAlteracaoDTO acervoFotograficoAlteracaoDto);
        Task<AcervoFotograficoDTO> ObterPorId(long id);
        Task<bool> Excluir(long id);
    }
}
