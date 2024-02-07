using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum SituacaoSolicitacaoItem
{
    [Display(Description = "Aguardando atendimento")]
    AGUARDANDO_ATENDIMENTO = 1,
    
    [Display(Description = "Aguardando visita")]
    AGUARDANDO_VISITA = 2,
        
    [Display(Description = "Finalizado automaticamente")]
    FINALIZADO_AUTOMATICAMENTE = 3
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