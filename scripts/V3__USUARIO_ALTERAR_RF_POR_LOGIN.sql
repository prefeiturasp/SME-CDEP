ALTER TABLE usuario DROP COLUMN IF EXISTS criado_rf; 
ALTER TABLE usuario DROP COLUMN IF EXISTS alterado_rf;
ALTER TABLE usuario ADD COLUMN IF NOT EXISTS criado_login varchar(200) NOT NULL; 
ALTER TABLE usuario ADD COLUMN IF NOT EXISTS alterado_login varchar(200) NULL; 