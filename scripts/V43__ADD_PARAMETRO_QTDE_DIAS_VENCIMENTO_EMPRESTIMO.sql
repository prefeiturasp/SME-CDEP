--> NotificarQtdeDiasAntesDoVencimentoEmprestimo
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'NotificarQtdeDiasAntesDoVencimentoEmprestimo', 
       16,
       'Notificar usuário quantos dias antes do vencimento do empréstimo',
       '1',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'NotificarQtdeDiasAntesDoVencimentoEmprestimo' and tipo = 16);

--> Modelo e-mail para aviso de término de empréstimo
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'ModeloEmailAvisoDevolucaoEmprestimo', 
       17,
       'Modelo de conteúdo do e-mail a ser enviado quando avisado sobre o término do empréstimo do acervo',
       '<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Vencimento do empréstimo</title>
    <style>
        table {
            border-collapse: collapse;
            width: 100%;
        }
        th, td {
            border: 1px solid #dddddd;
            text-align: left;
            padding: 8px;
        }
        th {
            background-color: #f2f2f2;
        }
    </style>
</head>
<body>
    <p><strong>Olá, #NOME</strong></p>
    <p>O prazo para devolução dos acervos abaixo vence em um dia, não esqueça de fazer a devolução na data combinada.</p>
    #CONTEUDO_TABELA
    <p>Para verificar a possibilidade de estender o prazo de devolução, entre em contato com a equipe do CDEP por meio do formulário de contato disponível no endereço:</p>
    <p><a href="#LINK_FORMULARIO_CDEP">Formulário de Contato do CDEP</a></p>
</body>
</html>
',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'ModeloEmailAvisoDevolucaoEmprestimo' and tipo = 17);