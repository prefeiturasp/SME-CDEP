--> Ajustando campo tipo na tabela de autor
alter table acervo_credito_autor add column IF NOT EXISTS ehCoAutor bool default(false);