using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoSolicitacaoItem : IRepositorioBaseAuditavel<AcervoSolicitacaoItem>
    {
        Task<IEnumerable<AcervoSolicitacaoItem>> ObterMinhasSolicitacoes(long usuarioId);
        Task<IEnumerable<AcervoSolicitacaoItemDetalhe>> ObterSolicitacoesPorFiltro(long? acervoSolicitacaoId, TipoAcervo? tipoAcervo, 
            DateTime? dataSolicitacaoInicio, DateTime? dataSolicitacaoFim, string? responsavel, SituacaoSolicitacaoItem? situacaoItem, DateTime? dataVisitaInicio, 
            DateTime? dataVisitaFim);
    }
}