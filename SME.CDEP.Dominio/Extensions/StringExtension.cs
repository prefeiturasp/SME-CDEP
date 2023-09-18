
namespace SME.CDEP.Dominio.Excecoes
{
    public static class StringExtension
    {
        public static bool EhExtensaoImagemParaOtimizar(this string extensao)
        {
            return (extensao.ToLower().Equals(".jpg") || extensao.ToLower().Equals(".jpeg") || extensao.ToLower().Equals(".png") || extensao.ToLower().Equals(".tiff"));
        }
        
        public static bool EhArquivoImagemParaOtimizar(this string nomeArquivo)
        {
            return EhExtensaoImagemParaOtimizar(Path.GetExtension(nomeArquivo));
        }
        
        public static bool ContemSigla(this string valor)
        {
            return valor.ToUpper().Equals("FT");
        }
    }
}
