using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Infra.Dominio.Enumerados
{
    public enum TipoArquivo
    {
        [Display(Description ="temp")] 
        Temp = 1,
        
        [Display(Description ="temp")] 
        Editor = 2,

        [Display(Description ="Acervo fotografico")] 
        AcervoFotografico = 3,
        
        [Display(Description ="Acervo arte grafica")] 
        AcervoArteGrafica = 4,
        
        [Display(Description ="Acervo tridimensional")] 
        AcervoTridimensional = 5,
        
        [Display(Description ="Acervo documental")] 
        AcervoDocumental = 6,
        
        [Display(Description ="Sistema")] 
        Sistema = 7,
    }

    public static class TipoArquivoExtension
    {
        public static bool EhTipoArquivoSistema(this TipoArquivo tipoArquivo)
        {
            return tipoArquivo == TipoArquivo.Sistema;
        }
        
        public static bool NaoEhTipoArquivoSistema(this TipoArquivo tipoArquivo)
        {
            return !tipoArquivo.EhTipoArquivoSistema();
        }
    }
}