using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoAudiovisualManutencao
    {
        Task<long> Inserir(AcervoAudiovisualCadastroDTO acervoAudiovisualCadastroDto);
        Task<IEnumerable<AcervoAudiovisualDTO>> ObterTodos();
        Task<AcervoAudiovisualDTO> Alterar(AcervoAudiovisualAlteracaoDTO acervoAudiovisualAlteracaoDto);
        Task<AcervoAudiovisualDTO> ObterPorId(long id);
        Task<bool> Excluir(long id);
    }
}
