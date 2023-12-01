using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum TipoCreditoAutoria
{
    [Display(Name = "Crédito")]
    Credito = 1,
    
    [Display(Name = "Autoria")]
    Autoria = 2,
    
    [Display(Name = "CoAutor")]
    Coautor = 3,
}