ALTER TABLE sumario_solicitacoes_mensal
ADD COLUMN IF NOT EXISTS total_solicitacoes_automaticas BIGINT NOT NULL DEFAULT 0,
ADD COLUMN IF NOT EXISTS total_solicitacoes_manuais BIGINT NOT NULL DEFAULT 0;

-- Carga inicial
INSERT INTO sumario_solicitacoes_mensal (mes_referencia, total_solicitacoes, total_solicitacoes_automaticas, total_solicitacoes_manuais)
SELECT date_trunc('month', solicitacao.data_solicitacao) AS MesAno
     , COUNT(*) AS TotalConsultas
     , COUNT(*) FILTER (WHERE item.situacao = 3) AS TotalAtendimentoAutomatico
     , COUNT(*) FILTER (WHERE item.situacao = 5) AS TotalAtendimentoManual
  FROM acervo_solicitacao_item item
       INNER JOIN acervo_solicitacao solicitacao ON item.acervo_solicitacao_id = solicitacao.id
WHERE NOT solicitacao.excluido
  AND NOT solicitacao.excluido
GROUP BY MesAno
ON CONFLICT (mes_referencia)
DO UPDATE set 
 total_solicitacoes = EXCLUDED.total_solicitacoes,
 total_solicitacoes_automaticas = EXCLUDED.total_solicitacoes_automaticas,
 total_solicitacoes_manuais = EXCLUDED.total_solicitacoes_manuais,
 data_ultima_atualizacao = NOW();