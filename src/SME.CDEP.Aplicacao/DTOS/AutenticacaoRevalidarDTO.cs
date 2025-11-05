using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class AutenticacaoRevalidarDTO
    {
        [Required(ErrorMessage = "Informe o token para revalidar")]
        public string Token { get; set; }
    }
}
