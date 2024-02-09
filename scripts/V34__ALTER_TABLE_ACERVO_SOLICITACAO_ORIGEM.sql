--> Adicionando campo de origem do atendimento
alter table acervo_solicitacao add column if not exists origem int8 default 1;

--> Adicionando campo de data_solicitacao do atendimento
alter table acervo_solicitacao add column if not exists data_solicitacao timestamp null;

--> Preenchendo o campo data da solicitação com a informação de data da criação
update acervo_solicitacao set data_solicitacao = criado_em where data_solicitacao is null;
