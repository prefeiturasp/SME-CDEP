﻿using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum SituacaoSolicitacaoItem
{
    [Display(Description = "Aguardando atendimento")]
    AGUARDANDO_ATENDIMENTO = 1,
    
    [Display(Description = "Aguardando visita")]
    AGUARDANDO_VISITA = 2,
        
    [Display(Description = "Finalizado automaticamente")]
    FINALIZADO_AUTOMATICAMENTE = 3,
    
    [Display(Description = "Cancelado")]
    CANCELADO = 4,
    
    [Display(Description = "Finalizado manualmente")]
    FINALIZADO_MANUALMENTE = 5
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
    
    public static bool EstaCancelado(this SituacaoSolicitacaoItem situacaoSolicitacaoItem)
    {
        return situacaoSolicitacaoItem == SituacaoSolicitacaoItem.CANCELADO;
    }
    
    public static bool NaoEstaCancelado(this SituacaoSolicitacaoItem situacaoSolicitacaoItem)
    {
        return situacaoSolicitacaoItem != SituacaoSolicitacaoItem.CANCELADO;
    }
    
    public static bool NaoPodeFinalizarAtendimentoItem(this SituacaoSolicitacaoItem situacaoSolicitacaoItem)
    {
        return situacaoSolicitacaoItem == SituacaoSolicitacaoItem.AGUARDANDO_VISITA 
               || situacaoSolicitacaoItem == SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE
               || situacaoSolicitacaoItem == SituacaoSolicitacaoItem.CANCELADO;
    }
    
    public static bool PodeCancelarAtendimento(this SituacaoSolicitacaoItem situacaoSolicitacaoItem)
    {
        return !NaoPodeCancelarAtendimento(situacaoSolicitacaoItem);
    }
    
    public static bool NaoPodeCancelarAtendimento(this SituacaoSolicitacaoItem situacaoSolicitacaoItem)
    {
        return situacaoSolicitacaoItem == SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE 
               || situacaoSolicitacaoItem == SituacaoSolicitacaoItem.CANCELADO
               || situacaoSolicitacaoItem == SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE;
    }
    
    public static bool EstaAguardandoVisita(this SituacaoSolicitacaoItem situacaoSolicitacaoItem)
    {
        return situacaoSolicitacaoItem == SituacaoSolicitacaoItem.AGUARDANDO_VISITA;
    }
    
    public static bool EstaEmSituacaoFinalizadoAutomaticamenteOuCancelado(this SituacaoSolicitacaoItem situacaoSolicitacaoItem)
    {
        return situacaoSolicitacaoItem == SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE 
               || situacaoSolicitacaoItem == SituacaoSolicitacaoItem.CANCELADO;
    } 
    
    public static bool EstaFinalizadoManualmente(this SituacaoSolicitacaoItem situacaoSolicitacaoItem)
    {
        return situacaoSolicitacaoItem == SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE;
    }
    
    public static bool EstaEmSituacaoAguardandoVisitaEAguardandoAtendimento(this SituacaoSolicitacaoItem situacaoSolicitacaoItem)
    {
        return situacaoSolicitacaoItem == SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO 
               || situacaoSolicitacaoItem == SituacaoSolicitacaoItem.AGUARDANDO_VISITA;
    }
    
    public static bool NaoEstaAguardandoAtendimento(this SituacaoSolicitacaoItem situacaoSolicitacaoItem)
    {
        return !situacaoSolicitacaoItem.EstaAguardandoAtendimento();
    } 
}