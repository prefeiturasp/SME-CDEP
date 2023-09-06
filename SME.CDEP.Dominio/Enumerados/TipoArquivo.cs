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
    }
}