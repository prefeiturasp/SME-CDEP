/**
 * Tabela de sumarização (datamart) para os dados consolidados
 * das solicitações de acervos, agrupados por mês.
 */
CREATE TABLE sumario_solicitacoes_mensal (
	mes_referencia DATE PRIMARY KEY, total_solicitacoes BIGINT NOT NULL DEFAULT 0, data_ultima_atualizacao timestamptz NOT NULL DEFAULT now()
);