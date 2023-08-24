CREATE TABLE if not exists public.acervo_fotografico (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	titulo varchar(200) NULL,
	credito_id int8 NOT NULL,
	tombo int8 NULL,
	localizacao varchar(200) NULL,
	procedencia varchar(200) NULL,
	data_acervo timestamp NULL,
	copia_digital bool NULL,
	permite_uso_imagem bool NULL,
	conservacao_id int8 NOT NULL,
	descricao text NULL,
	quantidade int4 NULL,
	dimensao_largura varchar(20) NULL,
	dimensao_altura varchar(20) NULL,
	suporte_id int8 NULL,
	formato_id int8 NULL,
	cromia_id int8 NULL,
	resolucao varchar(50) NULL,
	tamanho_arquivo varchar(50) NULL,
	excluido bool NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_login varchar(200) NOT NULL,
	alterado_login varchar(200) NULL,
	CONSTRAINT acervo_fotografico_pk PRIMARY KEY (id)		
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
