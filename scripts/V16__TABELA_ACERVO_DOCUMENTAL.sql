--> acervo_documental
CREATE TABLE if not exists public.acervo_documental (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	acervo_id int8 not null,	
	material_id int8 null,	
	idioma_id int8 not null,	
	ano varchar(15) NULL,	
	numero_pagina varchar(4) NOT NULL,
	volume varchar(15) NULL,
	tipo_anexo varchar(50) NULL,
	largura float NULL,
	altura float NULL,
	tamanho_arquivo varchar(15) NULL,
	localizacao varchar(100) NULL,
	digitalizado bool NOT NULL DEFAULT false,
	conservacao_id int8 null,		
	CONSTRAINT acervo_documental_pk PRIMARY KEY (id),
	CONSTRAINT acervo_documental_acervo_fk FOREIGN KEY (acervo_id) REFERENCES public.acervo(id),
	CONSTRAINT acervo_documental_idioma_fk FOREIGN KEY (idioma_id) REFERENCES public.idioma(id)
);

drop index if exists acervo_documental_acervo_idx;
CREATE INDEX acervo_documental_acervo_idx ON public.acervo_documental USING btree (acervo_id);

drop index if exists acervo_documental_idioma_idx;
CREATE INDEX acervo_documental_idioma_idx ON public.acervo_documental USING btree (idioma_id);


--> acervo_documental_acesso_documento
CREATE TABLE if not exists public.acervo_documental_acesso_documento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	acervo_documental_id int8 not null,	
	acesso_documento_id int8 not null,	
	CONSTRAINT acervo_documental_acesso_documento_pk PRIMARY KEY (id),
	CONSTRAINT acervo_documental_acesso_documento_acervo_fk FOREIGN KEY (acesso_documento_id) REFERENCES public.acesso_documento(id),
	CONSTRAINT acervo_documental_acesso_documento_documental_fk FOREIGN KEY (acervo_documental_id) REFERENCES public.acervo_documental(id)
);

drop index if exists acervo_documental_acesso_documento_documental_idx;
CREATE INDEX acervo_documental_acesso_documento_documental_idx ON public.acervo_documental_acesso_documento USING btree (acervo_documental_id);

drop index if exists acervo_documental_acesso_documento_acesso_documento_idx;
CREATE INDEX acervo_documental_acesso_documento_acesso_documento_idx ON public.acervo_documental_acesso_documento USING btree (acesso_documento_id);


--> acervo_documental_arquivo
CREATE TABLE if not exists public.acervo_documental_arquivo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE),
	acervo_documental_id int8 NOT NULL,
	arquivo_id int8 NOT NULL,
	CONSTRAINT acervo_documental_arquivo_pk PRIMARY KEY (id),
	CONSTRAINT acervo_documental_arquivo_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES public.arquivo(id),
	CONSTRAINT acervo_documental_arquivo_acervo_documental_fk FOREIGN KEY (acervo_documental_id) REFERENCES public.acervo_documental(id)
);

drop index if exists acervo_documental_arquivo_acervo_documental_idx;
CREATE INDEX acervo_documental_arquivo_acervo_documental_idx ON public.acervo_documental_arquivo USING btree (acervo_documental_id);

drop index if exists acervo_documental_arquivo_arquivo_idx;
CREATE INDEX acervo_documental_arquivo_arquivo_idx ON public.acervo_documental_arquivo USING btree (arquivo_id);

--> adicionando 'codigo_novo' no acervo
ALTER TABLE acervo ADD COLUMN IF NOT EXISTS codigo_novo varchar(50) NULL; 