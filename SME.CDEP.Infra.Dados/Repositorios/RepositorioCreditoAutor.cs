using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioCreditoAutor : RepositorioBase<CreditoAutor>, IRepositorioCreditoAutor
    {
        public RepositorioCreditoAutor(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public async Task<IList<CreditoAutor>> ObterTodosPorTipo(TipoCreditoAutoria tipo)
        => (await conexao.Obter()
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
                    new { tipo })).ToList();

        public async Task<IList<CreditoAutor>> PesquisarPorNomeTipo(string nome, TipoCreditoAutoria tipo)
        => (await conexao.Obter()
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
                    new { tipo })).ToList();
    }
}