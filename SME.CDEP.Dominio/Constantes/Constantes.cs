﻿namespace SME.CDEP.Dominio.Constantes;

public class Constantes
{
    public const string OPCAO_SIM = "sim";
    
    public const string FORMATO_STRING = "string";
    public const string FORMATO_DOUBLE = "double";
    public const string FORMATO_INTEIRO = "int";
    public const string FORMATO_LONGO = "long";

    public const string MATERIAL = "Material";
    public const string TITULO = "Título";
    public const string SUB_TITULO = "Subtítulo";
    public const string CREDITO = "Crédito";
    public const string AUTOR = "Autor";
    public const string CO_AUTOR = "Coautor";
    public const string TIPO_AUTORIA = "Tipo autoria";
    public const string EDITORA = "Editora";
    public const string ASSUNTO = "Assunto";
    public const string ANO = "Ano";
    public const string EDICAO = "Edição";
    public const string NUMERO_PAGINAS = "Número de páginas";
    public const string LARGURA = "Largura";
    public const string ALTURA = "Altura";
    public const string DIAMETRO = "Diâmetro";
    public const string SERIE_COLECAO = "Série/coleção";
    public const string VOLUME = "Volume";
    public const string IDIOMA = "Idioma";
    public const string LOCALIZACAO_CDD = "Localização CDD";
    public const string LOCALIZACAO_PHA = "Localização PHA";
    public const string NOTAS_GERAIS = "Notas gerais";
    public const string ISBN = "Isbn";
    public const string TOMBO = "Tombo";
    public const string CODIGO_ANTIGO = "Código antigo";
    public const string CODIGO_NOVO = "Código novo";
    public const string DESCRICAO = "Descrição";
    public const string TIPO_ANEXO = "Tipo anexo";
    public const string TAMANHO_ARQUIVO = "Tamanho arquivo";
    public const string ACESSO_DOCUMENTO = "Acesso documento";
    public const string LOCALIZACAO = "Localização";
    public const string COPIA_DIGITAL = "Cópia digital";
    public const string AUTORIZACAO_USO_DE_IMAGEM = "Autorização de uso de imagem";
    public const string ESTADO_CONSERVACAO = "Estado conservação";
    public const string PROCEDENCIA = "Procedência";
    public const string DATA = "Data";
    public const string CROMIA = "Cromia";
    public const string TECNICA = "Técnica";
    public const string SUPORTE = "Suporte";
    public const string QUANTIDADE = "Quantidade";
    
    public const string CAMPO_X_NAO_PREENCHIDO = "O campo '{0}' é obrigatório";
    public const string CAMPO_X_ATINGIU_LIMITE_CARACTERES = "Os campos '{0}' atingiu o limite de caracteres";
    public const string CAMPO_X_REQUER_UM_VALOR_NUMERICO = "O campo '{0}' requer um valor numérico";
    public const string CAMPO_COAUTOR_SEM_PREENCHIMENTO_E_TIPO_AUTORIA_PREENCHIDO = "O preenchimento do campo coautor é obrigatório quando o tipo de autoria está especificado";
    public const string TEMOS_MAIS_TIPO_AUTORIA_QUE_COAUTORES = "A quantidade de tipos de autoria excede a de coautores. É aceitável ter coautores desprovidos de tipo de autoria, mas o contrário não é permitido";
    public const string OCORREU_UMA_FALHA_INESPERADA_NA_LINHA_X_MOTIVO_Y = "Ocorreu uma falha inesperada na linha '{0}' -  Motivo: '{1}'";
    public const string OCORREU_UMA_FALHA_INESPERADA_NO_CADASTRO_DAS_REFERENCIAS_MOTIVO_X = "Ocorreu uma falha inesperada no cadastrodas referências -  Motivo: '{0}'";
    public const string CAMPO_CODIGO_ANTIGO_OU_CODIGO_NOVO_DEVE_SER_PREENCHIDO = "O campo código antigo ou código novo deve ser preenchido";
    
    public const char PIPE = '|';
    public const int INICIO_LINHA_TITULO = 1;
    public const int INICIO_LINHA_DADOS = 2;
    
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_TITULO = 1;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_SUB_TITULO = 2;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_MATERIAL = 3;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_AUTOR = 4;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_CO_AUTOR = 5;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_TIPO_DE_AUTORIA = 6;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_EDITORA = 7;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_ASSUNTO = 8;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_ANO = 9;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_EDICAO = 10;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_NUMERO_PAGINAS = 11;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_DIMENSAO_ALTURA = 12;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_DIMENSAO_LARGURA = 13;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_SERIE_COLECAO = 14;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_VOLUME = 15;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_IDIOMA = 16;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_LOCALIZACAO_CDD = 17;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_LOCALIZACAO_PHA = 18;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_NOTAS_GERAIS = 19;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_ISBN = 20;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_TOMBO = 21;
    
    public const int ACERVO_DOCUMENTAL_CAMPO_TITULO = 1;
    public const int ACERVO_DOCUMENTAL_CAMPO_CODIGO_ANTIGO = 2;
    public const int ACERVO_DOCUMENTAL_CAMPO_CODIGO_NOVO = 3;
    public const int ACERVO_DOCUMENTAL_CAMPO_MATERIAL = 4;
    public const int ACERVO_DOCUMENTAL_CAMPO_IDIOMA = 5;
    public const int ACERVO_DOCUMENTAL_CAMPO_AUTOR = 6;
    public const int ACERVO_DOCUMENTAL_CAMPO_ANO = 7;
    public const int ACERVO_DOCUMENTAL_CAMPO_NUMERO_PAGINAS = 8;
    public const int ACERVO_DOCUMENTAL_CAMPO_VOLUME = 9;
    public const int ACERVO_DOCUMENTAL_CAMPO_DESCRICAO = 10;
    public const int ACERVO_DOCUMENTAL_CAMPO_TIPO_ANEXO = 11;
    public const int ACERVO_DOCUMENTAL_CAMPO_DIMENSAO_LARGURA = 12;
    public const int ACERVO_DOCUMENTAL_CAMPO_DIMENSAO_ALTURA = 13;
    public const int ACERVO_DOCUMENTAL_CAMPO_TAMANHO_ARQUIVO = 14;
    public const int ACERVO_DOCUMENTAL_CAMPO_ACESSO_DOCUMENTO = 15;
    public const int ACERVO_DOCUMENTAL_CAMPO_LOCALIZACAO = 16;
    public const int ACERVO_DOCUMENTAL_CAMPO_COPIA_DIGITAL = 17;
    public const int ACERVO_DOCUMENTAL_CAMPO_ESTADO_CONSERVACAO = 18;
    
    public const int ACERVO_ARTE_GRAFICA_CAMPO_TITULO = 1;
    public const int ACERVO_ARTE_GRAFICA_CAMPO_TOMBO = 2;
    public const int ACERVO_ARTE_GRAFICA_CAMPO_CREDITO = 3;
    public const int ACERVO_ARTE_GRAFICA_CAMPO_LOCALIZACAO = 4;
    public const int ACERVO_ARTE_GRAFICA_CAMPO_PROCEDENCIA = 5;
    public const int ACERVO_ARTE_GRAFICA_CAMPO_DATA = 6;
    public const int ACERVO_ARTE_GRAFICA_CAMPO_COPIA_DIGITAL = 7;
    public const int ACERVO_ARTE_GRAFICA_CAMPO_AUTORIZACAO_USO_DE_IMAGEM = 8;
    public const int ACERVO_ARTE_GRAFICA_CAMPO_ESTADO_CONSERVACAO = 9;
    public const int ACERVO_ARTE_GRAFICA_CAMPO_CROMIA = 10;
    public const int ACERVO_ARTE_GRAFICA_CAMPO_DIMENSAO_LARGURA = 11;
    public const int ACERVO_ARTE_GRAFICA_CAMPO_DIMENSAO_ALTURA = 12;
    public const int ACERVO_ARTE_GRAFICA_CAMPO_DIMENSAO_DIAMETRO = 13;
    public const int ACERVO_ARTE_GRAFICA_CAMPO_TECNICA = 14;
    public const int ACERVO_ARTE_GRAFICA_CAMPO_SUPORTE = 15;
    public const int ACERVO_ARTE_GRAFICA_CAMPO_QUANTIDADE = 16;
    public const int ACERVO_ARTE_GRAFICA_CAMPO_DESCRICAO = 17;
    
    public const string ContentTypeExcel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public const string BUCKET_CDEP = "cdep";
    public const string PLANILHA_ACERVO_BIBLIOGRAFICO = "planilha_acervo_bibliografico.xlsx";
    public const string PLANILHA_ACERVO_DOCUMENTAL = "planilha_acervo_documental.xlsx";
    public const string PLANILHA_ACERVO_ARTE_GRAFICA = "planilha_acervo_arte_grafica.xlsx";
    public const string PLANILHA_ACERVO_AUDIOVISUAL = "planilha_acervo_audiovisual.xlsx";
    public const string PLANILHA_ACERVO_FOTOGRAFICO = "planilha_acervo_fotografico.xlsx";
    public const string PLANILHA_ACERVO_TRIDIMENSIONAL = "planilha_acervo_tridimensional.xlsx";
    
    public const string SIGLA_ACERVO_FOTOGRAFICO = ".FT";
    public const string SIGLA_ACERVO_ARTE_GRAFICA = ".AG";
    public const string SIGLA_ACERVO_TRIDIMENSIONAL = ".TD";
    public const string SIGLA_ACERVO_AUDIOVISUAL = ".AV";
    
    public const string VIOLACAO_CONSTRAINT_DUPLICACAO_REGISTROS_CODIGO = "23505";
    public const string VIOLACAO_CONSTRAINT_DUPLICACAO_REGISTROS_MENSAGEM = "Registro duplicado";
    
    public const string PERFIL_EXTERNO_GUID = "3092428D-CA98-4788-9717-E706DF1945A0";
    public const string CLAIM_PERMISSAO = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    public const string CLAIMS = "Claims";
    public const string EXPRESSAO_TAG_BR = @"<br[^>]*>";
    public const string EXPRESSAO_TAG_P = @"<p[^>]*>";
    public const string EXPRESSAO_TAG_LI = @"<li[^>]*>";
    public const string EXPRESSAO_TAG_HTML_QUALQUER = @"<[^>]*>";
    public const string EXPRESSAO_ESPACO_BRANCO = @"&nbsp;";
    
    public const int CARACTERES_PERMITIDOS_500 = 500;
    public const int CARACTERES_PERMITIDOS_200 = 200;
    public const int CARACTERES_PERMITIDOS_100 = 100;
    public const int CARACTERES_PERMITIDOS_50 = 50;
    public const int CARACTERES_PERMITIDOS_15 = 15;
    public const int CARACTERES_PERMITIDOS_4 = 4;
    public const int CARACTERES_PERMITIDOS_3 = 3;
}