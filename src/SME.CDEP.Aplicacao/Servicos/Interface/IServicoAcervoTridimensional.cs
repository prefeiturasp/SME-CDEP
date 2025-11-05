using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoTridimensional : IServicoAcervoAuditavel
    {
        Task<long> Inserir(AcervoTridimensionalCadastroDTO acervoTridimensionalCadastroDto);
        Task<IEnumerable<AcervoTridimensionalDTO>> ObterTodos();
        Task<AcervoTridimensionalDTO> Alterar(AcervoTridimensionalAlteracaoDTO acervoTridimensionalAlteracaoDto);
        Task<AcervoTridimensionalDTO> ObterPorId(long id);
        Task<bool> Excluir(long id);
    }
}
