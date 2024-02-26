using System.ComponentModel.DataAnnotations;
using SME.CDEP.Dominio.Extensions;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum TipoEvento
{
    [Display(Description = "Visita")]
    VISITA = 1,
    
    [Display(Description = "Feriado")]
    FERIADO = 2,
    
    [Display(Description = "Suspensão")]
    SUSPENSAO = 3
}

public static class TipoEventoExtension
{
    public static bool EhFeriado(this TipoEvento tipoFeriado)
    {
       return tipoFeriado == TipoEvento.FERIADO;
    }
    
    public static bool EhVisita(this TipoEvento tipoFeriado)
    {
        return tipoFeriado == TipoEvento.VISITA;
    }
    
    public static bool EhSuspensao(this TipoEvento tipoFeriado)
    {
        return tipoFeriado == TipoEvento.SUSPENSAO;
    }
}