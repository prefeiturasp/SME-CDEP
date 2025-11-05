using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum Origem
{
    [Display(Description = "Portal")]
    Portal = 1,
    
    [Display(Description = "Manual")]
    Manual = 2
}

public static class OrigemExtension
{
    public static bool EhViaPortal(this Origem origem)
    {
       return origem == Origem.Portal;
    }
    
    public static bool EhManual(this Origem situacaoSolicitacao)
    {
        return situacaoSolicitacao == Origem.Manual;
    }
}