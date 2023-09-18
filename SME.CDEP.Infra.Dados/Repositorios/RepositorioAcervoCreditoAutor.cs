using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcervoCreditoAutor : RepositorioBaseSomenteId<AcervoCreditoAutor>, IRepositorioAcervoCreditoAutor
    {
        public RepositorioAcervoCreditoAutor(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public async Task<IEnumerable<AcervoCreditoAutor>> ObterPorAcervoId(long id)
            => await conexao.Obter()
                .QueryAsync<AcervoCreditoAutor>(@"select id, 
                                                          acervo_id, 
                                                          credito_autor_id
                                                    from acervo_credito_autor 
                                                    where acervo_id = @id", new { id });

        public async Task Excluir(long[] creditosAutoresIdsExcluir, long acervoId)
        {
            await conexao.Obter()
                .ExecuteAsync(@"Delete from acervo_credito_autor where acervo_id = @acervoId and credito_autor_id = any(@creditosAutoresIdsExcluir)", 
                new { acervoId, creditosAutoresIdsExcluir });
        }
    }
}