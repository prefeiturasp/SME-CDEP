ALTER TABLE usuario ADD COLUMN IF NOT EXISTS Telefone varchar(15) NULL; 
ALTER TABLE usuario ADD COLUMN IF NOT EXISTS Endereco varchar(200) NULL; 
ALTER TABLE usuario ADD COLUMN IF NOT EXISTS Numero INTEGER NULL; 
ALTER TABLE usuario ADD COLUMN IF NOT EXISTS Complemento  varchar(20) NULL;
ALTER TABLE usuario ADD COLUMN IF NOT EXISTS Cidade  varchar(50) NULL;
ALTER TABLE usuario ADD COLUMN IF NOT EXISTS Estado  varchar(5) NULL;
ALTER TABLE usuario ADD COLUMN IF NOT EXISTS Cep  varchar(10) NULL;
ALTER TABLE usuario ADD COLUMN IF NOT EXISTS Tipo INTEGER NULL; 