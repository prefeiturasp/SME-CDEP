
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Extensions
{
    public static class TipoAcervoExtension
    {
        public static bool EhAcervoDocumental(this TipoAcervo tipo)
        {
            return tipo == TipoAcervo.DocumentacaoTextual; 
        }
        
        public static bool EhAcervoArteGraficaOuFotograficoOuTridimensional(this TipoAcervo tipo)
        {
            return tipo == TipoAcervo.ArtesGraficas || tipo == TipoAcervo.Fotografico || tipo == TipoAcervo.Tridimensional;  
        }
        
        public static bool NaoSaoIguais(this TipoAcervo tipo, TipoAcervo tipoAAvailar)
        {
            return tipo != tipoAAvailar;  
        }
    }
}
