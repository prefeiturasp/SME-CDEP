using System.ComponentModel.DataAnnotations;
using SME.CDEP.Dominio.Extensions;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum SituacaoSolicitacaoItem
{
    [Display(Description = "Em análise")]
    EM_ANALISE = 1,
    
    [Display(Description = "Finalizado")]
    FINALIZADO = 2
}
public static class SituacaoSolicitacaoItemExtension
{
    public static bool EstaAguardandoAtendimento(this SituacaoSolicitacaoItem situacaoSolicitacaoItem)
    {
       return situacaoSolicitacaoItem == SituacaoSolicitacaoItem.EM_ANALISE;
    }
    
    public static bool EstaFinalizadoAtendimento(this SituacaoSolicitacaoItem situacaoSolicitacaoItem)
    {
        return situacaoSolicitacaoItem == SituacaoSolicitacaoItem.FINALIZADO;
    }
}