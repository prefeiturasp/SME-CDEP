using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum SituacaoAtendimento
{
    [Display(Description = "Atendimento agendado")]
    AtendimentoAgendado = 1,
    
    [Display(Description = "Atendimento finalizado")]
    AtendimentoFinalizado = 2
}

public static class SituacaoAtendimentoExtensions 
{
   
    public static bool EhAtendimentoAgendado(this SituacaoAtendimento situacaoAtendimento)
    {
        return situacaoAtendimento == SituacaoAtendimento.AtendimentoAgendado;
    }
            
    public static bool EhAtendimentoFinalizado(this SituacaoAtendimento situacaoAtendimento)
    {
        return situacaoAtendimento == SituacaoAtendimento.AtendimentoFinalizado;
    }
}