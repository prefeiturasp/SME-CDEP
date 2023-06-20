using Dapper;
using SME.CDEP.Dominio.Dominios;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioUsuario : RepositorioBase<Usuario>, IRepositorioUsuario
    {
        public RepositorioUsuario(ICdepConexao conexao) : base(conexao)
        { }

        public Task<Usuario> ObterPorLogin(string login)
            => conexao.Obter()
                .QueryFirstOrDefaultAsync<Usuario>("select id, criado_em, criado_por, alterado_em, alterado_por, " +
                                                      "criado_login, alterado_login, login, ultimo_login, nome, " +
                                                      "expiracao_recuperacao_senha, token_recuperacao_senha, cep, " +
                                                      "telefone, endereco, numero, complemento, cidade, estado, " +
                                                      "bairro, tipo " +
                                                      "from usuario " +
                                                      "where login = @login", new { login });
    }
}