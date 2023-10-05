using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcessoDocumento : RepositorioBase<AcessoDocumento>, IRepositorioAcessoDocumento
    {
        public RepositorioAcessoDocumento(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public async Task<IEnumerable<AcessoDocumento>> ObterPorIds(long[] ids)
        {
            const string query = @"select * 
                                    from acesso_documento
                                    where id = ANY(@ids)";

            return await conexao.Obter().QueryAsync<AcessoDocumento>(query, new { ids });
        }
    }
}