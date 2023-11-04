namespace SME.CDEP.Dominio.Extensions
{
    public static class EnumerableStringExtension
    {
       public static string UnificarPipe(this IEnumerable<string> textos)
        {
            return string.Join(Constantes.Constantes.PIPE,textos);
        }
    }
}
