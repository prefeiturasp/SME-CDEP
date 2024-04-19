--> Adicionando campo e-mail na tabela de usuário
alter table public.usuario add column IF NOT EXISTS email varchar(100) null;