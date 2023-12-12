--> Adicionando campo Descrição ao Acervo
alter table public.acervo add column IF NOT EXISTS data_acervo varchar(50) NULL;
alter table public.acervo add column IF NOT EXISTS ano int4 NULL;

--> Transferir valores das tabelas auxiliares para Acervo
UPDATE acervo
SET data_acervo = acervo_arte_grafica.data_acervo
FROM acervo_arte_grafica
WHERE acervo.id = acervo_arte_grafica.acervo_id;

UPDATE acervo
SET data_acervo = acervo_audiovisual.data_acervo 
FROM acervo_audiovisual
WHERE acervo.id = acervo_audiovisual.acervo_id;

UPDATE acervo
SET data_acervo = acervo_fotografico.data_acervo 
FROM acervo_fotografico
WHERE acervo.id = acervo_fotografico.acervo_id;

UPDATE acervo
SET data_acervo = acervo_tridimensional.data_acervo 
FROM acervo_tridimensional
WHERE acervo.id = acervo_tridimensional.acervo_id;

--> Deletar campo descrição das tabelas auxiliares de acervo
alter table acervo_arte_grafica drop column if exists data_acervo;
alter table acervo_audiovisual drop column if exists data_acervo;
alter table acervo_fotografico drop column if exists data_acervo;
alter table acervo_tridimensional drop column if exists data_acervo;
alter table acervo_documental drop column if exists ano;
alter table acervo_bibliografico drop column if exists ano;