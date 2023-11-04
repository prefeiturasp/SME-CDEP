
using System.Text.RegularExpressions;

namespace SME.CDEP.Dominio.Extensions
{
    public static class StringExtension
    {
        public static readonly Regex RegexTagsBR = new(Constantes.Constantes.EXPRESSAO_TAG_BR, RegexOptions.Compiled);
        public static readonly Regex RegexTagsP = new(Constantes.Constantes.EXPRESSAO_TAG_P, RegexOptions.Compiled);
        public static readonly Regex RegexTagsLI = new(Constantes.Constantes.EXPRESSAO_TAG_LI, RegexOptions.Compiled);
        public static readonly Regex RegexTagsHTMLQualquer = new(Constantes.Constantes.EXPRESSAO_TAG_HTML_QUALQUER, RegexOptions.Compiled);
        public static readonly Regex RegexEspacosEmBranco = new(Constantes.Constantes.EXPRESSAO_ESPACO_BRANCO, RegexOptions.Compiled);
        
        public static bool EhExtensaoImagemParaOtimizar(this string extensao)
        {
            return (extensao.ToLower().Equals(".jpg") || extensao.ToLower().Equals(".jpeg") || extensao.ToLower().Equals(".png") || extensao.ToLower().Equals(".tiff") || extensao.ToLower().Equals(".tif"));
        }
        
        public static bool EhArquivoImagemParaOtimizar(this string nomeArquivo)
        {
            return EhExtensaoImagemParaOtimizar(Path.GetExtension(nomeArquivo));
        }
        
        public static bool ContemSigla(this string valor, string sigla)
        {
            return valor.ToUpper().EndsWith(sigla);
        }
        
        public static string RemoverSufixo(this string str)
        {
            if (str.Length < 3) 
                return string.Empty;
 
            return str[..^3];
        }

        public static bool EstaPreenchido(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }
        
        public static bool NaoEstaPreenchido(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        
        public static string Limite(this string str, int limite)
        {
            var tamanhoString = str.Length;
            return tamanhoString > limite ? str.Substring(0,limite) : str;
        }
        
        public static string RemoverTagsHtml(this string texto)
        {
            if (texto.NaoEstaPreenchido())
                return string.Empty;
            
            texto = RegexTagsBR.Replace(texto, " ");
            texto = RegexTagsP.Replace(texto, " ");
            texto = RegexTagsLI.Replace(texto, " ");
            texto = RegexTagsHTMLQualquer.Replace(texto, string.Empty);
            texto = RegexEspacosEmBranco.Replace(texto, " ").Trim();
            return texto.Trim();
        }

        public static bool EhArquivoXlsx(this string texto)
        {
            return texto.Equals(Constantes.Constantes.ContentTypeExcel);
        }
        
        public static bool NaoEhArquivoXlsx(this string texto)
        {
            return !EhArquivoXlsx(texto);
        }
        
        public static string[] SplitPipe(this string texto)
        {
            return texto.Trim().Split(Constantes.Constantes.PIPE).Select(s=> s.Trim()).ToArray();
        }
        
        public static bool ValidarLimiteDeCaracteres(this string str, int limite = 0)
        {
            if (limite.EhIgualZero())
                return true;
            
            return str.Length <= limite;
        }
        
        public static bool EhFormatoString(this string formato)
        {
            return formato.Equals(Constantes.Constantes.FORMATO_STRING);
        }
        
        public static bool EhFormatoDouble(this string formato)
        {
            return formato.Equals(Constantes.Constantes.FORMATO_DOUBLE);
        }
        
        public static bool EhFormatoInteiro(this string formato)
        {
            return formato.Equals(Constantes.Constantes.FORMATO_INTEIRO);
        }
        
        public static bool EhFormatoLongo(this string formato)
        {
            return formato.Equals(Constantes.Constantes.FORMATO_LONGO);
        }

        public static IEnumerable<string> FormatarTextoEmArray(this string textoComPipes)
        {
            return textoComPipes.SplitPipe().Distinct();
        }
    }
}
