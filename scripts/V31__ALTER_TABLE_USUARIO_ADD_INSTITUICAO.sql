--> Adicionando o campo instituicao na tabela de usuários
alter table USUARIO add column if not exists INSTITUICAO VARCHAR(100) null;