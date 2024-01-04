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
        
        public static bool ObterBooleanoPorValorDoCampo(this string valorDoCampo)
        {
            if (valorDoCampo.NaoEhNulo() && valorDoCampo.EstaPreenchido())
                return bool.Parse(valorDoCampo);

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
        
        public static double? FormatarParaDoubleComCasasDecimais(this double? valor)
        {
            if (!valor.HasValue)
                return default;
            
            if (valor.Value.ToString().Contains(",") || valor.Value.ToString().Contains("."))
                return valor.Value;
            
            return valor.Value/100; 
        }
        
        public static double? FormatarDoubleComCasasDecimais(this double valor)
        {
            if (valor.EhNulo())
                return default;
            
            if (valor.ToString().Contains(",") || valor.ToString().Contains("."))
                return valor;
            
            return valor/100; 
        }
        
        public static string FormatarDoubleComCasasDecimais(this double? valor)
        {
            if (!valor.HasValue)
                return default;
            
            return valor.Value.ToString("F2"); 
        }
    }
}