--> Adicionando campo Descrição ao Acervo
alter table public.acervo add column IF NOT EXISTS descricao text null;

--> Transferir valores das tabelas auxiliares para Acervo
UPDATE acervo
SET descricao = acervo_fotografico.descricao
FROM acervo_fotografico
WHERE acervo.id = acervo_fotografico.acervo_id;

UPDATE acervo
SET descricao = acervo_arte_grafica.descricao
FROM acervo_arte_grafica
WHERE acervo.id = acervo_arte_grafica.acervo_id;

UPDATE acervo
SET descricao = acervo_audiovisual.descricao
FROM acervo_audiovisual
WHERE acervo.id = acervo_audiovisual.acervo_id;

UPDATE acervo
SET descricao = acervo_documental.descricao
FROM acervo_documental
WHERE acervo.id = acervo_documental.acervo_id;

UPDATE acervo
SET descricao = acervo_tridimensional.descricao
FROM acervo_tridimensional
WHERE acervo.id = acervo_tridimensional.acervo_id;

--> Deletar campo descrição das tabelas auxiliares de acervo
alter table acervo_fotografico drop column if exists descricao;
alter table acervo_arte_grafica drop column if exists descricao;
alter table acervo_audiovisual drop column if exists descricao;
alter table acervo_documental drop column if exists descricao;
alter table acervo_tridimensional drop column if exists descricao;

