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

