--> QtdeDiasAntesDoVencimentoEmprestimo
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'QtdeDiasAntesDoVencimentoEmprestimo', 
       16,
       'Notificar usuário quantos dias antes do vencimento do empréstimo',
       '1',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'QtdeDiasAntesDoVencimentoEmprestimo' and tipo = 16);

--> Modelo e-mail para aviso de término de empréstimo
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'TemplateAvisoDevolucaoEmprestimo', 
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
where not exists (select 1 from public.parametro_sistema where nome = 'TemplateAvisoDevolucaoEmprestimo' and tipo = 17);
---------------------------------------------------
--> QtdeDiasAntesDoVencimentoEmprestimo
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'QtdeDiasAtrasoDevolucaoEmprestimo', 
       18,
       'Notificar usuário quantos dias após o atraso da devolução do empréstimo do acervo',
       '1',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'QtdeDiasAtrasoDevolucaoEmprestimo' and tipo = 18);

--> Modelo e-mail para aviso de atraso na devolução do acervo emprestado
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'TemplateAvisoAtrasoDevolucaoEmprestimo', 
       19,
       'Modelo de conteúdo do e-mail a ser enviado quando avisado sobre o atraso na devolução do acervo emprestado',
       '<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Atraso na devolução do acervo emprestado</title>
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
    <p>O prazo para devolução dos acervos abaixo expirou no dia #DATA_DEVOLUCAO_PROGRAMADA, por favor entre em contato com a equipe do CDEP por meio do formulário de contato disponível no endereço:</p>
    <p><a href="#LINK_FORMULARIO_CDEP">Formulário de Contato do CDEP</a></p>
	#CONTEUDO_TABELA    
</body>
</html>
',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'TemplateAvisoAtrasoDevolucaoEmprestimo' and tipo = 19);

--> NotificarQtdeDiasAtrasoProlongadoDevolucaoEmprestimo
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'QtdeDiasAtrasoProlongadoDevolucaoEmprestimo', 
       20,
       'Notificar usuário quantos dias após o atraso prolongado da devolução do empréstimo',
       '5',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'QtdeDiasAtrasoProlongadoDevolucaoEmprestimo' and tipo = 20);

--> Modelo e-mail para aviso de atraso prolongado na devolução do acervo emprestado
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'TemplateAvisoAtrasoProlongadoDevolucaoEmprestimo', 
       21,
       'Modelo de conteúdo do e-mail enviado quando ocorrer o atraso prolongado na devolução do emprestado',
       '<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Atraso prolongado na devolução do acervo emprestado</title>
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
    <p>O prazo para devolução dos acervos abaixo expirou no dia #DATA_DEVOLUCAO_PROGRAMADA, por favor entre em contato com a equipe do CDEP por meio do formulário de contato disponível no endereço:</p>
    <p><a href="#LINK_FORMULARIO_CDEP">Formulário de Contato do CDEP</a></p>
	#CONTEUDO_TABELA    
</body>
</html>
',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'TemplateAvisoAtrasoProlongadoDevolucaoEmprestimo' and tipo = 21);