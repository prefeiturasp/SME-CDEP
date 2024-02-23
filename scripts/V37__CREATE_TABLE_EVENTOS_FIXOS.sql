--> Criar tabela de eventos fixos
create table if not exists public.evento_fixo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE),
	data timestamp NOT NULL,
	tipo int4 NOT NULL,
	descricao varchar(200) NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_login varchar(200) NOT NULL,
	alterado_login varchar(200) NULL,
	CONSTRAINT evento_fixo_pk PRIMARY KEY (id)
);

--> Inserindo eventos fixos
insert into evento_fixo (data, tipo, descricao, criado_em, criado_por, criado_login) 
select '2024-01-01'::timestamp,2,'Confraternização universal', now() , 'Sistema','Sistema'  where not exists (select 1 from evento_fixo where descricao = 'Confraternização universal') union all
select '2024-01-25'::timestamp,2,'Aniversário de São Paulo (Municipal)', now(), 'Sistema','Sistema'  where not exists (select 1 from evento_fixo where descricao = 'Aniversário de São Paulo (Municipal)') union all
select '2024-04-21'::timestamp,2,'Tiradentes', now(), 'Sistema','Sistema'  where not exists (select 1 from evento_fixo where descricao = 'Tiradentes') union all
select '2024-07-09'::timestamp,2,'Revolução constitucionalista (Estadual)', now(), 'Sistema','Sistema'  where not exists (select 1 from evento_fixo where descricao = 'Revolução constitucionalista (Estadual)') union all
select '2024-09-07'::timestamp,2,'Independência', now(), 'Sistema','Sistema'  where not exists (select 1 from evento_fixo where descricao = 'Independência') union all
select '2024-12-10'::timestamp,2,'Nossa Senhora Aparecida', now(), 'Sistema','Sistema'  where not exists (select 1 from evento_fixo where descricao = 'Nossa Senhora Aparecida') union all
select '2024-11-02'::timestamp,2,'Finados', now(), 'Sistema','Sistema'  where not exists (select 1 from evento_fixo where descricao = 'Finados') union all
select '2024-11-15'::timestamp,2,'Proclamação da república', now(), 'Sistema','Sistema'  where not exists (select 1 from evento_fixo where descricao = 'Proclamação da república') union all
select '2024-11-20'::timestamp,2,'Consciência Negra (Municipal)', now(), 'Sistema','Sistema'  where not exists (select 1 from evento_fixo where descricao = 'Consciência Negra (Municipal)') union all
select '2024-12-25'::timestamp,2,'Natal', now(), 'Sistema','Sistema'  where not exists (select 1 from evento_fixo where descricao = 'Natal');