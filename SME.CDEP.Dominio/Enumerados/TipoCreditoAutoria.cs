using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum TipoCreditoAutoria
{
    [Display(Description = "Crédito")]
    Credito = 1,
    
    [Display(Description = "Autoria")]
    Autoria = 2,
    
    [Display(Description = "CoAutor")]
    Coautor = 3,
}