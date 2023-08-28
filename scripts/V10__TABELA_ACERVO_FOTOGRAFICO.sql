drop table if exists public.credito;
drop table if exists public.autor;

--> CréditoAutor
CREATE TABLE if not exists public.credito_autor (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	nome varchar(200) NULL,
	tipo INTEGER null, 
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_login varchar(200) NOT NULL,
	alterado_login varchar(200) NULL,
	CONSTRAINT credito_autor_pk PRIMARY KEY (id),
	CONSTRAINT credito_autor_un_login UNIQUE (nome,tipo)
);
drop index if exists credito_autor_nome_tipo_idx;
CREATE INDEX credito_autor_nome_tipo_idx ON public.credito_autor (nome,tipo);


CREATE TABLE if not exists public.acervo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	tipo int4 not NULL,
	titulo varchar(500) not NULL,
	credito_autor_id int8 NOT NULL,
	codigo varchar(15) null,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_login varchar(200) NOT NULL,
	alterado_login varchar(200) NULL,
	CONSTRAINT acervo_pk PRIMARY KEY (id),
	CONSTRAINT acervo_credito_autor_fk FOREIGN KEY (credito_autor_id) REFERENCES public.credito_autor(id)
);

CREATE TABLE if not exists public.acervo_fotografico (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	acervo_id int8 not null,
	localizacao varchar(100) NULL,
	procedencia varchar(200) NULL,
	data_acervo varchar(50) NULL,
	copia_digital bool NULL,
	permite_uso_imagem bool NULL,
	conservacao_id int8 NOT NULL,
	descricao text NULL,
	quantidade int4 NULL,
	largura float NULL,
	altura float NULL,
	suporte_id int8 NOT NULL,
	formato_id int8 NOT NULL,
	cromia_id int8 NOT NULL,
	resolucao varchar(15) NULL,
	tamanho_arquivo varchar(15) NULL,
	CONSTRAINT acervo_fotografico_pk PRIMARY KEY (id),
	CONSTRAINT acervo_fotografico_acervo_fk FOREIGN KEY (acervo_id) REFERENCES public.acervo(id)
);

CREATE TABLE if not exists public.arquivo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE),
	nome varchar NOT NULL,
	codigo uuid NOT NULL,
	tipo int4 NOT NULL,
	tipo_conteudo varchar NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_login varchar(200) NOT NULL,
	alterado_login varchar(200) NULL,
	CONSTRAINT arquivo_pk PRIMARY KEY (id)
);

CREATE TABLE if not exists public.acervo_fotografico_arquivo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE),
	acervo_fotografico_id int8 NOT NULL,
	arquivo_id int8 NOT NULL,
	CONSTRAINT acervo_fotografico_arquivo_pk PRIMARY KEY (id),
	CONSTRAINT acervo_fotografico_arquivo_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES public.arquivo(id),
	CONSTRAINT acervo_fotografico_arquivo_acervo_fotografico_fk FOREIGN KEY (acervo_fotografico_id) REFERENCES public.acervo_fotografico(id)
);