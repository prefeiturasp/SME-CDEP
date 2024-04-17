using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioUsuario : RepositorioBaseAuditavel<Usuario>, IRepositorioUsuario
    {
        public RepositorioUsuario(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public Task<Usuario> ObterPorLogin(string login)
        {
            var query = @"select id, 
                                 criado_em, 
                                 criado_por, 
                                 alterado_em, 
                                 alterado_por, 
                                 criado_login, 
                                 alterado_login, 
                                 login, 
                                 ultimo_login, 
                                 nome, 
                                 expiracao_recuperacao_senha, 
                                 token_recuperacao_senha, 
                                 cep, 
                                 telefone, 
                                 endereco, 
                                 numero, 
                                 complemento, 
                                 cidade, 
                                 estado, 
                                 bairro, 
                                 tipo, 
                                 email, 
                                 instituicao 
                          from usuario 
                          where login = @login";
            return conexao.Obter().QueryFirstOrDefaultAsync<Usuario>(query, new { login });
        }
    }
}