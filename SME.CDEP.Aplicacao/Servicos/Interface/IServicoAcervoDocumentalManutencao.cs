using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoDocumentalManutencao
    {
        Task<long> Inserir(AcervoDocumentalCadastroDTO acervoDocumentalCadastroDto);
        Task<IEnumerable<AcervoDocumentalDTO>> ObterTodos();
        Task<AcervoDocumentalDTO> Alterar(AcervoDocumentalAlteracaoDTO acervoDocumentalAlteracaoDto);
        Task<AcervoDocumentalDTO> ObterPorId(long id);
        Task<bool> Excluir(long id);
    }
}
