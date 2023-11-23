using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Infra.Dominio.Enumerados
{
    public enum TipoArquivo
    {
        [Display(Name ="temp")] 
        Temp = 1,
        
        [Display(Name ="temp")] 
        Editor = 2,

        [Display(Name ="acervo-fotografico")] 
        AcervoFotografico = 3,
        
        [Display(Name ="acervo-arte-grafica")] 
        AcervoArteGrafica = 4,
        
        [Display(Name ="acervo-tridimensional")] 
        AcervoTridimensional = 5,
        
        [Display(Name ="acervo-documental")] 
        AcervoDocumental = 6,
        
        [Display(Name ="sistema")] 
        Sistema = 7,
    }
}