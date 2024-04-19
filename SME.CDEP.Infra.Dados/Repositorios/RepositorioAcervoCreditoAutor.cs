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

        public async Task<IEnumerable<AcervoCreditoAutor>> ObterPorAcervoId(long id, bool incluirTipoAutoria = false)
            => await conexao.Obter()
                .QueryAsync<AcervoCreditoAutor>($@"select id, 
                                                          acervo_id, 
                                                          credito_autor_id,
                                                          tipo_autoria as tipoAutoria
                                                    from acervo_credito_autor 
                                                    where acervo_id = @id {IncluirTipoAutoria(incluirTipoAutoria)}",
                    new { id });

        private string IncluirTipoAutoria(bool incluirTipoAutoria)
        {
            return incluirTipoAutoria ? " and tipo_autoria is not null" : " and tipo_autoria is null";
        }

        public async Task Excluir(long creditoAutorId, string tipoAutoria, long acervoId)
        {
            await conexao.Obter()
                .ExecuteAsync(@"Delete from acervo_credito_autor where acervo_id = @acervoId and credito_autor_id = @creditoAutorId and tipo_autoria = @tipoAutoria", 
                new { acervoId, creditoAutorId, tipoAutoria });
        }
        
        public async Task Excluir(long[] creditosAutoresIdsExcluir, long acervoId)
        {
            await conexao.Obter()
                .ExecuteAsync(@"Delete from acervo_credito_autor where acervo_id = @acervoId and credito_autor_id = any(@creditosAutoresIdsExcluir) and tipo_autoria is null", 
                    new { acervoId, creditosAutoresIdsExcluir });
        }
    }
}