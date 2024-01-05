
using System.Drawing.Imaging;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using SME.CDEP.Dominio.Excecoes;

namespace SME.CDEP.Dominio.Extensions
{
    public static class StringExtension
    {
        public static readonly Regex RegexTagsBR = new(Constantes.Constantes.EXPRESSAO_TAG_BR, RegexOptions.Compiled);
        public static readonly Regex RegexTagsP = new(Constantes.Constantes.EXPRESSAO_TAG_P, RegexOptions.Compiled);
        public static readonly Regex RegexTagsLI = new(Constantes.Constantes.EXPRESSAO_TAG_LI, RegexOptions.Compiled);
        public static readonly Regex RegexTagsHTMLQualquer = new(Constantes.Constantes.EXPRESSAO_TAG_HTML_QUALQUER, RegexOptions.Compiled);
        public static readonly Regex RegexEspacosEmBranco = new(Constantes.Constantes.EXPRESSAO_ESPACO_BRANCO, RegexOptions.Compiled);
        
        public static bool EhExtensaoImagemGerarMiniatura(this string extensao)
        {
            return (extensao.ToLower().Contains(".jpg") || extensao.ToLower().Contains(".jpeg") || extensao.ToLower().Contains(".png") || extensao.ToLower().Contains(".tiff") || extensao.ToLower().Contains(".tif"));
        }
        
        public static bool EhArquivoImagemParaOtimizar(this string nomeArquivo)
        {
            return EhExtensaoImagemGerarMiniatura(Path.GetExtension(nomeArquivo));
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
            return texto.Equals(Constantes.Constantes.CONTENT_TYPE_EXCEL);
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
        
        public static IEnumerable<string> FormatarTextoEmArraySemDistinct(this string textoComPipes)
        {
            return textoComPipes.SplitPipe();
        }
        
        public static bool EhOpcaoSim(this string valor)
        {
            return valor.ToLower().Equals(Constantes.Constantes.OPCAO_SIM);
        }
        
        public static bool EhOpcaoNao(this string valor)
        {
            return valor.ToLower().Equals(Constantes.Constantes.OPCAO_NAO);
        }
        
        public static bool SaoIguais(this string valor, string valorAComparar)
        {
            return valor.ToLower().Equals(valorAComparar.ToLower()); 
        }
        
        public static bool SaoDiferentes(this string valor, string valorAComparar)
        {
            return !valor.ToLower().Equals(valorAComparar.ToLower()); 
        }
        
        public static int ConverterParaInteiro(this string valor)
        {
            return int.Parse(valor); 
        }
        
        public static double ConverterParaDouble(this string valor)
        {
            return double.Parse(valor); 
        }
        
        public static bool EhImagemTiff(this string nomeArquivo)
        {
            return nomeArquivo.ToLower().Contains(Constantes.Constantes.CONTENT_TYPE_TIFF);
        }
        
        public static string ObterValorOuZero(this string valor)
        {
            return valor.EstaPreenchido() ? valor : "0";
        }
        public static string RemoverAcentuacao(this string valor)
        {
            if (valor.NaoEstaPreenchido())
                return valor;
            
            return new string(valor
                .Normalize(NormalizationForm.FormD)
                .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                .ToArray());
        }
        public static string RemoverAcentuacaoFormatarMinusculo(this string valor)
        {
            if (valor.NaoEstaPreenchido())
                return valor;
            
            return new string(valor
                .Normalize(NormalizationForm.FormD)
                .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                .ToArray()).ToLower();
        }
        
        public static string ObterExtensao(this string valor)
        {
            if (valor.NaoEstaPreenchido())
                return valor;
                
            return Path.GetExtension(valor);
        }
    }
}
