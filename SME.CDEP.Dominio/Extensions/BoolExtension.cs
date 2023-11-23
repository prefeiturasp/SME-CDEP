
namespace SME.CDEP.Dominio.Extensions
{
    public static class BoolExtension
    {
        public static bool SaoIguais(this bool valor, bool valorAComparar)
        {
            return valor == valorAComparar; 
        }
    }
}