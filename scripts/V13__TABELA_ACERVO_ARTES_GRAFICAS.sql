CREATE TABLE if not exists public.acervo_artes_graficas (
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
	CONSTRAINT acervo_artes_graficas_pk PRIMARY KEY (id),
	CONSTRAINT acervo_artes_graficas_acervo_fk FOREIGN KEY (acervo_id) REFERENCES public.acervo(id)
);

CREATE TABLE if not exists public.acervo_artes_graficas_arquivo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE),
	acervo_artes_graficas_id int8 NOT NULL,
	arquivo_id int8 NOT NULL,
	CONSTRAINT acervo_artes_graficas_arquivo_pk PRIMARY KEY (id),
	CONSTRAINT acervo_artes_graficas_arquivo_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES public.arquivo(id),
	CONSTRAINT acervo_artes_graficas_arquivo_acervo_artes_graficas_fk FOREIGN KEY (acervo_artes_graficas_id) REFERENCES public.acervo_artes_graficas(id)
);