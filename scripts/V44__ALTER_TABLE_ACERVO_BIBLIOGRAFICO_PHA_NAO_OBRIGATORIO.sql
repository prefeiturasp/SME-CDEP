--> Removendo a obrigatoriedade do campo 'localizacao_pha'
ALTER TABLE acervo_bibliografico ALTER COLUMN localizacao_pha drop NOT NULL;