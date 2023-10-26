
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Extensions
{
    public static class TipoAcervoExtension
    {
        public static bool EhAcervoDocumental(this TipoAcervo tipo)
        {
            return tipo == TipoAcervo.DocumentacaoHistorica; 
        }
        
        public static bool EhAcervoArteGraficaOuFotografico(this TipoAcervo tipo)
        {
            return tipo == TipoAcervo.ArtesGraficas || tipo == TipoAcervo.Fotografico;  
        }
    }
}
