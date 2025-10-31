using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcervoArteGraficaArquivo : RepositorioBaseSomenteId<AcervoArteGraficaArquivo>, IRepositorioAcervoArteGraficaArquivo
    {
        public RepositorioAcervoArteGraficaArquivo(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public async Task<IEnumerable<AcervoArteGraficaArquivo>> ObterPorAcervoArteGraficaId(long id)
            => await conexao.Obter()
                .QueryAsync<AcervoArteGraficaArquivo>(@"select id, 
                                                                  acervo_arte_grafica_id, 
                                                                  arquivo_id
                                                            from acervo_arte_grafica_arquivo 
                                                            where acervo_arte_grafica_id = @id", new { id });

        public async Task Excluir(long[] arquivosIdsExcluir, long acervoArteGraficaId)
        {
            await conexao.Obter()
                .ExecuteAsync(@"Delete from acervo_arte_grafica_arquivo where acervo_arte_grafica_id = @acervoArteGraficaId and arquivo_id = any(@arquivosIdsExcluir)", 
                new { acervoArteGraficaId, arquivosIdsExcluir });
        }
    }
}