/**
 * Tabela de sumarização (datamart) para os dados consolidados
 * das consultas ao acervo, agrupados por mês.
 */
CREATE TABLE sumario_consultas_mensal (
    mes_referencia DATE PRIMARY KEY,
    total_consultas BIGINT NOT NULL DEFAULT 0,
    data_ultima_atualizacao TIMESTAMPTZ NOT NULL DEFAULT NOW()
);