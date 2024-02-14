using System.Collections;
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
            DateTime? dataVisitaFim);
        Task<IEnumerable<AcervoSolicitacaoItem>> ObterItensEmSituacaoAguardandoAtendimentoOuVisitaOuFinalizadoManualmentePorSolicitacaoId(long acervoSolicitacaoId);
        Task<bool> PossuiItensEmSituacaoAguardandoAtendimentoOuAguardandoVisitaComDataFutura(long acervoSolicitacaoId);
        Task<bool> PossuiItensEmSituacaoFinalizadoAutomaticamenteOuCancelado(long acervoSolicitacaoItemId);
        Task<bool> AtendimentoPossuiSituacaoAguardandoVisitaEItemSituacaoFinalizadoAutomaticamenteOuCancelado(long acervoSolicitacaoItemId);
        Task<bool> PossuiItensQueForamAtendidosParcialmente(long acervoSolicitacaoId);
        Task<IEnumerable<AcervoSolicitacaoItem>> ObterItensEmSituacaoAguardandoVisitaPorSolicitacaoId(long acervoSolicitacaoId);
    }
}