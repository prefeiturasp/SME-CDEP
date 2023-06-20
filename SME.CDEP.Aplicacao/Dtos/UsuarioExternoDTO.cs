using System.ComponentModel.DataAnnotations;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class UsuarioExternoDTO
    {
        [Required(ErrorMessage = "É necessário informar o cpf.")]
        [StringLength(50, ErrorMessage = "O cpf não pode exceder 50 caracteres.")]  
        public string Cpf { get; set; }
       
        
        [Required(ErrorMessage = "É necessário informar o e-mail.")]
        [StringLength(500, ErrorMessage = "O e-mail não pode exceder 500 caracteres.")]
        public string Email { get; set; }
        
        
        [Required(ErrorMessage = "É necessário informar o nome.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; }
        
        
        [Required(ErrorMessage = "É necessário informar o telefone.")]
        [StringLength(15, ErrorMessage = "O telefone não pode exceder 15 caracteres.")]
        public string Telefone { get; set; }
        
        
        [Required(ErrorMessage = "É necessário informar o endereço.")]
        [StringLength(200, ErrorMessage = "O endereço não pode exceder 200 caracteres.")]
        public string Endereco { get; set; }
        
        public string? Complemento { get; set; }
        

        [Required(ErrorMessage = "É necessário informar o número.")]
        public int Numero { get; set; }
        
        
        [Required(ErrorMessage = "É necessário informar a cidade.")]
        [StringLength(50, ErrorMessage = "A cidade não pode exceder 50 caracteres.")]
        public string Cidade { get; set; }
        
        
        [Required(ErrorMessage = "É necessário informar o estado.")]
        public string Estado { get; set; }
        
        
        [Required(ErrorMessage = "É necessário informar o cep.")]
        [StringLength(10, ErrorMessage = "O cep não pode exceder 10 caracteres.")]
        public string Cep { get; set; }
        
        
        [Required(ErrorMessage = "É necessário informar a senha.")]
        [StringLength(12, ErrorMessage = "A senha não pode exceder 12 caracteres.")]
        public string Senha { get; set; }
        
        
        [Required(ErrorMessage = "É necessário informar confirmar senha.")]
        [StringLength(12, ErrorMessage = "O confirmar senha não pode exceder 12 caracteres.")]
        public string ConfirmarSenha { get; set; }
        
        
        [Required(ErrorMessage = "É necessário informar o tipo de perfil.")]
        public TipoUsuario TipoUsuario { get; set; }
        
        
        [Required(ErrorMessage = "É necessário informar o bairro.")]
        [StringLength(200, ErrorMessage = "O bairro não pode exceder 200 caracteres.")]
        public string Bairro { get; set; }
    }
}
