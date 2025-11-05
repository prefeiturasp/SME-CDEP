/**
 * Tabela para armazenar o histórico de consultas (pesquisas)
 * realizadas na tela de "Consulta Acervos" (Sintaxe PostgreSQL).
 */
CREATE table if not exists  historico_consultas_acervos (
    id BIGSERIAL PRIMARY KEY,
    termo_pesquisado VARCHAR(1000) NULL,
    ano_inicial SMALLINT NULL,
    ano_final SMALLINT NULL,
    tipo_acervo INT NULL,
    data_consulta TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    quantidade_resultados INT NOT NULL DEFAULT 0
);

-- --- Índices ---

-- Criar um índice para análises futuras dos termos mais buscados
CREATE INDEX if not exists IX_historico_consultas_acervos_termo 
ON historico_consultas_acervos(termo_pesquisado);