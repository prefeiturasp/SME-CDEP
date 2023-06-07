using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.Dtos
{
    public class AutenticacaoDto
    {
        [Required(ErrorMessage = "É necessário informar o login.")]
        [MinLength(5, ErrorMessage = "O login deve conter no mínimo 5 caracteres.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "É necessário informar a senha.")]
        [MinLength(4, ErrorMessage = "A senha deve conter no mínimo 4 caracteres.")]
        public string Senha { get; set; }
    }
}
