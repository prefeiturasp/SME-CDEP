using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioFormato : RepositorioBase<Formato>, IRepositorioFormato
    {
        public RepositorioFormato(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public Task<long> ObterPorNomeETipo(string nome, int tipoFormato)
        {
            return conexao.Obter().QueryFirstOrDefaultAsync<long>("select id from formato where f_unaccent(lower(nome)) = f_unaccent(@nome) and not excluido and tipo = @tipoFormato",new { nome = nome.ToLower(), tipoFormato });
        }
    }
}