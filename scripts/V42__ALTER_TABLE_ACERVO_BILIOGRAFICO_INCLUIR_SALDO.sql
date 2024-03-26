--> Adicionando campo 'situacao_saldo' para controlar saldo
alter table public.acervo_bibliografico add column IF NOT EXISTS situacao_saldo int2 null default 1;

--> Adicionar campo 'excluido' nos parâmetros
alter table parametro_sistema add column if not exists excluido bool default false;

--> renomeando nome da coluna
ALTER TABLE parametro_sistema RENAME COLUMN alterado_rf TO alterado_login;