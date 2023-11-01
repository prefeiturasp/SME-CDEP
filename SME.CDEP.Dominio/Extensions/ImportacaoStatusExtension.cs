
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Extensions
{
    public static class ImportacaoStatusExtension
    {
        public static bool EhPendente(this ImportacaoStatus importacaoStatus)
        {
            return importacaoStatus == ImportacaoStatus.Pendente; 
        }
        
        public static bool EhErro(this ImportacaoStatus importacaoStatus)
        {
            return importacaoStatus == ImportacaoStatus.Erro; 
        }
        
        public static bool EhSucesso(this ImportacaoStatus importacaoStatus)
        {
            return importacaoStatus == ImportacaoStatus.Sucesso; 
        }
    }
}
