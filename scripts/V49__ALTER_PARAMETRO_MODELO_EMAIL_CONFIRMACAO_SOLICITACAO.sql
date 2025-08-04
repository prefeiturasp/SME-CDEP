update public.parametro_sistema set valor =
'<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Confirmação de Solicitação</title>
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
    <p>Sua solicitação foi confirmada e você deverá se dirigir até a sede do CDEP para realizar a visita/retirada. </p>
    #CONTEUDO_TABELA    
	<p>#ENDERECO_SEDE_CDEP_VISITA</p>
	<p>#HORARIO_FUNCIONAMENTO_SEDE_CDEP</p>
	<p>Para mais detalhes, entre em contato com a equipe do CDEP por meio do formulário de contato disponível no endereço:</p>
    <p><a href="#LINK_FORMULARIO_CDEP">Formulário de Contato do CDEP</a></p>
</body>
</html>
'
where nome = 'ModeloEmailConfirmacaoSolicitacao' and tipo = 12