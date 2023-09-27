using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoCadastroDTO
{
    [Required(ErrorMessage = "É necessário informar o título do acervo")]
    [MaxLength(500, ErrorMessage = "A localização do acervo não pode conter mais que 500 caracteres")]
    public string Titulo { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o Tombo do acervo")]
    [MaxLength(12, ErrorMessage = "O Tombo do acervo fotográfico não pode conter mais que 12 caracteres")]
    public string Codigo { get; set; }
    
    public long[]? CreditosAutoresIds { get; set; }
}