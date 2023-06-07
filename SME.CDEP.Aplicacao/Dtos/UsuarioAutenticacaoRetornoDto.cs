using System;

namespace SME.CDEP.Aplicacao.Dtos;
 
    public class UsuarioAutenticacaoRetornoDto
    {
        public DateTime DataHoraExpiracao { get; set; }
        public string Token { get; set; }
        public string UsuarioLogin { get; set; }
        public string UsuarioNome { get; set; }
        public string Email { get; set; }
        public Guid Perfil { get; set; }
        public string PerfilNome { get; set; }
        public bool Autenticado { get; set; }
    }