﻿namespace SME.CDEP.Aplicacao.DTOS;
    public class RetornoPerfilUsuarioDTO
    {
        public string UsuarioNome { get; set; }
        public string UsuarioLogin { get; set; }
        public DateTime DataHoraExpiracao { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public bool Autenticado { get; set; }
        public IEnumerable<PerfilUsuarioDTO> PerfilUsuario { get; set; }
    }
