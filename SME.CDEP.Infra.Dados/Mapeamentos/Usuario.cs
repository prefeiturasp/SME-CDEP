﻿using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class Usuario : BaseMapAuditavel<CDEP.Dominio.Entidades.Usuario>
    {
        public Usuario()
        {
            ToTable("usuario");
            Map(c => c.ExpiracaoRecuperacaoSenha).ToColumn("expiracao_recuperacao_senha");
            Map(c => c.Login).ToColumn("login");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.TokenRecuperacaoSenha).ToColumn("token_recuperacao_senha");
            Map(c => c.UltimoLogin).ToColumn("ultimo_login");
            Map(c => c.Telefone).ToColumn("telefone");
            Map(c => c.Endereco).ToColumn("endereco");
            Map(c => c.Numero).ToColumn("numero");
            Map(c => c.Complemento).ToColumn("complemento");
            Map(c => c.Cidade).ToColumn("cidade");
            Map(c => c.Estado).ToColumn("estado");
            Map(c => c.Cep).ToColumn("cep");
            Map(c => c.TipoUsuario).ToColumn("tipo");
            Map(c => c.Bairro).ToColumn("bairro");
        }
    }
}