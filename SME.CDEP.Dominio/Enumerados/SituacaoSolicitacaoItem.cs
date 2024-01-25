using System.ComponentModel.DataAnnotations;
using SME.CDEP.Dominio.Extensions;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum SituacaoSolicitacaoItem
{
    [Display(Description = "Aguardando atendimento")]
    AGUARDANDO_ATENDIMENTO = 1,
    
    [Display(Description = "Finalizado automaticamente")]
    FINALIZADO_AUTOMATICAMENTE = 2
}
public static class SituacaoSolicitacaoItemExtension
{
    public static bool EstaAguardandoAtendimento(this SituacaoSolicitacaoItem situacaoSolicitacaoItem)
    {
       return situacaoSolicitacaoItem == SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO;
    }
    
    public static bool EstaFinalizadoAtendimento(this SituacaoSolicitacaoItem situacaoSolicitacaoItem)
    {
        return situacaoSolicitacaoItem == SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE;
    }
}