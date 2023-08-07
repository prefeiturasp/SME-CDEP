--> Crédito
CREATE TABLE if not exists public.credito (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	nome varchar(200) NULL,
	excluido bool NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_login varchar(200) NOT NULL,
	alterado_login varchar(200) NULL,
	CONSTRAINT credito_pk PRIMARY KEY (id),
	CONSTRAINT credito_un_login UNIQUE (nome)
);
drop index if exists credito_nome_idx;
CREATE INDEX credito_nome_idx ON public.credito (nome);

--> Autor
CREATE TABLE if not exists public.autor (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	nome varchar(200) NULL,
	excluido bool NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_login varchar(200) NOT NULL,
	alterado_login varchar(200) NULL,
	CONSTRAINT autor_pk PRIMARY KEY (id),
	CONSTRAINT autor_un_login UNIQUE (nome)
);
drop index if exists autor_nome_idx;
CREATE INDEX autor_nome_idx ON public.autor (nome);

--> Editora
CREATE TABLE if not exists public.editora (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	nome varchar(200) NULL,
	excluido bool NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_login varchar(200) NOT NULL,
	alterado_login varchar(200) NULL,
	CONSTRAINT editora_pk PRIMARY KEY (id),
	CONSTRAINT editora_un_login UNIQUE (nome)
);
drop index if exists editora_nome_idx;
CREATE INDEX editora_nome_idx ON public.editora (nome);

--> Assunto
CREATE TABLE if not exists public.assunto (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	nome varchar(200) NULL,
	excluido bool NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_login varchar(200) NOT NULL,
	alterado_login varchar(200) NULL,
	CONSTRAINT assunto_pk PRIMARY KEY (id),
	CONSTRAINT assunto_un_login UNIQUE (nome)
);
drop index if exists assunto_nome_idx;
CREATE INDEX assunto_nome_idx ON public.assunto (nome);

--> Série e Coleção
CREATE TABLE if not exists public.serie_colecao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	nome varchar(200) NULL,
	excluido bool NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_login varchar(200) NOT NULL,
	alterado_login varchar(200) NULL,
	CONSTRAINT serie_colecao_pk PRIMARY KEY (id),
	CONSTRAINT serie_colecao_un_login UNIQUE (nome)
);
drop index if exists serie_colecao_nome_idx;
CREATE INDEX serie_colecao_nome_idx ON public.serie_colecao (nome);

