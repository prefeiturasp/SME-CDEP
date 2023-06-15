﻿using System.ComponentModel.DataAnnotations;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class UsuarioExternoDTO
    {
        [Required(ErrorMessage = "É necessário informar o login.")]
        public string Login { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o e-mail.")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o nome.")]
        public string Nome { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o telefone.")]
        public string Telefone { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o endereço.")]
        public string Endereco { get; set; }
        
        public string Complemento { get; set; }

        [Required(ErrorMessage = "É necessário informar o número.")]
        public int Numero { get; set; }
        
        [Required(ErrorMessage = "É necessário informar a cidade.")]
        public string Cidade { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o estado.")]
        public string Estado { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o cep.")]
        public string Cep { get; set; }
        
        [Required(ErrorMessage = "É necessário informar a senha.")]
        public string Senha { get; set; }
        
        [Required(ErrorMessage = "É necessário informar confirmar senha.")]
        public string ConfirmarSenha { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o tipo de perfil.")]
        public TipoUsuario TipoUsuario { get; set; }
    }
}
