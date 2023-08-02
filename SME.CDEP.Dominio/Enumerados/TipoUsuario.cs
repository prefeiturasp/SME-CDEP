using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum TipoUsuario
{
    [Display(Name = "Usuário Interno")]
    CORESSO = 0,
    
    [Display(Name = "Usuário Externo - Servidor Público")]
    SERVIDOR_PUBLICO = 1,
    
    [Display(Name = "Usuário Externo - Estudante")]
    ESTUDANTE = 2,
    
    [Display(Name = "Usuário Externo - Professor")]
    PROFESSOR = 3,
    
    [Display(Name = "Usuário Externo - População Geral")]
    POPULACAO_GERAL = 4
}