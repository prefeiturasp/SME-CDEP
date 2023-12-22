
namespace SME.CDEP.Dominio.Extensions
{
    public static class BoolExtension
    {
        public static bool SaoIguais(this bool valor, bool valorAComparar)
        {
            return valor == valorAComparar; 
        }
        
        public static string ObterSimNaoVazio(this bool? valor)
        {
            if (!valor.HasValue)
                return string.Empty;
            
            return valor.Value ? "Sim" : "Não"; 
        }
        
        public static string ObterSimNao(this bool valor)
        {
            return valor ? "Sim" : "Não"; 
        }
    }
}