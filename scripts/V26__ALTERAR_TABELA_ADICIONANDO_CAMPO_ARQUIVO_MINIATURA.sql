--> Adicionando campo miniatura ao Acervo Tridimensional
alter table public.acervo_tridimensional_arquivo add column IF NOT EXISTS arquivo_miniatura_id int8 NULL;

ALTER TABLE public.acervo_tridimensional_arquivo DROP CONSTRAINT IF EXISTS acervo_tridimensional_arquivo_arquivo_miniatura_fk;
ALTER TABLE public.acervo_tridimensional_arquivo ADD CONSTRAINT acervo_tridimensional_arquivo_arquivo_miniatura_fk FOREIGN KEY (arquivo_miniatura_id) REFERENCES public.arquivo(id);

drop index if exists acervo_tridimensional_arquivo_arquivo_miniatura_idx;
CREATE INDEX acervo_tridimensional_arquivo_arquivo_miniatura_idx ON public.acervo_tridimensional_arquivo USING btree (arquivo_miniatura_id);

--> Adicionando campo miniatura ao Acervo Arte Gráfica
alter table public.acervo_arte_grafica_arquivo add column IF NOT EXISTS arquivo_miniatura_id int8 NULL;

ALTER TABLE public.acervo_arte_grafica_arquivo DROP CONSTRAINT IF EXISTS acervo_arte_grafica_arquivo_arquivo_miniatura_fk;
ALTER TABLE public.acervo_arte_grafica_arquivo ADD CONSTRAINT acervo_arte_grafica_arquivo_arquivo_miniatura_fk FOREIGN KEY (arquivo_miniatura_id) REFERENCES public.arquivo(id);

drop index if exists acervo_arte_grafica_arquivo_arquivo_miniatura_idx;
CREATE INDEX acervo_arte_grafica_arquivo_arquivo_miniatura_idx ON public.acervo_arte_grafica_arquivo USING btree (arquivo_miniatura_id);

--> Adicionando campo miniatura ao Acervo Fotográfico
alter table public.acervo_fotografico_arquivo add column IF NOT EXISTS arquivo_miniatura_id int8 NULL;

ALTER TABLE public.acervo_fotografico_arquivo DROP CONSTRAINT IF EXISTS acervo_fotografico_arquivo_arquivo_miniatura_fk;
ALTER TABLE public.acervo_fotografico_arquivo ADD CONSTRAINT acervo_fotografico_arquivo_arquivo_miniatura_fk FOREIGN KEY (arquivo_miniatura_id) REFERENCES public.arquivo(id);

drop index if exists acervo_fotografico_arquivo_arquivo_miniatura_idx;
CREATE INDEX acervo_fotografico_arquivo_arquivo_miniatura_idx ON public.acervo_fotografico_arquivo USING btree (arquivo_miniatura_id);