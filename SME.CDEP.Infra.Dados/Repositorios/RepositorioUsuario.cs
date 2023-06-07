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
                .QueryFirstOrDefaultAsync<Usuario>("select * from usuario where login = @login", new { login });
    }
}