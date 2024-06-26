--> Endereço do contato do CDEP
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'EnderecoContatoCDEPConfirmacaoCancelamentoVisita', 
       2,
       'Endereço para entrar em contato com equipe do CDEP, após cancelamento ou confirmação da visita',
       'https://hom-educacao.sme.prefeitura.sp.gov.br/cdep/contato/',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'EnderecoContatoCDEPConfirmacaoCancelamentoVisita' and tipo = 2);

--> E-mail remetente
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'EmailRemetente', 
       3,
       'Endereço de e-mail do remetente de envio de e-mails',
       'cdep-nao_responder@sme.prefeitura.sp.gov.br',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'EmailRemetente' and tipo = 3);

--> Nome do remetente de E-mail
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'NomeRemetenteEmail', 
       4,
       'Nome do remetente de envio de e-mails',
       'CDEP - Não responder',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'NomeRemetenteEmail' and tipo = 4);

--> Endereço de E-mail
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'EnderecoSMTP', 
       5,
       'Endereço do SMTP de envio de e-mails',
       'smtp.office365.com',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'EnderecoSMTP' and tipo = 5);

--> Usuário rementente do E-mail
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'UsuarioRemetenteEmail', 
       6,
       'Usuário do rementente do envio de e-mails',
       'cdep-nao_responder@sme.prefeitura.sp.gov.br',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'UsuarioRemetenteEmail' and tipo = 6);

--> Senha envio de E-mail
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'SenhaRemetenteEmail', 
       7,
       'Senha do remetente de envio de e-mails',
       '247@Vzi#00',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'SenhaRemetenteEmail' and tipo = 7);

--> Usar TLS
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'UsarTLSEmail', 
       8,
       'Enviar e-mail usando protocolo TLS',
       'false',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'UsarTLSEmail' and tipo = 8);

--> Porta
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'PortaEnvioEmail', 
       9,
       'Número da porta para envio do e-mail',
       '587',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'PortaEnvioEmail' and tipo = 9);

--> Modelo e-mail para cancelamento de solicitação
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'ModeloEmailCancelamentoSolicitacao', 
       10,
       'Modelo de conteúdo do e-mail a ser enviado quando cancelar atendimento',
       '<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Cancelamento de Solicitação</title>
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
    <p>Os acervos abaixo não estão disponíveis no momento, por isso sua solicitação foi cancelada:</p>
    #CONTEUDO_TABELA
    <p>Para mais detalhes, entre em contato com a equipe do CDEP por meio do formulário de contato disponível no endereço:</p>
    <p><a href="#LINK_FORMULARIO_CDEP">Formulário de Contato do CDEP</a></p>
</body>
</html>
',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'ModeloEmailCancelamentoSolicitacao' and tipo = 10);

--> Modelo e-mail para cancelamento de solicitação item
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'ModeloEmailCancelamentoSolicitacaoItem', 
       11,
       'Modelo de conteúdo do e-mail a ser enviado quando cancelar do item do atendimento',
       '<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Cancelamento de Item da Solicitação</title>
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
    <p>A solicitação do acervo abaixo foi cancelada:</p>
    #CONTEUDO_TABELA
    <p>Para mais detalhes, entre em contato com a equipe do CDEP por meio do formulário de contato disponível no endereço:</p>
    <p><a href="#LINK_FORMULARIO_CDEP">Formulário de Contato do CDEP</a></p>
</body>
</html>
',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'ModeloEmailCancelamentoSolicitacaoItem' and tipo = 11);

--> Modelo e-mail para confirmação da solicitação
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'ModeloEmailConfirmacaoSolicitacao', 
       12,
       'Modelo de conteúdo do e-mail a ser enviado quando confirmar o atendimento',
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
    <p>Caso não consiga comparecer nesta data, acesse o CDEP e altere a data na sua solicitação que está disponível na página inicial.</p>
    #CONTEUDO_TABELA    
	<p>#ENDERECO_SEDE_CDEP_VISITA</p>
	<p>#HORARIO_FUNCIONAMENTO_SEDE_CDEP</p>
	<p>Para mais detalhes, entre em contato com a equipe do CDEP por meio do formulário de contato disponível no endereço:</p>
    <p><a href="#LINK_FORMULARIO_CDEP">Formulário de Contato do CDEP</a></p>
</body>
</html>
',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'ModeloEmailConfirmacaoSolicitacao' and tipo = 12);

--> Horário de funcionamento do CDEP para visitação
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'EnderecoSedeCDEPVisita', 
       13,
       'Endereço da sede do CDEP para visitação',
       'Endereço para visita: R. Estado de Israel, 200 - Vila Clementino, São Paulo - SP',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'EnderecoSedeCDEPVisita' and tipo = 13);

--> Horário de funcionamento do CDEP para visitação
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'HorarioFuncionamentoSedeCDEPVisita', 
       14,
       'Horário de funcionamento do CDEP para visitação',
       'Horário de atendimento: 08:00 às 17:00h',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'HorarioFuncionamentoSedeCDEPVisita' and tipo = 14);

