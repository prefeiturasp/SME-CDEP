using SME.CDEP.Dominio.Excecoes;

namespace SME.CDEP.Dominio.Extensions
{
    public static class DoubleExtension
    {
        public static double? ObterDoubleOuNuloPorValorDoCampo(this string valorDoCampo)
        {
            if (valorDoCampo.NaoEhNulo() && valorDoCampo.EstaPreenchido())
                return double.Parse(valorDoCampo);

            return default;
        }
        
        public static double ObterDoublePorValorDoCampo(this string valorDoCampo)
        {
            if (valorDoCampo.NaoEhNulo() && valorDoCampo.EstaPreenchido())
                return double.Parse(valorDoCampo);

            throw new NegocioException(string.Format(Constantes.Constantes.O_CAMPO_X_NAO_EH_UM_VALOR_NUMERICO_Y,valorDoCampo, Constantes.Constantes.FORMATO_DOUBLE));
        }
        
        public static bool SaoIguais(this double valor, double valorAComparar)
        {
            return valor == valorAComparar; 
        }
        
        public static bool SaoIguais(this double? valor, double? valorAComparar)
        {
            return valor == valorAComparar; 
        }
    }
}