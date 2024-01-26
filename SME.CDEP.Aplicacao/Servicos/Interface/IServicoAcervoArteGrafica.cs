using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoArteGrafica : IServicoAcervoAuditavel
    {
        Task<long> Inserir(AcervoArteGraficaCadastroDTO acervoArteGraficaCadastroDto);
        Task<IEnumerable<AcervoArteGraficaDTO>> ObterTodos();
        Task<AcervoArteGraficaDTO> Alterar(AcervoArteGraficaAlteracaoDTO acervoArteGraficaAlteracaoDto);
        Task<AcervoArteGraficaDTO> ObterPorId(long id);
        Task<bool> Excluir(long id);
    }
}
