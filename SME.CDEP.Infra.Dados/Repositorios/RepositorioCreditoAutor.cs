using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioCreditoAutor : RepositorioBase<CreditoAutor>, IRepositorioCreditoAutor
    {
        public RepositorioCreditoAutor(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto, conexao)
        {
        }

        public async Task<IEnumerable<CreditoAutor>> ObterTodosPorTipo(TipoCreditoAutoria tipo)
            => await conexao.Obter()
                .QueryAsync<CreditoAutor>(@"select id, 
                                                       criado_em, 
                                                       criado_por, 
                                                       alterado_em, 
                                                       alterado_por, 
                                                       criado_login, 
                                                       alterado_login, 
                                                       excluido,
                                                       nome, 
                                                       tipo 
                                                from credito_autor
                                                where tipo = @tipo
                                                  and not excluido",
                    new { tipo });

        public async Task<IEnumerable<CreditoAutor>> PesquisarPorNomeTipo(string nome, TipoCreditoAutoria tipo)
            => await conexao.Obter()
                .QueryAsync<CreditoAutor>($@"select id, 
                                                       criado_em, 
                                                       criado_por, 
                                                       alterado_em, 
                                                       alterado_por, 
                                                       criado_login, 
                                                       alterado_login,
                                                       excluido,
                                                       nome, 
                                                       tipo 
                                                from credito_autor
                                                where tipo = @tipo
                                                  and lower(nome) like '%{nome.ToLower()}%' and not excluido ",
                    new { tipo });

        public async Task<bool> Existe(string nome, long id, int tipo)
            => await conexao.Obter().QueryFirstOrDefaultAsync<bool>(
                "select 1 from credito_autor where lower(nome) = @nome and tipo = @tipo and not excluido and id != @id",
                new { id, nome = nome.ToLower(), tipo });

        public async Task<IEnumerable<CreditoAutor>> PesquisarPorNome(string nome, int tipo)
            => await conexao.Obter().QueryAsync<CreditoAutor>($@"select id, 
                                                                    nome,
                                                                    excluido, 
                                                                    criado_em, 
                                                                    alterado_em, 
                                                                    criado_por, 
                                                                    alterado_por, 
                                                                    criado_login, 
                                                                    alterado_login 
                                                            from credito_autor 
                                                            where lower(nome) like '%{nome.ToLower()}%' 
                                                              and tipo = @tipo
                                                              and not excluido ", new { tipo });

        public Task<long> ObterPorNomeTipo(string nome, TipoCreditoAutoria tipoCreditoAutoria)
        {
            return conexao.Obter()
                .QueryFirstOrDefaultAsync<long>("select id from credito_autor where f_unaccent(lower(nome)) = f_unaccent(@nome)  and not excluido and tipo = @tipoCreditoAutoria",
                    new { nome = nome.ToLower(),tipoCreditoAutoria });
        }
    }
}