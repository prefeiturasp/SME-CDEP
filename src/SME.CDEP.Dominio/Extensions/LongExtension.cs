using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Extensions
{
    public static class LongExtension
    {
        public static bool EhAcervoDocumental(this long valor)
        {
            return valor == (long)TipoAcervo.DocumentacaoTextual; 
        }
        
        public static bool EhAcervoTridimensional(this long valor)
        {
            return valor == (long)TipoAcervo.Tridimensional; 
        }
        
        public static bool EhAcervoBibliografico(this long valor)
        {
            return valor == (long)TipoAcervo.Bibliografico; 
        }
        
        public static bool NaoEhAcervoBibliografico(this long valor)
        {
            return !EhAcervoBibliografico(valor); 
        }
        
        public static bool EhAcervoArteGrafica(this long valor)
        {
            return valor == (long)TipoAcervo.ArtesGraficas; 
        }
        
        public static bool EhAcervoFotografico(this long valor)
        {
            return valor == (long)TipoAcervo.Fotografico; 
        }
        
        public static bool EhAcervoAudiovisual(this long valor)
        {
            return valor == (long)TipoAcervo.Audiovisual; 
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
            if (valorDoCampo.NaoEhNulo() && valorDoCampo.EstaPreenchido())
                return long.Parse(valorDoCampo);

            return default;
        }
        
        public static long ObterLongoPorValorDoCampo(this string valorDoCampo)
        {
            if (valorDoCampo.NaoEhNulo() && valorDoCampo.EstaPreenchido() && long.TryParse(valorDoCampo, out long valorLongo))
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