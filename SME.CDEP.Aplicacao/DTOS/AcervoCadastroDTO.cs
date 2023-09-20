using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoCadastroDTO
{
    [Required(ErrorMessage = "É necessário informar o título do acervo fotográfico")]
    [MaxLength(500, ErrorMessage = "A localização do acervo fotográfico não pode conter mais que 500 caracteres")]
    public string Titulo { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o Tombo do acervo fotográfico")]
    [MaxLength(13, ErrorMessage = "O Tombo do acervo fotográfico não pode conter mais que 13 caracteres")]
    public string Codigo { get; set; }
    
    [Required(ErrorMessage = "É necessário informar ao menos um crédito ou autoria do acervo fotográfico")]
    public long[] CreditosAutoresIds { get; set; }
}