--> Tabela Suporte tipo 1 incluir negativo e excluir papel/digital
update suporte set nome = 'Negativo' where nome = 'Papel/Digital';

--> Tabela formato remover as opções VDF e VOB
DROP TABLE IF EXISTS public.formato;
CREATE TABLE IF NOT EXISTS public.formato (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	nome varchar(500) NULL,
	tipo INTEGER null,
	excluido bool NOT NULL DEFAULT false,	
	CONSTRAINT formato_pk PRIMARY KEY (id)
);

insert into public.formato (nome,tipo) 
select 'JPEG',1 where not exists (select 1 from public.formato where nome = 'JPEG' and tipo = 1) union all
select 'TIFF',1 where not exists (select 1 from public.formato where nome = 'TIFF' and tipo = 1);

 
--> Tabela material tipo 1 remover os tipos caderno e revistas 
DROP TABLE IF EXISTS public.material;
CREATE TABLE IF NOT EXISTS public.material (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	nome varchar(500) NULL,
	excluido bool NOT NULL DEFAULT false,
	tipo INTEGER null,
	CONSTRAINT material_pk PRIMARY KEY (id)
);

insert into public.material (nome,tipo) 
select 'Apostila',1 where not exists (select 1 from public.material where nome = 'Apostila' and tipo = 1) union all
select 'Livro',1 where not exists (select 1 from public.material where nome = 'Livro' and tipo = 1) union all
select 'Periódico',1 where not exists (select 1 from public.material where nome = 'Periódico' and tipo = 1) union all
select 'Livro',2 where not exists (select 1 from public.material where nome = 'Livro' and tipo = 2) union all
select 'Tese',2 where not exists (select 1 from public.material where nome = 'Tese'  and tipo = 2) union all
select 'Periódico',2 where not exists (select 1 from public.material where nome = 'Periódico'  and tipo = 2);

--> Tabela idioma remover alemão 
DROP TABLE IF EXISTS public.idioma;
CREATE TABLE IF NOT EXISTS public.idioma (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	nome varchar(500) NULL,
	excluido bool NOT NULL DEFAULT false,	
	CONSTRAINT idioma_pk PRIMARY KEY (id)
);

insert into public.idioma (nome) 
select 'Português' where not exists (select 1 from public.idioma where nome = 'Português') union all
select 'Inglês' where not exists (select 1 from public.idioma where nome = 'Inglês') union all
select 'Espanhol' where not exists (select 1 from public.idioma where nome = 'Espanhol') union all
select 'Francês' where not exists (select 1 from public.idioma where nome = 'Francês');

--> Remover a tabela tipo de anexo
DROP TABLE IF EXISTS public.tipo_anexo; 
