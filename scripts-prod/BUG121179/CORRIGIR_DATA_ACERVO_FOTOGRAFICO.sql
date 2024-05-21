--> Corrigir as datas de acervos 
update acervo
set data_acervo = TO_CHAR(TO_TIMESTAMP(data_acervo, 'MM/DD/YYYY HH24:MI:SS'), 'DD/MM/YYYY HH24:MI:SS')
from acervo 
where tipo = 5 and length(data_acervo ) > 7
