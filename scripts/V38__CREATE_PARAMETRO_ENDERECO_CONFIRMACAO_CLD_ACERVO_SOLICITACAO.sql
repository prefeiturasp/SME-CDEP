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