--> Criar tabela de eventos
create table if not exists public.evento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE),
	data timestamp NOT NULL,
	tipo int4 NOT NULL,
	descricao varchar(200) NOT NULL,
	acervo_solicitacao_item_id int8 NULL,
	justificativa varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_login varchar(200) NOT NULL,
	alterado_login varchar(200) NULL,
	CONSTRAINT evento_pk PRIMARY KEY (id),
	CONSTRAINT evento_acervo_solicitacao_item_fk FOREIGN KEY (acervo_solicitacao_item_id) REFERENCES public.acervo_solicitacao_item(id)
);

--> Criando índices
drop index if exists evento_acervo_solicitacao_item_idx;
CREATE INDEX evento_acervo_solicitacao_item_idx ON public.acervo_solicitacao_item USING btree (id);