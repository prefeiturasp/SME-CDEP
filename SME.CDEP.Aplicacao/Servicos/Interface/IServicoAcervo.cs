using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervo : IServicoAcervoTipos, IServicoAcervoPesquisa
    {
        Task<long> Inserir(Acervo acervo);
        Task<IEnumerable<AcervoDTO>> ObterTodos();
        Task<AcervoDTO> Alterar(Acervo acervo);
        Task<AcervoDTO> Alterar(long id,string titulo, string codigo, long[] creditosAutoresIds = null);
        Task<AcervoDTO> ObterPorId(long acervoId);
        Task<PaginacaoResultadoDTO<IdTipoTituloCreditoAutoriaCodigoAcervoDTO>> ObterPorFiltro(int? tipoAcervo, string titulo, long? creditoAutorId, string codigo);
        Task<bool> Excluir(long entidaId);
    }
}
