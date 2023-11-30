--> Ajustando campo opcional
ALTER TABLE acervo_documental ALTER COLUMN copia_digital DROP NOT NULL;
ALTER TABLE acervo_documental ALTER COLUMN copia_digital DROP DEFAULT;