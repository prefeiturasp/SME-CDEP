using System.ComponentModel.DataAnnotations;
using SME.CDEP.Dominio.Extensions;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum SituacaoSolicitacao
{
    [Display(Description = "Aguardando atendimento")]
    AGUARDANDO_ATENDIMENTO = 1,
    
    [Display(Description = "Finalizado atendimento")]
    FINALIZADO_ATENDIMENTO = 2,
    
    [Display(Description = "Aguardando visita")]
    AGUARDANDO_VISITA = 3,
    
    [Display(Description = "Cancelado")]
    CANCELADO = 4,
        
    [Display(Description = "Atendido parcialmente")]
    ATENDIDO_PARCIALMENTE = 5
}
public static class SituacaoSolicitacaoExtension
{
    public static bool EstaAguardandoAtendimento(this SituacaoSolicitacao situacaoSolicitacao)
    {
       return situacaoSolicitacao == SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO;
    }
    
    public static bool EstaFinalizadoAtendimento(this SituacaoSolicitacao situacaoSolicitacao)
    {
        return situacaoSolicitacao == SituacaoSolicitacao.FINALIZADO_ATENDIMENTO;
    }
    
    public static bool EstaAguardandoVisita(this SituacaoSolicitacao situacaoSolicitacao)
    {
        return situacaoSolicitacao == SituacaoSolicitacao.AGUARDANDO_VISITA;
    }
    
    public static bool FoiAtendidaParcialmente(this SituacaoSolicitacao situacaoSolicitacao)
    {
        return situacaoSolicitacao == SituacaoSolicitacao.ATENDIDO_PARCIALMENTE;
    }
    
    public static bool EstaFinalizadoAtendimentoOuCancelado(this SituacaoSolicitacao situacaoSolicitacao)
    {
        return situacaoSolicitacao == SituacaoSolicitacao.FINALIZADO_ATENDIMENTO || situacaoSolicitacao == SituacaoSolicitacao.CANCELADO;
    }
    
    public static bool NaoEstaFinalizadoAtendimentoOuCancelado(this SituacaoSolicitacao situacaoSolicitacao)
    {
        return !situacaoSolicitacao.EstaFinalizadoAtendimentoOuCancelado();
    }
}