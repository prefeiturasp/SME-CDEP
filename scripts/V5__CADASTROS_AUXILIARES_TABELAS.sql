﻿
CREATE TABLE IF NOT EXISTS public.tipo_anexo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	nome varchar(500) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT tipo_anexo_pk PRIMARY KEY (id)
);

insert into public.tipo_anexo (nome) 
select 'CD' where not exists (select 1 from public.tipo_anexo where nome = 'CD') union all
select 'Disquete' where not exists (select 1 from public.tipo_anexo where nome = 'Disquete') union all
select 'DVD' where not exists (select 1 from public.tipo_anexo where nome = 'DVD') union all
select 'Encarte' where not exists (select 1 from public.tipo_anexo where nome = 'Encarte') union all
select 'VHS' where not exists (select 1 from public.tipo_anexo where nome = 'Fitas de vídeo');

CREATE TABLE IF NOT EXISTS public.suporte (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	nome varchar(500) NULL,
	excluido bool NOT NULL DEFAULT false,
	tipo INTEGER null,
	CONSTRAINT suporte_pk PRIMARY KEY (id)
);

insert into public.suporte (nome,tipo) 
select 'Papel',1 where not exists (select 1 from public.suporte where nome = 'Papel' and tipo = 1) union all
select 'Digital',1 where not exists (select 1 from public.suporte where nome = 'Digital' and tipo = 1) union all
select 'Papel/Digital',1 where not exists (select 1 from public.suporte where nome = 'Papel/Digital' and tipo = 1) union all
select 'VHS',2 where not exists (select 1 from public.suporte where nome = 'VHS' and tipo = 2) union all
select 'DVD',2 where not exists (select 1 from public.suporte where nome = 'DVD' and tipo = 2);

CREATE TABLE IF NOT EXISTS public.formato (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	nome varchar(500) NULL,
	tipo INTEGER null,
	excluido bool NOT NULL DEFAULT false,	
	CONSTRAINT formato_pk PRIMARY KEY (id)
);

insert into public.formato (nome,tipo) 
select 'JPEG',1 where not exists (select 1 from public.formato where nome = 'JPEG' and tipo = 1) union all
select 'TIFF',1 where not exists (select 1 from public.formato where nome = 'TIFF' and tipo = 1) union all
select 'VDF',2 where not exists (select 1 from public.formato where nome = 'VDF' and tipo = 2) union all
select 'VOB',2 where not exists (select 1 from public.formato where nome = 'VOB' and tipo = 2);


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
select 'Caderno',1 where not exists (select 1 from public.material where nome = 'Caderno' and tipo = 1) union all
select 'Periódico',1 where not exists (select 1 from public.material where nome = 'Periódico' and tipo = 1) union all
select 'Revista',1 where not exists (select 1 from public.material where nome = 'Revista' and tipo = 1) union all
select 'Livro',2 where not exists (select 1 from public.material where nome = 'Livro' and tipo = 2) union all
select 'Tese',2 where not exists (select 1 from public.material where nome = 'Tese'  and tipo = 2) union all
select 'Periódico',2 where not exists (select 1 from public.material where nome = 'Periódico'  and tipo = 2);

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
select 'Francês' where not exists (select 1 from public.idioma where nome = 'Francês') union all
select 'Alemão' where not exists (select 1 from public.idioma where nome = 'Alemão');

CREATE TABLE IF NOT EXISTS public.cromia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	nome varchar(500) NULL,
	excluido bool NOT NULL DEFAULT false,	
	CONSTRAINT cromia_pk PRIMARY KEY (id)
);

insert into public.cromia (nome) 
select 'Color' where not exists (select 1 from public.cromia where nome = 'Color') union all
select 'PB' where not exists (select 1 from public.cromia where nome = 'PB') ;

CREATE TABLE IF NOT EXISTS public.conservacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	nome varchar(500) NULL,
	excluido bool NOT NULL DEFAULT false,	
	CONSTRAINT conservacao_pk PRIMARY KEY (id)
);

insert into public.conservacao (nome) 
select 'Ótimo' where not exists (select 1 from public.conservacao where nome = 'Ótimo') union all
select 'Bom' where not exists (select 1 from public.conservacao where nome = 'Bom') union all
select 'Regular' where not exists (select 1 from public.conservacao where nome = 'Regular') union all
select 'Ruim' where not exists (select 1 from public.conservacao where nome = 'Ruim');

CREATE TABLE IF NOT EXISTS public.acesso_documento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	nome varchar(500) NULL,
	excluido bool NOT NULL DEFAULT false,	
	CONSTRAINT acesso_documento_pk PRIMARY KEY (id)
);

insert into public.acesso_documento (nome) 
select 'Digital' where not exists (select 1 from public.acesso_documento where nome = 'Digital') union all
select 'Físico' where not exists (select 1 from public.acesso_documento where nome = 'Físico');