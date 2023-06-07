CREATE TABLE public.usuario (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	rf_codigo varchar(12) NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	login varchar(50) NULL,
	ultimo_login timestamp NULL,
	nome varchar(100) NULL,
	expiracao_recuperacao_senha timestamp NULL,
	token_recuperacao_senha uuid NULL,
	CONSTRAINT usuario_pk PRIMARY KEY (id),
	CONSTRAINT usuario_un_login UNIQUE (login)
);
CREATE INDEX usuario_codigo_rf_idx ON public.usuario (rf_codigo);
CREATE INDEX usuario_login_idx ON public.usuario (login);