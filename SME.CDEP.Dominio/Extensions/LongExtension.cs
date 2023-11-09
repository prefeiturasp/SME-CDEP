using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Extensions
{
    public static class LongExtension
    {
        public static bool EhAcervoDocumental(this long valor)
        {
            return valor == (long)TipoAcervo.DocumentacaoHistorica; 
        }
        
        public static bool NaoEhAcervoDocumental(this long valor)
        {
            return !EhAcervoDocumental(valor); 
        }
        
        public static bool EhMaiorQueZero(this long valor)
        {
            return valor > 0; 
        }
        
        public static bool EhMenorIgualQueZero(this long valor)
        {
            return valor <= 0; 
        }
        
        public static long? ObterLongoOuNuloPorValorDoCampo(this string valorDoCampo)
        {
            if (valorDoCampo.NaoEhNulo())
                return long.Parse(valorDoCampo);

            return default;
        }
        
        public static long ObterLongoPorValorDoCampo(this string valorDoCampo)
        {
            if (valorDoCampo.NaoEhNulo() && long.TryParse(valorDoCampo, out long valorLongo))
                return valorLongo;

            throw new NegocioException(string.Format(Constantes.Constantes.O_CAMPO_X_NAO_EH_UM_VALOR_NUMERICO_Y,valorDoCampo, Constantes.Constantes.FORMATO_LONGO));
        }
        
        public static bool SaoIguais(this long valor, long valorAComparar)
        {
            return valor == valorAComparar; 
        }
        
        public static bool SaoIguais(this long? valor, long? valorAComparar)
        {
            return valor == valorAComparar; 
        }
    }
}