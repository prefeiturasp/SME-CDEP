using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum TipoAcervoTag
{
    [Display(Name = "Biblioteca")]
    Biblioteca = 1,
    
    [Display(Name = "Memória Documental")]
    MemoriaDocumental = 2,
    
    [Display(Name = "Memória da Educação Municipal")]
    MemoriaEducacaoMunicipal = 3
}