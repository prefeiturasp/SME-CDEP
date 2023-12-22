namespace SME.CDEP.Dominio.Extensions
{
    public static class EnumerableStringExtension
    {
       public static string UnificarPipe(this IEnumerable<string> textos)
        {
            return string.Join(Constantes.Constantes.PIPE,textos);
        }
       
        public static bool PossuiElementos<T>(this IEnumerable<T> items)
        {
            return items.NaoEhNulo() && items.Any();
        }
        
        public static bool NaoPossuiElementos<T>(this IEnumerable<T> items)
        {
            return items.EhNulo() || !items.Any();
        }
    }
}
