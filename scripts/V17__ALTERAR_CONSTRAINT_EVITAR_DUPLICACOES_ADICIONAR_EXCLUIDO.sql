ALTER TABLE public.cromia DROP CONSTRAINT IF EXISTS cromia_un;
ALTER TABLE public.cromia ADD CONSTRAINT cromia_un UNIQUE (nome,excluido);

ALTER TABLE public.assunto DROP CONSTRAINT IF EXISTS assunto_un;
ALTER TABLE public.assunto ADD CONSTRAINT assunto_un UNIQUE (nome,excluido);

ALTER TABLE public.acesso_documento DROP CONSTRAINT IF EXISTS acesso_documento_un;
ALTER TABLE public.acesso_documento ADD CONSTRAINT acesso_documento_un UNIQUE (nome,excluido);

ALTER TABLE public.credito_autor DROP CONSTRAINT IF EXISTS credito_autor_un;
ALTER TABLE public.credito_autor ADD CONSTRAINT credito_autor_un UNIQUE (nome,tipo, excluido);

ALTER TABLE public.conservacao DROP CONSTRAINT IF EXISTS conservacao_un;
ALTER TABLE public.conservacao ADD CONSTRAINT conservacao_un UNIQUE (nome,excluido);

ALTER TABLE public.editora DROP CONSTRAINT IF EXISTS editora_un;
ALTER TABLE public.editora ADD CONSTRAINT editora_un UNIQUE (nome,excluido);

ALTER TABLE public.formato DROP CONSTRAINT IF EXISTS formato_un;
ALTER TABLE public.formato ADD CONSTRAINT formato_un UNIQUE (nome,tipo,excluido);

ALTER TABLE public.idioma DROP CONSTRAINT IF EXISTS idioma_un;
ALTER TABLE public.idioma ADD CONSTRAINT idioma_un UNIQUE (nome,excluido);

ALTER TABLE public.material DROP CONSTRAINT IF EXISTS material_un;
ALTER TABLE public.material ADD CONSTRAINT material_un UNIQUE (nome,tipo,excluido);

ALTER TABLE public.serie_colecao DROP CONSTRAINT IF EXISTS serie_colecao_un;
ALTER TABLE public.serie_colecao ADD CONSTRAINT serie_colecao_un UNIQUE (nome,excluido);

ALTER TABLE public.suporte DROP CONSTRAINT IF EXISTS suporte_un;
ALTER TABLE public.suporte ADD CONSTRAINT suporte_un UNIQUE (nome,tipo,excluido);