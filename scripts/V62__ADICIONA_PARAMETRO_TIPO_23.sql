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
    FALSE, 
    23, 
    NULL, 
    TRUE, 
    now(), 
    NULL, 
    'Sistema', 
    'PrazoEncerramentoAutomaticoSolicitacao', 
    'Sistema', 
    'Prazo em dias da data de registro da solicitação, para que a solicitação seja finalizada', 
    '30', 
    NULL, 
    NULL
WHERE NOT EXISTS (
    SELECT 1 FROM parametro_sistema WHERE tipo = 23
);