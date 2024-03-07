--> Inclusão do campo  da estrutura de dados do campo número de inteiro para string
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 
        FROM information_schema.columns 
        WHERE table_name = 'acervo_solicitacao_item' 
        AND column_name = 'usuario_responsavel_id'
    ) THEN
        
		--> Adicionar campo
		ALTER TABLE acervo_solicitacao_item 
        ADD usuario_responsavel_id INT8 NULL;

        --> Adicionar constraint
		ALTER TABLE acervo_solicitacao_item 
        ADD CONSTRAINT acervo_solicitacao_item_usuario_responsavel_fk
        FOREIGN KEY (usuario_responsavel_id) 
        REFERENCES public.usuario(id);
       
       --> Tansferir os dados para tabela de itens
	   UPDATE acervo_solicitacao_item asi
       SET usuario_responsavel_id = aso.usuario_responsavel_id
       FROM acervo_solicitacao aso
       WHERE aso.id = asi.acervo_solicitacao_id;
      
      --> Remover responsável de acervo_solicitacao
	  IF EXISTS (
        SELECT 1 
        FROM information_schema.columns 
        WHERE table_name = 'acervo_solicitacao' 
        AND column_name = 'usuario_responsavel_id'
      ) THEN
        ALTER TABLE acervo_solicitacao 
        DROP column usuario_responsavel_id;
       END IF;
      
    END IF;
END $$;