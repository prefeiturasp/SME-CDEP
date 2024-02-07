--> Adicionando campo de responsável pelo atendimento
alter table acervo_solicitacao add column if not exists  usuario_responsavel_id int8 null;

--> Adicionando campo de tipo de atendimento
alter table acervo_solicitacao_item add column if not exists  tipo_atendimento int4 null;

--> Adicionando constraint para responsável pelo atendimento
ALTER TABLE acervo_solicitacao DROP CONSTRAINT IF EXISTS acervo_solicitacao_usuario_responsavel_fk;
ALTER TABLE acervo_solicitacao ADD CONSTRAINT acervo_solicitacao_usuario_responsavel_fk FOREIGN KEY (usuario_responsavel_id) REFERENCES public.usuario(id);