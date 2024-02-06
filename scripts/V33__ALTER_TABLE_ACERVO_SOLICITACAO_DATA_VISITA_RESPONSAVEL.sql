--> Adicionando campo de responsável pelo atendimento
alter table acervo_solicitacao add column if not exists  usuario_responsavel_id int8 null;