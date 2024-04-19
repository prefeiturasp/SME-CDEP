using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioSuporte : RepositorioBase<Suporte>, IRepositorioSuporte
    {
        public RepositorioSuporte(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public Task<long> ObterPorNomeTipo(string nome, int tipoSuporte)
        {
            return conexao.Obter().QueryFirstOrDefaultAsync<long>("select id from suporte where f_unaccent(lower(nome)) = f_unaccent(@nome) and not excluido and tipo = @tipoSuporte",new { nome = nome.ToLower(), tipoSuporte });
        }
    }
}