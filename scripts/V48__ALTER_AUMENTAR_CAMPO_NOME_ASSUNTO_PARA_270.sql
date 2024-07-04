--> Alterar tamanho do campo nome
ALTER TABLE public.assunto ALTER COLUMN nome TYPE varchar(270);

--> Alterar tamanho do campo tipo autoria
ALTER TABLE public.acervo_credito_autor ALTER COLUMN tipo_autoria TYPE varchar(30);

--> Alterar tamanho do campo 'nome' de crédito/autor
ALTER TABLE public.credito_autor ALTER COLUMN nome TYPE varchar(330);