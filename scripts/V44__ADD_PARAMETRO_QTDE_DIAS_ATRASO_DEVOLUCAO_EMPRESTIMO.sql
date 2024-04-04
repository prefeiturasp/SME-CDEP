--> NotificarQtdeDiasAntesDoVencimentoEmprestimo
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'NotificarQtdeDiasAtrasoDevolucaoEmprestimo', 
       18,
       'Notificar usuário quantos dias após o atraso da devolução do empréstimo do acervo',
       '1',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'NotificarQtdeDiasAtrasoDevolucaoEmprestimo' and tipo = 18);

--> Modelo e-mail para aviso de atraso na devolução do acervo emprestado
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'ModeloEmailAvisoAtrasoDevolucaoEmprestimo', 
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
    <p>O prazo para devolução dos acervos abaixo expirou na data de ontem, por favor entre em contato com a equipe do CDEP por meio do formulário de contato disponível no endereço:</p>
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
where not exists (select 1 from public.parametro_sistema where nome = 'ModeloEmailAvisoAtrasoDevolucaoEmprestimo' and tipo = 19);