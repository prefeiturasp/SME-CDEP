using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioMaterial : RepositorioBase<Material>, IRepositorioMaterial
    {
        public RepositorioMaterial(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public Task<long> ObterPorNomeTipo(string material, TipoMaterial tipoMaterial)
        {
            return conexao.Obter().QueryFirstOrDefaultAsync<long>("select id from material where f_unaccent(lower(nome)) = f_unaccent(@material) and not excluido and tipo = @tipoMaterial",new { material = material.ToLower(), tipoMaterial });
        }
    }
}