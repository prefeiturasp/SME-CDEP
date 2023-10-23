
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Extensions
{
    public static class LongExtension
    {
        public static bool EhAcervoDocumental(this long valor)
        {
            return valor == (int)TipoAcervo.DocumentacaoHistorica; 
        }
    }
}
