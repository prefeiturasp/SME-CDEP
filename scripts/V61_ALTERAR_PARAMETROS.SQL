INSERT INTO parametro_sistema (
    excluido,
    tipo,
    ano,
    ativo,
    criado_em,
    alterado_em,
    criado_login,
    nome,
    criado_por,
    descricao,
    valor,
    alterado_login,
    alterado_por
)
SELECT
    p.excluido,
    p.tipo,
    2026 AS ano,
    p.ativo,
    now() AS criado_em,
    now() AS alterado_em,
    p.criado_login,
    p.nome,
    p.criado_por,
    p.descricao,
    p.valor,
    p.alterado_login,
    p.alterado_por
FROM parametro_sistema p
WHERE p.ano = 2025
AND NOT EXISTS (
    SELECT 1
    FROM parametro_sistema ps
    WHERE ps.nome = p.nome
      AND ps.ano = 2026
);