using Dapper;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SME.CDEP.Infra.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioDeConsolidacao(ICdepConexao conexao) : IRepositorioDeConsolidacao
    {
        public async Task ConsolidarMesDoHistoricoDeConsultasAsync(DateTime inicio, DateTime fim)
        {
            const string queryUpsert = @"
                INSERT INTO sumario_consultas_mensal (mes_referencia, total_consultas)
                SELECT date_trunc('month', data_consulta) as MesAno, 
                       COUNT(id) as TotalConsultas
                  FROM historico_consultas_acervos
                 WHERE data_consulta >= @inicio AND data_consulta < @fim
                 GROUP BY MesAno
                ON CONFLICT (mes_referencia)
                DO UPDATE set 
                 total_consultas = EXCLUDED.total_consultas,
                 data_ultima_atualizacao = NOW();";

            var parametros = new { inicio, fim };
            await conexao.Obter().ExecuteAsync(queryUpsert, parametros);
        }
    }
}
