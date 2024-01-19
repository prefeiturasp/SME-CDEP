create table if not exists public.parametro_sistema (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE),
	nome varchar(50) NOT NULL,
	tipo int4 NOT NULL,
	descricao varchar(200) NOT NULL,
	valor text NOT NULL,
	ano int4 NULL,
	ativo bool NOT NULL DEFAULT true,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_login varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT parametro_sistema_pk PRIMARY KEY (id)
);
CREATE INDEX parametro_sistema_ano_idx ON public.parametro_sistema USING btree (ano);
CREATE INDEX parametro_sistema_tipo_idx ON public.parametro_sistema USING btree (tipo);

INSERT INTO public.parametro_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_login)
select 'TermoCompromissoPesquisador', 
       1,
       'TERMO DE COMPROMISSO DO PESQUISADOR CDEP',
       '<span style="box-sizing:border-box; display:inline !important"><p align="center" style="margin:0cm; font-size:11pt; font-family:Calibri, sans-serif; margin:0cm; text-align:center"><b><span  style="font-size:10.5pt; font-family:&quot;Verdana&quot;,sans-serif; color:black">TERMO DE
		COMPROMISSO DO PESQUISADOR&nbsp;</span></b> </p><p align="center" style="margin:0cm; font-size:11pt; font-family:Calibri, sans-serif; text-align:center; background:white"><b><span style="font-size:10.5pt; font-family:&quot;Verdana&quot;,sans-serif; color:black">CDEP</span></b><span style="font-size:10.5pt; font-family:&quot;Verdana&quot;,sans-serif; color:black">&nbsp;</span> </p><p style="margin:0cm; font-size:11pt; font-family:Calibri, sans-serif; background:white"><span style="font-size:10.5pt; font-family:&quot;Verdana&quot;,sans-serif; color:black">&nbsp;</span> </p><p style="margin:0cm; font-size:11pt; font-family:Calibri, sans-serif; text-align:justify; background:white"><span style="font-size:10.5pt; font-family:&quot;Verdana&quot;,sans-serif; color:black">Pesquisador(a), &nbsp;</span> </p><p style="margin:0cm; font-size:11pt; font-family:Calibri, sans-serif; text-align:justify; background:white"><span style="font-size:10.5pt; font-family:&quot;Verdana&quot;,sans-serif; color:black">&nbsp;&nbsp;</span> </p><p style="margin:0cm; font-size:11pt; font-family:Calibri, sans-serif; margin:0cm; text-align:justify; background:white"><span style="font-size:10.5pt; font-family:&quot;Verdana&quot;,sans-serif; color:black">Os&nbsp;acervos&nbsp;da&nbsp;Secretaria
		Municipal de Educação de São Paulo estão&nbsp;disponíveis&nbsp;apenas para
		consulta. O usuário é o único e exclusivo responsável pelo respeito aos
		direitos autorais, personalíssimos e conexos das obras pesquisadas. É vedada a
		reprodução de obras originais ou cópias, no todo ou em parte, de qualquer forma
		e para qualquer finalidade, em conformidade com a </span><span style="font-size:10.5pt; font-family:&quot;Verdana&quot;,sans-serif; color:blue"><a href="https://nam10.safelinks.protection.outlook.com/?url=http:%2f%2fwww.planalto.gov.br%2fccivil_03%2fleis%2fL9610.htm&amp;data=05%7c01%7cmarlon.goncalves%40amcom.com.br%7c042a7a1716714824ec5108dbea93f8ff%7c62803d520a0447558c886e5fcb82b92f%7c0%7c0%7c638361693521019640%7cUnknown%7cTWFpbGZsb3d8eyJWIjoiMC4wLjAwMDAiLCJQIjoiV2luMzIiLCJBTiI6Ik1haWwiLCJXVCI6Mn0%3D%7c3000%7c%7c%7c&amp;sdata=3ghBEnxg2PKKhXplkkuPYCdcvgf4bPZ%2BybLxDxmoB1I%3D&amp;reserved=0" target="_blank">Lei nº 9.610 de
		19.02.1998.</a></span><span style="font-size:10.5pt; font-family:&quot;Verdana&quot;,sans-serif; color:black">&nbsp;&nbsp;</span> </p><p style="margin:0cm; font-size:11pt; font-family:Calibri, sans-serif; text-align:justify; background:white"><span style="font-size:10.5pt; font-family:&quot;Verdana&quot;,sans-serif; color:black">&nbsp;&nbsp;</span> </p><p style="margin:0cm; font-size:11pt; font-family:Calibri, sans-serif; margin:0cm; text-align:justify; background:white"><span style="font-size:10.5pt; font-family:&quot;Verdana&quot;,sans-serif; color:black">Quando
		utilizado em pesquisas e trabalhos acadêmicos dar&nbsp;crédito&nbsp;a <b>Secretaria
		Municipal de Educação de São Paulo</b>,
		como&nbsp;instituição&nbsp;detentora&nbsp;da
		propriedade&nbsp;intelectual&nbsp;do conteúdo&nbsp;pesquisado.&nbsp;</span> </p><br></span>
	   ',
       2024,
       true,
       now(),
       'Sistema',
       'Sistema'
where not exists (select 1 from public.parametro_sistema where nome = 'TermoCompromissoPesquisador' and tipo = 1);


