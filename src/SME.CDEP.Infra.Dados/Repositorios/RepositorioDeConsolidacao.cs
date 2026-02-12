using Dapper;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
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

        public async Task ConsolidarMesDasSolicitacoesDeAcervosAsync(DateTime inicio, DateTime fim)
        {
            const string queryUpsert = """
             INSERT INTO sumario_solicitacoes_mensal (mes_referencia, total_solicitacoes, total_solicitacoes_automaticas, total_solicitacoes_manuais)
             SELECT date_trunc('month', solicitacao.data_solicitacao) AS MesAno
                  , COUNT(*) AS TotalConsultas
                  , COUNT(*) FILTER (WHERE item.situacao = @finalizadoAutomaticamente) AS TotalAtendimentoAutomatico
                  , COUNT(*) FILTER (WHERE item.situacao = @finalizadoManualmente) AS TotalAtendimentoManual
               FROM acervo_solicitacao_item item
                    INNER JOIN acervo_solicitacao solicitacao ON item.acervo_solicitacao_id = solicitacao.id
             WHERE NOT solicitacao.excluido
               AND NOT solicitacao.excluido
               AND solicitacao.data_solicitacao >= @inicio AND solicitacao.data_solicitacao < @fim
             GROUP BY MesAno
             ON CONFLICT (mes_referencia)
             DO UPDATE set 
              total_solicitacoes = EXCLUDED.total_solicitacoes,
              total_solicitacoes_automaticas = EXCLUDED.total_solicitacoes_automaticas,
              total_solicitacoes_manuais = EXCLUDED.total_solicitacoes_manuais,
              data_ultima_atualizacao = NOW();
             """;

            var parametros = new { 
                inicio, 
                fim,
                finalizadoAutomaticamente = SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE,
                finalizadoManualmente = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
            };
            await conexao.Obter().ExecuteAsync(queryUpsert, parametros);
        }
    }
}
