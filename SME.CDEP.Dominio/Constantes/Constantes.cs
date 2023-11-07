namespace SME.CDEP.Dominio.Constantes;

public class Constantes
{
    public static string FORMATO_STRING = "string";
    public static string FORMATO_DOUBLE = "double";
    public static string FORMATO_INTEIRO = "int";
    public static string FORMATO_LONGO = "long";

    public const string MATERIAL = "material";
    public const string TITULO = "título";
    public const string SUB_TITULO = "subtítulo";
    public const string AUTOR = "autor";
    public const string CO_AUTOR = "coautor";
    public const string TIPO_AUTORIA = "tipo autoria";
    public const string EDITORA = "editora";
    public const string ASSUNTO = "assunto";
    public const string ANO = "ano";
    public const string EDICAO = "edição";
    public const string NUMERO_PAGINAS = "número de páginas";
    public const string LARGURA = "largura";
    public const string ALTURA = "altura";
    public const string SERIE_COLECAO = "série/coleção";
    public const string VOLUME = "volume";
    public const string IDIOMA = "idioma";
    public const string LOCALIZACAO_CDD = "localização CDD";
    public const string LOCALIZACAO_PHA = "localização PHA";
    public const string NOTAS_GERAIS = "notas gerais";
    public const string ISBN = "isbn";
    public const string TOMBO = "tombo";
    
    public const string CAMPO_X_NAO_PREENCHIDO = "O campo '{0}' é obrigatório";
    public const string CAMPO_X_ATINGIU_LIMITE_CARACTERES = "Os campos '{0}' atingiu o limite de caracteres";
    public const string CAMPO_X_REQUER_UM_VALOR_NUMERICO = "O campo '{0}' requer um valor numérico";
    public const string CAMPO_COAUTOR_SEM_PREENCHIMENTO_E_TIPO_AUTORIA_PREENCHIDO = "O preenchimento do campo coautor é obrigatório quando o tipo de autoria está especificado";
    public const string TEMOS_MAIS_TIPO_AUTORIA_QUE_COAUTORES = "A quantidade de tipos de autoria excede a de coautores. É aceitável ter coautores desprovidos de tipo de autoria, mas o contrário não é permitido";
    public const string OCORREU_UMA_FALHA_INESPERADA_NA_LINHA_X_MOTIVO_Y = "Ocorreu uma falha inesperada na linha '{0}' -  Motivo: '{1}'";
    public const string OCORREU_UMA_FALHA_INESPERADA_NO_CADASTRO_DAS_REFERENCIAS_MOTIVO_X = "Ocorreu uma falha inesperada no cadastrodas referências -  Motivo: '{0}'";
    
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
}