ALTER TABLE acervo drop COLUMN credito_autor_id;

CREATE TABLE if not exists public.acervo_credito_autor (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE),
	acervo_id int8 NOT NULL,
	credito_autor_id int8 NOT NULL,
	CONSTRAINT acervo_credito_autor_pk PRIMARY KEY (id),
	CONSTRAINT acervo_credito_autor_acervo_fk FOREIGN KEY (acervo_id) REFERENCES public.acervo(id),
	CONSTRAINT acervo_credito_autor_credito_autor_fk FOREIGN KEY (credito_autor_id) REFERENCES public.credito_autor(id)
);