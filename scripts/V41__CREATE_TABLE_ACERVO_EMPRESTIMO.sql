--> Criando a tabela de empréstimo de acervos
create table if not exists public.acervo_emprestimo (
	id int8 GENERATED ALWAYS AS IDENTITY( MINVALUE 0 NO MAXVALUE START 0 NO CYCLE) NOT NULL,
	acervo_solicitacao_item_id int8 NOT NULL,	
	dt_emprestimo timestamp NULL,
	dt_devolucao timestamp NULL,
	situacao int2 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_login varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_login varchar(200) NULL,
	excluido bool DEFAULT false null,
	CONSTRAINT acervo_emprestimo_pk PRIMARY KEY (id),
	CONSTRAINT acervo_emprestimo_acervo_solicitacao_item_fk FOREIGN KEY (acervo_solicitacao_item_id) REFERENCES public.acervo_solicitacao_item(id)
);

--> Criando índices
drop index if exists acervo_emprestimo_acervo_solicitacao_item_idx;
CREATE INDEX acervo_emprestimo_acervo_solicitacao_item_idx ON public.acervo_emprestimo (acervo_solicitacao_item_id);

drop index if exists acervo_emprestimo_dt_emprestimo_idx;
CREATE INDEX acervo_emprestimo_dt_emprestimo_idx ON public.acervo_emprestimo (dt_emprestimo);

drop index if exists acervo_emprestimo_dt_devolucao_idx;
CREATE INDEX acervo_emprestimo_dt_devolucao_idx ON public.acervo_emprestimo (dt_devolucao);

--> Adicionando parâmetro de limite de dias para empréstimo de acervo
INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'LimiteDiasEmprestimoAcervo', 
       15,
       'Este parâmetro define a quantidade máxima de dias que um acervo pode ser emprestado antes de ser necessário devolvê-lo',
       '7',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'LimiteDiasEmprestimoAcervo' and tipo = 15);