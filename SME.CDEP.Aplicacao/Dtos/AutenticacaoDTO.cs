using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class AutenticacaoDTO
    {
        [Required(ErrorMessage = "É necessário informar o login.")]
        [Range(5,12, ErrorMessage = "O login deve ter no mínimo 5 e no máximo 12 caracteres.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "É necessário informar a senha.")]
        [Range(4,12, ErrorMessage = "A senha deve ter no mínimo 4 e no máximo 12 caracteres.")]
        public string Senha { get; set; }
    }
}
