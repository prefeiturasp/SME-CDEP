using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum TipoAcervo
{
    [Display(Name = "Bibliográfico")]
    Bibliografico = 1,
    
    [Display(Name = "Documentação histórica")]
    DocumentacaoHistorica = 2,
    
    [Display(Name = "Artes gráficas")]
    ArtesGraficas = 3,
    
    [Display(Name = "Audiovisual")]
    Audiovisual = 4,
    
    [Display(Name = "Fotográfico")]
    Fotografico = 5,
    
    [Display(Name = "Tridimensional")]
    Tridimensional = 6
}