CREATE TABLE if not exists public.acervo_audiovisual (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	acervo_id int8 not null,	
	localizacao varchar(100) NULL,
	procedencia varchar(200) NULL,
	data_acervo varchar(50) NOT NULL,	
	copia varchar(100) NULL,
	permite_uso_imagem bool NULL,	
	conservacao_id int8 NULL,
	descricao text NOT NULL,
	suporte_id int8 NOT NULL,
	duracao varchar(15) NULL,
	cromia_id int8 NULL,
	tamanho_arquivo varchar(15) NULL,
	acessibilidade varchar(100) NULL,
	disponibilizacao varchar(100) NULL,
	CONSTRAINT acervo_audiovisual_pk PRIMARY KEY (id),
	CONSTRAINT acervo_audiovisual_acervo_fk FOREIGN KEY (acervo_id) REFERENCES public.acervo(id),
	CONSTRAINT acervo_audiovisual_suporte_fk FOREIGN KEY (suporte_id) REFERENCES public.suporte(id)	
);

--> Índices audiovisual
drop index if exists acervo_audiovisual_acervo_idx;
CREATE INDEX acervo_audiovisual_acervo_idx ON public.acervo_audiovisual USING btree (acervo_id);

drop index if exists acervo_audiovisual_suporte_idx;
CREATE INDEX acervo_audiovisual_suporte_idx ON public.acervo_audiovisual USING btree (suporte_id);

drop index if exists acervo_audiovisual_cromia_idx;
CREATE INDEX acervo_audiovisual_cromia_idx ON public.acervo_audiovisual USING btree (cromia_id);

drop index if exists acervo_audiovisual_conservacao_idx;
CREATE INDEX acervo_audiovisual_conservacao_idx ON public.acervo_audiovisual USING btree (conservacao_id);

--> Constraints e índices do acervo tridimensional
ALTER TABLE acervo_tridimensional DROP CONSTRAINT IF EXISTS acervo_tridimensional_conservacao_fk;
alter table acervo_tridimensional add constraint acervo_tridimensional_conservacao_fk FOREIGN KEY (conservacao_id) REFERENCES conservacao(id);

drop index if exists acervo_tridimensional_conservacao_idx;
CREATE INDEX acervo_tridimensional_conservacao_idx ON public.acervo_tridimensional USING btree (conservacao_id);

--> Constraints e índices do acervo artes gráficas
ALTER TABLE acervo_arte_grafica DROP CONSTRAINT IF EXISTS acervo_arte_grafica_conservacao_fk;
alter table acervo_arte_grafica add constraint acervo_arte_grafica_conservacao_fk FOREIGN KEY (conservacao_id) REFERENCES conservacao(id);

ALTER TABLE acervo_arte_grafica DROP CONSTRAINT IF EXISTS acervo_arte_grafica_cromia_fk;
alter table acervo_arte_grafica add constraint acervo_arte_grafica_cromia_fk FOREIGN KEY (cromia_id) REFERENCES cromia(id);

ALTER TABLE acervo_arte_grafica DROP CONSTRAINT IF EXISTS acervo_arte_grafica_suporte_fk;
alter table acervo_arte_grafica add constraint acervo_arte_grafica_suporte_fk FOREIGN KEY (suporte_id) REFERENCES suporte(id);

drop index if exists acervo_arte_grafica_suporte_idx;
CREATE INDEX acervo_arte_grafica_suporte_idx ON public.acervo_arte_grafica USING btree (suporte_id);

drop index if exists acervo_arte_grafica_cromia_idx;
CREATE INDEX acervo_arte_grafica_cromia_idx ON public.acervo_arte_grafica USING btree (cromia_id);

drop index if exists acervo_arte_grafica_conservacao_idx;
CREATE INDEX acervo_arte_grafica_conservacao_idx ON public.acervo_arte_grafica USING btree (conservacao_id);

--> Constraints e índices do acervo artes fotográfico
ALTER TABLE acervo_fotografico DROP CONSTRAINT IF EXISTS acervo_fotografico_conservacao_fk;
alter table acervo_fotografico add constraint acervo_fotografico_conservacao_fk FOREIGN KEY (conservacao_id) REFERENCES conservacao(id);

ALTER TABLE acervo_fotografico DROP CONSTRAINT IF EXISTS acervo_fotografico_cromia_fk;
alter table acervo_fotografico add constraint acervo_fotografico_cromia_fk FOREIGN KEY (cromia_id) REFERENCES cromia(id);

ALTER TABLE acervo_fotografico DROP CONSTRAINT IF EXISTS acervo_fotografico_suporte_fk;
alter table acervo_fotografico add constraint acervo_fotografico_suporte_fk FOREIGN KEY (suporte_id) REFERENCES suporte(id);

ALTER TABLE acervo_fotografico DROP CONSTRAINT IF EXISTS acervo_fotografico_formato_fk;
alter table acervo_fotografico add constraint acervo_fotografico_formato_fk FOREIGN KEY (formato_id) REFERENCES formato(id);

drop index if exists acervo_fotografico_suporte_idx;
CREATE INDEX acervo_fotografico_suporte_idx ON public.acervo_fotografico USING btree (suporte_id);

drop index if exists acervo_fotografico_cromia_idx;
CREATE INDEX acervo_fotografico_cromia_idx ON public.acervo_fotografico USING btree (cromia_id);

drop index if exists acervo_fotografico_conservacao_idx;
CREATE INDEX acervo_fotografico_conservacao_idx ON public.acervo_fotografico USING btree (conservacao_id);

drop index if exists acervo_fotografico_conservacao_idx;
CREATE INDEX acervo_fotografico_conservacao_idx ON public.acervo_fotografico USING btree (formato_id);

drop index if exists acervo_fotografico_arquivo_acervo_fotografico_idx;
CREATE INDEX acervo_fotografico_arquivo_acervo_fotografico_idx ON public.acervo_fotografico_arquivo USING btree (acervo_fotografico_id);

--> Constraints e índices do acervo artes gráfica
drop index if exists acervo_arte_grafica_arquivo_acervo_arte_grafica_idx;
CREATE INDEX acervo_arte_grafica_arquivo_acervo_arte_grafica_idx ON public.acervo_arte_grafica_arquivo USING btree (acervo_arte_grafica_id);

drop index if exists acervo_credito_autor_acervo_idx;
CREATE INDEX acervo_credito_autor_acervo_idx ON public.acervo_credito_autor USING btree (acervo_id);

drop index if exists acervo_credito_autor_credito_autor_idx;
CREATE INDEX acervo_credito_autor_credito_autor_idx ON public.acervo_credito_autor USING btree (credito_autor_id);