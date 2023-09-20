CREATE TABLE if not exists public.acervo_tridimensional (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	acervo_id int8 not null,	
	procedencia varchar(200) NULL,
	data_acervo varchar(50) NULL,	
	conservacao_id int8 NOT NULL,
	quantidade int4 NULL,	
	descricao text NULL,
	largura float NULL,
	altura float NULL,
	profundidade float NULL,
	diametro float NULL,	
	CONSTRAINT acervo_tridimensional_pk PRIMARY KEY (id),
	CONSTRAINT acervo_tridimensional_acervo_fk FOREIGN KEY (acervo_id) REFERENCES public.acervo(id)
);

drop index if exists acervo_tridimensional_acervo_idx;
CREATE INDEX acervo_tridimensional_acervo_idx ON public.acervo_tridimensional USING btree (acervo_id);

CREATE TABLE if not exists public.acervo_tridimensional_arquivo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE),
	acervo_tridimensional_id int8 NOT NULL,
	arquivo_id int8 NOT NULL,
	CONSTRAINT acervo_tridimensional_arquivo_pk PRIMARY KEY (id),
	CONSTRAINT acervo_tridimensional_arquivo_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES public.arquivo(id),
	CONSTRAINT acervo_tridimensional_arquivo_acervo_tridimensional_fk FOREIGN KEY (acervo_tridimensional_id) REFERENCES public.acervo_tridimensional(id)
);

drop index if exists acervo_tridimensional_arquivo_acervo_tridimensional_idx;
CREATE INDEX acervo_tridimensional_arquivo_acervo_tridimensional_idx ON public.acervo_tridimensional_arquivo USING btree (acervo_tridimensional_id);

drop index if exists acervo_tridimensional_arquivo_arquivo_idx;
CREATE INDEX acervo_tridimensional_arquivo_arquivo_idx ON public.acervo_tridimensional_arquivo USING btree (arquivo_id);
