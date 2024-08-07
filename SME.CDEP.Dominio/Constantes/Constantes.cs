﻿namespace SME.CDEP.Dominio.Constantes;

public class Constantes
{
    public const string INSTITUICAO_NAO_IDENTIFICADA = "Não identificada";
    public const int QTDE_CARACTERES_270 = 270;
    public const string ACERVO_DISPONIVEL = "Disponível";
    public const string ACERVO_INDISPONIVEL = "Indisponível";
    public const string VALIDAR_EMAIL = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    public const string USUARIO_SEM_CADASTRO_CDEP = "Usuário sem cadastro no CDEP, para que o atendimento seja realizado, é necessário que o usuário se cadastre primeiro.";
    public const string USUARIO_NAO_ENCONTRADO = "Usuário não encontrado.";
    public const string TRIDIMENSIONAL = "Tridimensional";
    public const string FOTOGRAFICO = "Fotográfico";
    public const string DOCUMENTAL = "Documental";
    public const string BIBLIOGRAFICO = "Bibliográfico";
    public const string AUDIOVISUAL = "Audiovisual";
    public const string ARTE_GRAFICA = "Arte Gráfica";
    
    public const string OPCAO_SIM = "sim";
    public const string OPCAO_NAO = "não";
    
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
    public const string COPIA = "Cópia";
    public const string AUTORIZACAO_USO_DE_IMAGEM = "Autorização de uso de imagem";
    public const string ESTADO_CONSERVACAO = "Estado conservação";
    public const string PROCEDENCIA = "Procedência";
    public const string DATA = "Data";
    public const string CROMIA = "Cromia";
    public const string TECNICA = "Técnica";
    public const string SUPORTE = "Suporte";
    public const string QUANTIDADE = "Quantidade";
    public const string DURACAO = "Duração";
    public const string ACESSIBILIDADE = "Acessibilidade";
    public const string DISPONIBILIDADE = "Disponibilidade";
    public const string FORMATO_IMAGEM = "Formato da imagem";
    public const string RESOLUCAO = "Resolução";
    public const string PROFUNDIDADE = "Profundidade";
    
    public const string CAMPO_X_NAO_PREENCHIDO = "O campo '{0}' é obrigatório";
    public const string CAMPO_X_ATINGIU_LIMITE_CARACTERES = "O campo '{0}' atingiu o limite de caracteres";
    public const string CAMPO_X_REQUER_UM_VALOR_NUMERICO = "O campo '{0}' requer um valor numérico";
    public const string CAMPO_COAUTOR_SEM_PREENCHIMENTO_E_TIPO_AUTORIA_PREENCHIDO = "O preenchimento do campo coautor é obrigatório quando o tipo de autoria está especificado";
    public const string TEMOS_MAIS_TIPO_AUTORIA_QUE_COAUTORES = "A quantidade de tipos de autoria excede a de coautores. É aceitável ter coautores desprovidos de tipo de autoria, mas o contrário não é permitido";
    public const string OCORREU_UMA_FALHA_INESPERADA_NA_LINHA_X_MOTIVO_Y = "Ocorreu uma falha inesperada na linha '{0}' -  Motivo: '{1}'";
    public const string O_VALOR_X_DO_CAMPO_X_NAO_FOI_LOCALIZADO = "O valor '{0}' do campo '{1}' não foi localizado";
    public const string NAO_FOI_POSSIVEL_LER_A_PLANILHA = "Não foi possível ler a planilha";
    public const string VALOR_X_DO_CAMPO_X_NAO_PERMITIDO_ESPERADO_X = "O valor '{0}' do campo '{1}' não permitido, esperado: '{2}'";
    public const string O_CAMPO_X_NAO_EH_UM_VALOR_NUMERICO_Y = "O valor '{0}' não é um tipo numérico '{1}'";
    public const string ARQUIVO_NAO_ENCONTRADO = "Arquivo não encontrado";
    public const string CONTEUDO_DO_ARQUIVO_INVALIDO = "Conteúdo do arquivo inválido";
    public const string ESSE_ARQUIVO_NAO_EH_ACERVO_X = "Esse arquivo não é do Acervo {0}";
    public const string A_LINHA_INFORMADA_NAO_EXISTE_NO_ARQUIVO = "Essa linha não existe no arquivo";
    public const string NAO_EH_POSSIVEL_EXCLUIR_A_UNICA_LINHA_DO_ARQUIVO = "Não foi possível excluir a linha, pois a remoção da única linha resultaria em um arquivo vazio";
    public const string A_PLANLHA_DE_ACERVO_X_NAO_TEM_O_NOME_DA_COLUNA_Y_NA_COLUNA_Z = "A planilha do acervo '{0}' deveria apresentar o nome '{1}' na coluna '{2}', conforme previsto";
    
    public const char PIPE = '|';
    public const int INICIO_LINHA_TITULO = 1;
    public const int INICIO_LINHA_DADOS = 2;
    
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_TITULO = 1;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_SUB_TITULO = 2;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_MATERIAL = 3;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_AUTOR = 4;
    public const int ACERVO_BIBLIOGRAFICO_CAMPO_COAUTOR = 5;
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
    public const int ACERVO_ARTE_GRAFICA_CAMPO_ANO = 6;
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
    
    public const int ACERVO_AUDIOVISUAL_CAMPO_TITULO = 1;
    public const int ACERVO_AUDIOVISUAL_CAMPO_TOMBO = 2;
    public const int ACERVO_AUDIOVISUAL_CAMPO_CREDITO = 3;
    public const int ACERVO_AUDIOVISUAL_CAMPO_LOCALIZACAO = 4;
    public const int ACERVO_AUDIOVISUAL_CAMPO_PROCEDENCIA = 5;
    public const int ACERVO_AUDIOVISUAL_CAMPO_ANO = 6;
    public const int ACERVO_AUDIOVISUAL_CAMPO_COPIA = 7;
    public const int ACERVO_AUDIOVISUAL_CAMPO_AUTORIZACAO_USO_DE_IMAGEM = 8;
    public const int ACERVO_AUDIOVISUAL_CAMPO_ESTADO_CONSERVACAO = 9;
    public const int ACERVO_AUDIOVISUAL_CAMPO_DESCRICAO = 10;
    public const int ACERVO_AUDIOVISUAL_CAMPO_SUPORTE = 11;
    public const int ACERVO_AUDIOVISUAL_CAMPO_DURACAO = 12;
    public const int ACERVO_AUDIOVISUAL_CAMPO_CROMIA = 13;
    public const int ACERVO_AUDIOVISUAL_CAMPO_TAMANHO_ARQUIVO = 14;
    public const int ACERVO_AUDIOVISUAL_CAMPO_ACESSIBILIDADE = 15;
    public const int ACERVO_AUDIOVISUAL_CAMPO_DISPONIBILIZACAO = 16;
    
    public const int ACERVO_FOTOGRAFICO_CAMPO_TITULO = 1;
    public const int ACERVO_FOTOGRAFICO_CAMPO_CODIGO = 2;
    public const int ACERVO_FOTOGRAFICO_CAMPO_CREDITO = 3;
    public const int ACERVO_FOTOGRAFICO_CAMPO_LOCALIZACAO = 4;
    public const int ACERVO_FOTOGRAFICO_CAMPO_PROCEDENCIA = 5;
    public const int ACERVO_FOTOGRAFICO_CAMPO_ANO = 6;
    public const int ACERVO_FOTOGRAFICO_CAMPO_DATA = 7;
    public const int ACERVO_FOTOGRAFICO_CAMPO_COPIA_DIGITAL = 8;
    public const int ACERVO_FOTOGRAFICO_CAMPO_AUTORIZACAO_USO_DE_IMAGEM = 9;
    public const int ACERVO_FOTOGRAFICO_CAMPO_ESTADO_CONSERVACAO = 10;
    public const int ACERVO_FOTOGRAFICO_CAMPO_DESCRICAO = 11;
    public const int ACERVO_FOTOGRAFICO_CAMPO_QUANTIDADE = 12;
    public const int ACERVO_FOTOGRAFICO_CAMPO_LARGURA = 13;
    public const int ACERVO_FOTOGRAFICO_CAMPO_ALTURA = 14;
    public const int ACERVO_FOTOGRAFICO_CAMPO_SUPORTE = 15;
    public const int ACERVO_FOTOGRAFICO_CAMPO_FORMATO_IMAGEM = 16;
    public const int ACERVO_FOTOGRAFICO_CAMPO_TAMANHO_ARQUIVO = 17;
    public const int ACERVO_FOTOGRAFICO_CAMPO_CROMIA = 18;
    public const int ACERVO_FOTOGRAFICO_CAMPO_RESOLUCAO = 19;
    
    public const int ACERVO_TRIDIMENSIONAL_CAMPO_TITULO = 1;
    public const int ACERVO_TRIDIMENSIONAL_CAMPO_TOMBO = 2;
    public const int ACERVO_TRIDIMENSIONAL_CAMPO_PROCEDENCIA = 3;
    public const int ACERVO_TRIDIMENSIONAL_CAMPO_ANO = 4;
    public const int ACERVO_TRIDIMENSIONAL_CAMPO_ESTADO_CONSERVACAO = 5;
    public const int ACERVO_TRIDIMENSIONAL_CAMPO_QUANTIDADE = 6;
    public const int ACERVO_TRIDIMENSIONAL_CAMPO_DESCRICAO = 7;
    public const int ACERVO_TRIDIMENSIONAL_CAMPO_LARGURA = 8;
    public const int ACERVO_TRIDIMENSIONAL_CAMPO_ALTURA = 9;
    public const int ACERVO_TRIDIMENSIONAL_CAMPO_PROFUNDIDADE = 10;
    public const int ACERVO_TRIDIMENSIONAL_CAMPO_DIAMETRO = 11;
    
    public const string NOME_DA_COLUNA_TITULO = "TÍTULO";
    public const string NOME_DA_COLUNA_SUBTITULO = "SUBTÍTULO";
    public const string NOME_DA_COLUNA_TOMBO = "TOMBO";
    public const string NOME_DA_COLUNA_CODIGO_ANTIGO = "Código antigo";
    public const string NOME_DA_COLUNA_CODIGO_NOVO = "Código novo";
    public const string NOME_DA_COLUNA_CREDITO = "CRÉDITO";
    public const string NOME_DA_COLUNA_AUTOR = "AUTOR";
    public const string NOME_DA_COLUNA_COAUTOR = "COAUTOR";
    public const string NOME_DA_COLUNA_PROCEDENCIA = "PROCEDÊNCIA";
    public const string NOME_DA_COLUNA_DATA = "DATA";
    public const string NOME_DA_COLUNA_ESTADO_DE_CONSERVACAO = "ESTADO DE CONSERVAÇÃO";
    public const string NOME_DA_COLUNA_QUANTIDADE = "QUANTIDADE";
    public const string NOME_DA_COLUNA_DESCRICAO = "DESCRIÇÃO";
    public const string NOME_DA_COLUNA_DIMENSAO_LARGURA = "DIMENSÃO LARGURA (CM)";
    public const string NOME_DA_COLUNA_DIMENSAO_ALTURA = "DIMENSÃO ALTURA (CM)";
    public const string NOME_DA_COLUNA_DIMENSAO_PROFUNDIDADE = "DIMENSÃO PROFUNDIDADE (CM)";
    public const string NOME_DA_COLUNA_DIMENSAO_DIAMETRO = "DIMENSÃO DIÂMETRO (CM)";
    public const string NOME_DA_COLUNA_LOCALIZACAO = "LOCALIZAÇÃO";
    public const string NOME_DA_COLUNA_COPIA_DIGITAL = "CÓPIA DIGITAL";
    public const string NOME_DA_COLUNA_COPIA = "CÓPIA";
    public const string NOME_DA_COLUNA_AUTORIZACAO_USO_DE_IMAGEM = "AUTORIZAÇÃO DO USO DE IMAGEM";
    public const string NOME_DA_COLUNA_SUPORTE = "SUPORTE";
    public const string NOME_DA_COLUNA_FORMATO_DA_IMAGEM = "FORMATO DA IMAGEM";
    public const string NOME_DA_COLUNA_TAMANHO_DO_ARQUIVO = "TAMANHO DO ARQUIVO";
    public const string NOME_DA_COLUNA_CROMIA = "CROMIA";
    public const string NOME_DA_COLUNA_RESOLUCAO = "RESOLUÇÃO";
    public const string NOME_DA_COLUNA_MATERIAL = "MATERIAL";
    public const string NOME_DA_COLUNA_IDIOMA = "IDIOMA";
    public const string NOME_DA_COLUNA_ANO = "ANO";
    public const string NOME_DA_COLUNA_NUMERO_PAGINAS = "NUMERO PÁGINAS";
    public const string NOME_DA_COLUNA_VOLUME = "VOLUME";
    public const string NOME_DA_COLUNA_TIPO_DE_ANEXO = "TIPO DE ANEXO";
    public const string NOME_DA_COLUNA_ACESSO_DO_DOCUMENTO = "ACESSO DO DOCUMENTO";
    public const string NOME_DA_COLUNA_TIPO_DE_AUTORIA = "TIPO DE AUTORIA";
    public const string NOME_DA_COLUNA_EDITORA = "EDITORA";
    public const string NOME_DA_COLUNA_ASSUNTO = "ASSUNTO";
    public const string NOME_DA_COLUNA_EDICAO = "EDIÇÃO";
    public const string NOME_DA_COLUNA_SERIE_COLECAO = "SERIE COLEÇÃO";
    public const string NOME_DA_COLUNA_LOCALIZACAO_CDD = "LOCALIZAÇÃO CDD";
    public const string NOME_DA_COLUNA_LOCALIZACAO_PHA = "LOCALIZAÇÃO PHA";
    public const string NOME_DA_COLUNA_NOTAS_GERAIS = "NOTAS GERAIS";
    public const string NOME_DA_COLUNA_ISBN = "ISBN";
    public const string NOME_DA_COLUNA_DURACAO = "DURAÇÃO";
    public const string NOME_DA_COLUNA_ACESSIBILIDADE = "ACESSIBILIDADE";
    public const string NOME_DA_COLUNA_DISPONIBILIZACAO = "DISPONIBILIZAÇÃO";
    public const string NOME_DA_COLUNA_TECNICA = "TÉCNICA";
    
    
    public const string CONTENT_TYPE_EXCEL = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public const string CONTENT_TYPE_JPEG = "image/jpeg";
    public const string CONTENT_TYPE_TIFF = "image/tiff";
    public const string PLANILHA_ACERVO_BIBLIOGRAFICO = "planilha_acervo_bibliografico.xlsx";
    public const string PLANILHA_ACERVO_DOCUMENTAL = "planilha_acervo_documental.xlsx";
    public const string PLANILHA_ACERVO_ARTE_GRAFICA = "planilha_acervo_arte_grafica.xlsx";
    public const string PLANILHA_ACERVO_AUDIOVISUAL = "planilha_acervo_audiovisual.xlsx";
    public const string PLANILHA_ACERVO_FOTOGRAFICO = "planilha_acervo_fotografico.xlsx";
    public const string PLANILHA_ACERVO_TRIDIMENSIONAL = "planilha_acervo_tridimensional.xlsx";
    
    public const string IMAGEM_PADRAO_ACERVO_BIBLIOGRAFICO = "Bibliografico_sem_imagem.svg";
    public const string IMAGEM_PADRAO_ACERVO_DOCUMENTAL = "Documentacao_sem_imagem.svg";
    public const string IMAGEM_PADRAO_ACERVO_ARTE_GRAFICA = "Artesgraficas_sem_imagem.svg";
    public const string IMAGEM_PADRAO_ACERVO_AUDIOVISUAL = "Audiovisual_sem_imagem.svg";
    public const string IMAGEM_PADRAO_ACERVO_FOTOGRAFICO = "Fotografico_sem_imagem.svg";
    public const string IMAGEM_PADRAO_ACERVO_TRIDIMENSIONAL = "Tridimensional_sem_Imagem.svg";
    
    public const string SIGLA_ACERVO_FOTOGRAFICO = ".FT";
    public const string SIGLA_ACERVO_ARTE_GRAFICA = ".AG";
    public const string SIGLA_ACERVO_TRIDIMENSIONAL = ".TD";
    public const string SIGLA_ACERVO_AUDIOVISUAL = ".AV";
    
    public const string VIOLACAO_CONSTRAINT_DUPLICACAO_REGISTROS_CODIGO = "23505";
    public const string VIOLACAO_CONSTRAINT_DUPLICACAO_REGISTROS_MENSAGEM = "Registro duplicado";
    
    public const string PERFIL_EXTERNO_GUID = "3092428D-CA98-4788-9717-E706DF1945A0";
    public const string PERFIL_ADMIN_BIBLIOTECA_GUID = "B82673B9-52B9-4E01-9157-E19339B7211A";
    public const string PERFIL_ADMIN_GERAL_GUID = "D3766FB4-D753-4398-BFB0-C357724BB0A2";
    public const string PERFIL_BASICO_GUID = "064B3481-439B-4C67-8C88-5D1F1E9B91CE";
    public const string PERFIL_ADMIN_MEMORIA_GUID = "35F9D620-49A8-446A-8A75-0A0D26EBD79D";
    public const string PERFIL_ADMIN_MEMORIAL_GUID = "89C9D50D-B73B-4DDE-B870-7685FCD88B0C";
    
    public const string CLAIM_PERMISSAO = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    public const string CLAIMS = "Claims";
    public const string EXPRESSAO_TAG_BR = @"<br[^>]*>";
    public const string EXPRESSAO_TAG_P = @"<p[^>]*>";
    public const string EXPRESSAO_TAG_LI = @"<li[^>]*>";
    public const string EXPRESSAO_TAG_HTML_QUALQUER = @"<[^>]*>";
    public const string EXPRESSAO_ESPACO_BRANCO = @"&nbsp;";
    
    public const int CARACTERES_PERMITIDOS_500 = 500;
    public const int CARACTERES_PERMITIDOS_200 = 200;
    public const int CARACTERES_PERMITIDOS_270 = 270;
    public const int CARACTERES_PERMITIDOS_100 = 100;
    public const int CARACTERES_PERMITIDOS_50 = 50;
    public const int CARACTERES_PERMITIDOS_30 = 30;
    public const int CARACTERES_PERMITIDOS_15 = 15;
    public const int CARACTERES_PERMITIDOS_7 = 7;
    public const int CARACTERES_PERMITIDOS_4 = 4;
    public const int CARACTERES_PERMITIDOS_3 = 3;
    
    public const string CREDITOS_AUTORES = "Créditos/Autores";
    public const string ASSUNTOS = "Assuntos";

    public const string PERMITIR_SOMENTE_NUMERAL_SEPARADO_POR_VIRGULA_DUAS_CASAS_DECIMAIS = @"^(\d+,\d{2})$";

    #region Constantes Context Mapper

    public const string PERFIL_USUARIO_LOGADO_DESABILITA_DATA_VISITA = "PerfilUsuarioLogadoDesabilitaDataVisita";

    #endregion
}