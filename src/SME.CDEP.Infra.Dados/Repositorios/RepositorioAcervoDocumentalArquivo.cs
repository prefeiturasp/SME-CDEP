using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcervoDocumentalArquivo : RepositorioBaseSomenteId<AcervoDocumentalArquivo>, IRepositorioAcervoDocumentalArquivo
    {
        public RepositorioAcervoDocumentalArquivo(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public async Task<IEnumerable<AcervoDocumentalArquivo>> ObterPorAcervoDocumentalId(long id)
        => await conexao.Obter()
                .QueryAsync<AcervoDocumentalArquivo>(@"select id, 
                                                                  acervo_documental_id, 
                                                                  arquivo_id
                                                            from acervo_documental_arquivo 
                                                            where acervo_documental_id = @id", new { id });

        public async Task Excluir(long[] arquivosIdsExcluir, long acervoDocumentalId)
        {
            await conexao.Obter()
                .ExecuteAsync(@"Delete from acervo_documental_arquivo where acervo_documental_id = @acervoDocumentalId and arquivo_id = any(@arquivosIdsExcluir)", 
                new { acervoDocumentalId, arquivosIdsExcluir });
        }
    }
}