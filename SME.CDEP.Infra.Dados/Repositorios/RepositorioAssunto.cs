using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAssunto : RepositorioBase<Assunto>, IRepositorioAssunto
    {
        public RepositorioAssunto(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public Task<bool> Existe(string nome, long id)
        {
            return conexao.Obter().QueryFirstOrDefaultAsync<bool>("select 1 from assunto where lower(nome) = @nome and not excluido and id != @id",new { id, nome = nome.ToLower() });
        }

        public async Task<IEnumerable<Assunto>> PesquisarPorNome(string nome)
        {
            return await conexao.Obter().QueryAsync<Assunto>($@"select id, 
                                                                    nome,
                                                                    excluido, 
                                                                    criado_em, 
                                                                    alterado_em, 
                                                                    criado_por, 
                                                                    alterado_por, 
                                                                    criado_login, 
                                                                    alterado_login 
                                                            from assunto 
                                                            where lower(nome) like '%{nome.ToLower()}%' 
                                                              and not excluido ");
        }
        
        public async Task<IEnumerable<Assunto>> ObterPorIds(long[] ids)
        {
            const string query = @"select * 
                                    from assunto
                                    where id = ANY(@ids)";

            return await conexao.Obter().QueryAsync<Assunto>(query, new { ids });
        }
    }
}