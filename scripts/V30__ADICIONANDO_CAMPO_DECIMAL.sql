--> Alterando os tipos de dados para string ao invés de double
alter table acervo_fotografico alter column largura type varchar(6);
alter table acervo_fotografico alter column altura type varchar(6);

alter table acervo_arte_grafica alter column largura type varchar(6);
alter table acervo_arte_grafica alter column altura type varchar(6);
alter table acervo_arte_grafica alter column diametro type varchar(6);

alter table acervo_documental alter column largura type varchar(6);
alter table acervo_documental alter column altura type varchar(6);

alter table acervo_bibliografico alter column largura type varchar(6);
alter table acervo_bibliografico alter column altura type varchar(6);

alter table acervo_tridimensional alter column largura type varchar(6);
alter table acervo_tridimensional alter column altura type varchar(6);
alter table acervo_tridimensional alter column profundidade type varchar(6);
alter table acervo_tridimensional alter column diametro type varchar(6);