--> importacao_arquivo
CREATE TABLE if not exists public.importacao_arquivo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE),
	nome varchar NOT NULL, 
	tipo_acervo int4 NOT NULL,	
	status int4 not null,
	conteudo text NOT NULL, 
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_login varchar(200) NOT NULL,
	alterado_login varchar(200) NULL,
	CONSTRAINT importacao_arquivo_pk PRIMARY KEY (id)
);
