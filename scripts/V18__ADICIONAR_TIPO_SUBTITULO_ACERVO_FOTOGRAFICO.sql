--> Adicionando tipo na tabela Crédito Autor
alter table public.acervo_credito_autor add column IF NOT EXISTS tipo integer not null default 1;
alter table public.acervo_credito_autor add column IF NOT EXISTS tipo_autoria varchar(15) null;

--> Adicionando subtitulo na tabela acervo
alter table public.acervo add column IF NOT EXISTS subtitulo varchar(500) NULL;

--> Acervo Fotográfico
CREATE TABLE if not exists public.acervo_bibliografico (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	acervo_id int8 not null,
	material_id int8 NOT NULL,
	editora_id int8 NULL,
	ano varchar(15) not NULL,
	edicao varchar(15) NULL,
	numero_pagina float NULL,
	largura float NULL,
	altura float NULL,
	serie_colecao_id int8 NULL,
	volume varchar(15) NULL,
	idioma_id int8 not NULL,
	localizacao_cdd varchar(50) not NULL,
	localizacao_pha varchar(50) not NULL,
	notas_gerais varchar(500) null,
	isbn varchar(50) null,
	CONSTRAINT acervo_bibliografico_pk PRIMARY KEY (id),
	CONSTRAINT acervo_bibliografico_acervo_fk FOREIGN KEY (acervo_id) REFERENCES public.acervo(id),
	CONSTRAINT acervo_bibliografico_material_fk FOREIGN KEY (material_id) REFERENCES public.material(id),
	CONSTRAINT acervo_bibliografico_editora_fk FOREIGN KEY (editora_id) REFERENCES public.editora(id),
	CONSTRAINT acervo_bibliografico_serie_colecao_fk FOREIGN KEY (serie_colecao_id) REFERENCES public.serie_colecao(id),
	CONSTRAINT acervo_bibliografico_serie_idioma_fk FOREIGN KEY (idioma_id) REFERENCES public.idioma(id)
);

drop index if exists acervo_bibliografico_acervo_idx;
CREATE INDEX acervo_bibliografico_acervo_idx ON public.acervo_bibliografico USING btree (acervo_id);


CREATE TABLE if not exists public.acervo_bibliografico_assunto (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE),
	acervo_id int8 NOT NULL,
	assunto_id int8 NOT NULL,
	CONSTRAINT acervo_bibliografico_assunto_pk PRIMARY KEY (id),
	CONSTRAINT acervo_bibliografico_assunto_acervo_fk FOREIGN KEY (acervo_id) REFERENCES public.acervo(id),
	CONSTRAINT acervo_bibliografico_assunto_assunto_fk FOREIGN KEY (assunto_id) REFERENCES public.assunto(id)
);
drop index if exists acervo_bibliografico_assunto_acervo_idx;
CREATE INDEX acervo_bibliografico_assunto_acervo_idx ON public.acervo_bibliografico_assunto USING btree (acervo_id);

drop index if exists acervo_bibliografico_assunto_assunto_idx;
CREATE INDEX acervo_bibliografico_assunto_assunto_idx ON public.acervo_bibliografico_assunto USING btree (assunto_id);