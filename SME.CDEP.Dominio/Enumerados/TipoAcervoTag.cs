using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum TipoAcervoTag
{
    [Display(Description = "Biblioteca")]
    Biblioteca = 1,
    
    [Display(Description = "Memória Documental")]
    MemoriaDocumental = 2,
    
    [Display(Description = "Memória da Educação Municipal")]
    MemoriaEducacaoMunicipal = 3
}