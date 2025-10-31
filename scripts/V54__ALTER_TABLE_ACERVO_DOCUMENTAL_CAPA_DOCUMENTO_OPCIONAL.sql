-- Remove a coluna capa_documento da tabela acervo se existir
ALTER TABLE public.acervo DROP IF EXISTS capa_documento;

-- Adiciona a nova coluna para armazenar o nome do arquivo da imagem da capa
ALTER TABLE public.acervo_documental
ADD COLUMN capa_documento VARCHAR(255) NULL;