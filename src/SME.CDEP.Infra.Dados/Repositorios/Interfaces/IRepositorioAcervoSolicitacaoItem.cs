using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoSolicitacaoItem : IRepositorioBaseAuditavel<AcervoSolicitacaoItem>
    {
        Task<IEnumerable<AcervoSolicitacaoItemResumido>> ObterMinhasSolicitacoes(long usuarioId);
        Task<IEnumerable<AcervoSolicitacaoItemDetalhe>> ObterSolicitacoesPorFiltro(long? acervoSolicitacaoId, TipoAcervo? tipoAcervo,
            DateTime? dataSolicitacaoInicio, DateTime? dataSolicitacaoFim, string? responsavel, SituacaoSolicitacaoItem? situacaoItem, DateTime? dataVisitaInicio,
            DateTime? dataVisitaFim, string? filtroSolicitacaoDto, SituacaoEmprestimo? situacaoEmprestimo, long[] tiposAcervosPermitidos);
        Task<IEnumerable<AcervoSolicitacaoItem>> ObterItensEmSituacaoAguardandoAtendimentoOuVisitaOuFinalizadoManualmentePorSolicitacaoId(long acervoSolicitacaoId);
        Task<bool> PossuiItensEmSituacaoAguardandoAtendimentoOuAguardandoVisitaComDataFutura(long acervoSolicitacaoId);
        Task<bool> PossuiItensEmSituacaoFinalizadoAutomaticamenteOuCancelado(long acervoSolicitacaoItemId);
        Task<bool> AtendimentoPossuiItemSituacaoFinalizadoAutomaticamenteOuCancelado(long acervoSolicitacaoItemId);
        Task<bool> PossuiItensFinalizadosAutomaticamente(long acervoSolicitacaoId);
        Task<IEnumerable<AcervoSolicitacaoItem>> ObterItensEmSituacaoAguardandoVisitaPorSolicitacaoId(long acervoSolicitacaoId);
        Task<IEnumerable<AcervoSolicitacaoItem>> ObterItensPorSolicitacaoId(long acervoSolicitacaoId);
        Task<Acervo?> ObterAcervoPorAcervoSolicitacaoItemId(long acervoSolicitacaoItemId);
        Task<IEnumerable<AcervoSolicitacaoItemDetalhe>> ObterDetalhamentoDosItensPorSolicitacaoOuItem(long? acervoSolicitacaoId, long? acervoSolicitacaoItemId);
    }
}