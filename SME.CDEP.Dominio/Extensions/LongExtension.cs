
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Extensions
{
    public static class LongExtension
    {
        public static bool EhAcervoDocumental(this long valor)
        {
            return valor == (long)TipoAcervo.DocumentacaoHistorica; 
        }
        
        public static bool NaoEhAcervoDocumental(this long valor)
        {
            return !EhAcervoDocumental(valor); 
        }
        
        public static bool EhMaiorQueZero(this long valor)
        {
            return valor > 0; 
        }
    }
}