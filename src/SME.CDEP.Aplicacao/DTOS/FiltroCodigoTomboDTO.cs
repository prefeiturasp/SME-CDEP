using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS;

public class FiltroCodigoTomboDTO
{
    [Required(ErrorMessage = "É necessário informar o código/tombo do acervo")]
    public string CodigoTombo { get; set; } = string.Empty;
}
