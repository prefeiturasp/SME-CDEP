using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioArquivo : RepositorioBase<Arquivo>, IRepositorioArquivo
    {
        public RepositorioArquivo(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public async Task<IEnumerable<Arquivo>> ObterPorCodigos(string[] codigos)
            => await conexao.Obter()
                .QueryAsync<Arquivo>(@"select id, 
                                               criado_em, 
                                               criado_por, 
                                               alterado_em, 
                                               alterado_por, 
                                               criado_login, 
                                               alterado_login, 
                                               codigo, 
                                               nome, 
                                               tipo_conteudo, 
                                               tipo
                                        from arquivo 
                                        where codigo = any(@codigos)", new { codigos = codigos.Select(s=> new Guid(s)).ToArray() });
    }
}