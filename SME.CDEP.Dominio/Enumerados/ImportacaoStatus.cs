using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum ImportacaoStatus
{
    [Display(Name = "Pendente")]
    Pendente = 1,
    
    [Display(Name = "Erro")]
    Erro = 2,
    
    [Display(Name = "Sucesso")]
    Sucesso = 3
}