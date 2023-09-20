using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoArteGraficaDTO
    {
        Task<long> Inserir(AcervoArteGraficaCadastroDTO acervoArteGraficaCadastroDto);
        Task<IEnumerable<AcervoArteGraficaDTO>> ObterTodos();
        Task<AcervoArteGraficaDTO> Alterar(AcervoArteGraficaAlteracaoDTO acervoArteGraficaAlteracaoDto);
        Task<AcervoArteGraficaDTO> ObterPorId(long id);
        Task<bool> Excluir(long id);
    }
}
