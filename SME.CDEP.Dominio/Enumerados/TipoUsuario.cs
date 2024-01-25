using System.ComponentModel.DataAnnotations;
using SME.CDEP.Dominio.Extensions;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum TipoUsuario
{
    [Display(Description = "Usuário Interno")]
    CORESSO = 0,
    
    [Display(Description = "Usuário Externo - Servidor Público")]
    SERVIDOR_PUBLICO = 1,
    
    [Display(Description = "Usuário Externo - Estudante")]
    ESTUDANTE = 2,
    
    [Display(Description = "Usuário Externo - Professor")]
    PROFESSOR = 3,
    
    [Display(Description = "Usuário Externo - População Geral")]
    POPULACAO_GERAL = 4
    
}
public static class TipoUsuarioExtension
{
    public static bool EhCoreSSO(this TipoUsuario tipoUsuario)
    {
       return tipoUsuario == TipoUsuario.CORESSO;
    }
}