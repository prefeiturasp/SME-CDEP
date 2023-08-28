using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoFotograficoDTO
    {
        Task<long> Inserir(AcervoFotograficoDTO acervoFotograficoDto);
        Task<IList<AcervoFotograficoDTO>> ObterTodos();
        Task<AcervoFotograficoDTO> Alterar(AcervoFotograficoDTO acervoFotograficoDto);
        Task<AcervoFotograficoDTO> ObterPorId(long id);
        Task<bool> Excluir(long id);
    }
}
