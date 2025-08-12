-- Adiciona a nova coluna para armazenar o nome do arquivo da imagem da capa
ALTER TABLE public.acervo_documental
ADD COLUMN capa_documento VARCHAR(255) NULL;