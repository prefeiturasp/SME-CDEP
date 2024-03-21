using SME.CDEP.Dominio.Constantes;

namespace SME.CDEP.Aplicacao.Extensions;

public static class GuidExtension
{
    public static bool EhPerfilExterno(this Guid guid)
    {
        return guid.ToString().ToLower().Equals(Constantes.PERFIL_EXTERNO_GUID.ToLower());
    }
    
    public static bool EhPerfilAdminBiblioteca(this Guid guid)
    {
        return guid.ToString().ToLower().Equals(Constantes.PERFIL_ADMIN_BIBLIOTECA_GUID.ToLower());
    }
    
    public static bool EhPerfilAdminGeral(this Guid guid)
    {
        return guid.ToString().ToLower().Equals(Constantes.PERFIL_ADMIN_GERAL_GUID.ToLower());
    }
    
    public static bool EhPerfilBasico(this Guid guid)
    {
        return guid.ToString().ToLower().Equals(Constantes.PERFIL_BASICO_GUID.ToLower());
    }
    
    public static bool EhPerfilAdminMemoria(this Guid guid)
    {
        return guid.ToString().ToLower().Equals(Constantes.PERFIL_ADMIN_MEMORIA_GUID.ToLower());
    }
    
    public static bool EhPerfilAdminMemorial(this Guid guid)
    {
        return guid.ToString().ToLower().Equals(Constantes.PERFIL_ADMIN_MEMORIAL_GUID.ToLower());
    }
}