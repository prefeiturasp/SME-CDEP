--> Mudança da estrutura de dados do campo número de inteiro para string
ALTER TABLE usuario
ALTER COLUMN numero TYPE varchar(20);