using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcervoFotograficoArquivo : RepositorioBaseSomenteId<AcervoFotograficoArquivo>, IRepositorioAcervoFotograficoArquivo
    {
        public RepositorioAcervoFotograficoArquivo(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public async Task<IEnumerable<AcervoFotograficoArquivo>> ObterPorAcervoFotograficoId(long id)
            => await conexao.Obter()
                .QueryAsync<AcervoFotograficoArquivo>(@"select id, 
                                                                  acervo_fotografico_id, 
                                                                  arquivo_id
                                                            from acervo_fotografico_arquivo 
                                                            where acervo_fotografico_id = @id", new { id });

        public async Task Excluir(long[] arquivosIdsExcluir, long acervoFotograficoId)
        {
            await conexao.Obter()
                .ExecuteAsync(@"Delete from acervo_fotografico_arquivo where acervo_fotografico_id = @acervoFotograficoId and arquivo_id = any(@arquivosIdsExcluir)", 
                new { acervoFotograficoId, arquivosIdsExcluir });
        }
    }
}