using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioEditora : RepositorioBase<Editora>, IRepositorioEditora
    {
        public RepositorioEditora(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }
        
        public Task<bool> Existe(string nome, long id)
        {
            return conexao.Obter().QueryFirstOrDefaultAsync<bool>("select 1 from editora where lower(nome) = @nome and not excluido and id != @id",new { id, nome = nome.ToLower() });
        }

        public Task<IEnumerable<Editora>> PesquisarPorNome(string nome)
        {
            return conexao.Obter().QueryAsync<Editora>($@"select id, 
                                                                    nome,
                                                                    excluido, 
                                                                    criado_em, 
                                                                    alterado_em, 
                                                                    criado_por, 
                                                                    alterado_por, 
                                                                    criado_login, 
                                                                    alterado_login 
                                                            from editora 
                                                            where lower(nome) like '%{nome.ToLower()}%' 
                                                              and not excluido ");
        }
        
        public Task<long> ObterPorNome(string nome)
        {
            return conexao.Obter().QueryFirstOrDefaultAsync<long>("select id from editora where f_unaccent(lower(nome)) = f_unaccent(@nome) and not excluido ",new { nome = nome.ToLower() });
        }
    }
}