--> Removendo as constraints
alter table credito_autor drop constraint if exists credito_autor_un_login;
alter table credito_autor drop constraint if exists credito_autor_un;