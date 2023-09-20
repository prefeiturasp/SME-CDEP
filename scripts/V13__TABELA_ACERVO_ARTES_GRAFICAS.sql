CREATE TABLE if not exists public.acervo_arte_grafica (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	acervo_id int8 not null,
	localizacao varchar(100) NULL,
	procedencia varchar(200) NULL,
	data_acervo varchar(50) NULL,
	copia_digital bool NULL,
	permite_uso_imagem bool NULL,
	conservacao_id int8 NOT NULL,
	cromia_id int8 NOT NULL,
	largura float NULL,
	altura float NULL,
	diametro float NULL,
	tecnica varchar(100) NULL,
	suporte_id int8 NOT NULL,
	quantidade int4 NULL,	
	descricao text NULL,
	CONSTRAINT acervo_arte_grafica_pk PRIMARY KEY (id),
	CONSTRAINT acervo_arte_grafica_acervo_fk FOREIGN KEY (acervo_id) REFERENCES public.acervo(id)
);

drop index if exists acervo_arte_grafica_acervo_idx;
CREATE INDEX acervo_arte_grafica_acervo_idx ON public.acervo_arte_grafica USING btree (acervo_id);

CREATE TABLE if not exists public.acervo_arte_grafica_arquivo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE),
	acervo_arte_grafica_id int8 NOT NULL,
	arquivo_id int8 NOT NULL,
	CONSTRAINT acervo_arte_grafica_arquivo_pk PRIMARY KEY (id),
	CONSTRAINT acervo_arte_grafica_arquivo_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES public.arquivo(id),
	CONSTRAINT acervo_arte_grafica_arquivo_acervo_arte_grafica_fk FOREIGN KEY (acervo_arte_grafica_id) REFERENCES public.acervo_arte_grafica(id)
);

drop index if exists acervo_arte_grafica_arquivo_acervo_arte_grafica_idx;
CREATE INDEX acervo_arte_grafica_arquivo_acervo_arte_grafica_idx ON public.acervo_arte_grafica_arquivo USING btree (acervo_arte_grafica_id);

drop index if exists acervo_arte_grafica_arquivo_arquivo_idx;
CREATE INDEX acervo_arte_grafica_arquivo_arquivo_idx ON public.acervo_arte_grafica_arquivo USING btree (arquivo_id);

drop index if exists acervo_fotografico_acervo_idx;
CREATE INDEX acervo_fotografico_acervo_idx ON public.acervo_fotografico USING btree (acervo_id);

drop index if exists acervo_fotografico_arquivo_acervo_fotografico_idx;
CREATE INDEX acervo_fotografico_arquivo_acervo_fotografico_idx ON public.acervo_fotografico_arquivo USING btree (acervo_fotografico_id);

drop index if exists acervo_fotografico_arquivo_arquivo_idx;
CREATE INDEX acervo_fotografico_arquivo_arquivo_idx ON public.acervo_fotografico_arquivo USING btree (arquivo_id);

