--> Alterando o tipo de dado de ANO para string em ACERVO
alter table acervo alter column ano type VARCHAR(7);

--> Adicionando campos de ano inicial e final
alter table acervo add column if not exists ano_inicio int;
alter table acervo add column if not exists ano_fim int;