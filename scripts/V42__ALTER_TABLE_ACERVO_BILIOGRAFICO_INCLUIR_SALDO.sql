--> Adicionando campo 'situacao_saldo' para controlar saldo
alter table public.acervo_bibliografico add column IF NOT EXISTS situacao_saldo int2 null default 1;