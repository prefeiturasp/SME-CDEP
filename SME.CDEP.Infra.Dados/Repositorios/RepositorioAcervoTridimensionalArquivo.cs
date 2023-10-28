using Dapper;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    public class RepositorioAcervoTridimensionalArquivo : RepositorioBaseSomenteId<AcervoTridimensionalArquivo>, IRepositorioAcervoTridimensionalArquivo
    {
        public RepositorioAcervoTridimensionalArquivo(IContextoAplicacao contexto, ICdepConexao conexao) : base(contexto,conexao)
        { }

        public async Task<IEnumerable<AcervoTridimensionalArquivo>> ObterPorAcervoTridimensionalId(long id)
            => await conexao.Obter()
                .QueryAsync<AcervoTridimensionalArquivo>(@"select id, 
                                                                  acervo_tridimensional_id, 
                                                                  arquivo_id
                                                            from acervo_tridimensional_arquivo 
                                                            where acervo_tridimensional_id = @id", new { id });

        public async Task Excluir(long[] arquivosIdsExcluir, long acervoTridimensionalId)
        {
            await conexao.Obter()
                .ExecuteAsync(@"Delete from acervo_tridimensional_arquivo where acervo_tridimensional_id = @acervoArteGraficaId and arquivo_id = any(@arquivosIdsExcluir)", 
                new { acervoArteGraficaId = acervoTridimensionalId, arquivosIdsExcluir });
        }
    }
}