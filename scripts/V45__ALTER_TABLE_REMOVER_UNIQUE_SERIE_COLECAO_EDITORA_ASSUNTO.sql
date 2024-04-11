--> Removendo a RESTRIÇÃO DE UNIQUE duplicada
ALTER TABLE serie_colecao  
  DROP CONSTRAINT IF EXISTS serie_colecao_un_login;

ALTER TABLE serie_colecao  
  DROP CONSTRAINT IF EXISTS serie_colecao_un;    
 
ALTER TABLE editora  
  DROP CONSTRAINT IF EXISTS editora_un_login;
  
ALTER TABLE editora  
  DROP CONSTRAINT IF EXISTS editora_un;
 
ALTER TABLE assunto  
  DROP CONSTRAINT IF EXISTS assunto_un_login;
  
ALTER TABLE assunto  
  DROP CONSTRAINT IF EXISTS assunto_un;