using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioCromia : RepositorioBase<Cromia>, IRepositorioCromia
    {
        public RepositorioCromia(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }
        
        public Task<long> ObterPorNome(string nome)
        {
            return conexao.Obter().QueryFirstOrDefaultAsync<long>("select id from cromia where f_unaccent(lower(nome)) = f_unaccent(@nome) and not excluido ",new { nome = nome.ToLower() });
        }
    }
}