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
    excluido,
    tipo,
    2026 AS ano,
    ativo,
    now() AS criado_em,
    now() AS alterado_em,
    criado_login,
    nome,
    criado_por,
    descricao,
    valor,
    alterado_login,
    alterado_por
FROM parametro_sistema
WHERE ano = 2025;
