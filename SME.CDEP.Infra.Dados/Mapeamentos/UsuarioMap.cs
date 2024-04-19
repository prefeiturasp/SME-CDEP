namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class UsuarioMap : BaseMapAuditavel<CDEP.Dominio.Entidades.Usuario>
    {
        public UsuarioMap()
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
            Map(c => c.Email).ToColumn("email");
            Map(c => c.Instituicao).ToColumn("instituicao");
        }
    }
}