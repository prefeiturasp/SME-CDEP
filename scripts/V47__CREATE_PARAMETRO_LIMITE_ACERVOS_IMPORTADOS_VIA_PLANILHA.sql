--> Limite de acervo importados via planilha
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'LimiteAcervosImportadosViaPanilha', 
       22,
       'Limite de acervos importados via planilha',
       '1000',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'LimiteAcervosImportadosViaPanilha' and tipo = 22);