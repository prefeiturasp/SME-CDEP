using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Dominio.Enumerados
{
    public enum SituacaoAcervo
    {
        [Display(Description = "Ativo")]
        Ativo = 1,

        [Display(Description = "Inativo")]
        Inativo = 2,
    }
}
