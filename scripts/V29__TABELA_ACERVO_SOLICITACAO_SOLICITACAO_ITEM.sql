
--> acervo_solicitacao
CREATE TABLE if not exists public.acervo_solicitacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	usuario_id int8 not null,	
	situacao int2 NOT NULL,	
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_login varchar(200) NOT NULL,	
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_login varchar(200) null,	
	excluido bool NULL DEFAULT false,
	CONSTRAINT acervo_solicitacao_pk PRIMARY KEY (id),
	CONSTRAINT acervo_solicitacao_usuario_fk FOREIGN KEY (usuario_id) REFERENCES public.usuario(id)
);

drop index if exists acervo_solicitacao_usuario_idx;
CREATE INDEX acervo_solicitacao_usuario_idx ON public.acervo_solicitacao USING btree (usuario_id);

--> acervo_solicitacao_item
CREATE TABLE if not exists public.acervo_solicitacao_item (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( NO MINVALUE NO MAXVALUE NO CYCLE),
	acervo_solicitacao_id int8 not null,		
	acervo_id int8 not null,
	situacao int2 NOT NULL,		
	dt_visita timestamp NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_login varchar(200) NOT NULL,	
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_login varchar(200) null,
	excluido bool NULL DEFAULT false,
	CONSTRAINT acervo_solicitacao_item_pk PRIMARY KEY (id),
	CONSTRAINT acervo_solicitacao_item_solicitacao_fk FOREIGN KEY (acervo_solicitacao_id) REFERENCES public.acervo_solicitacao(id),
	CONSTRAINT acervo_solicitacao_item_acervo_fk FOREIGN KEY (acervo_id) REFERENCES public.acervo(id)
);

drop index if exists acervo_solicitacao_item_solicitacao_idx;
CREATE INDEX acervo_solicitacao_item_solicitacao_idx ON public.acervo_solicitacao_item USING btree (acervo_solicitacao_id);

drop index if exists acervo_solicitacao_item_acervo_idx;
CREATE INDEX acervo_solicitacao_item_acervo_idx ON public.acervo_solicitacao_item USING btree (acervo_id);