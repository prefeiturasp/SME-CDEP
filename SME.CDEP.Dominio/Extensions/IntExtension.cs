
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Extensions
{
    public static class IntExtension
    {
        public static bool EhMaiorQueZero(this int valor)
        {
            return valor > 0; 
        }
        
        public static bool EhMenorIgualQueZero(this int valor)
        {
            return valor <= 0; 
        }
        
        public static bool EhIgualZero(this int valor)
        {
            return valor == 0; 
        }
        
        public static bool EhDiferente(this int valor, int valor2)
        {
            return valor != valor2; 
        }
        
        public static bool SaoIguais(this int valor, int valorAComparar)
        {
            return valor == valorAComparar; 
        }
        
        public static bool SaoIguais(this int? valor, int? valorAComparar)
        {
            return valor == valorAComparar; 
        }
        
        public static bool SaoDiferentes(this int valor, int valorAComparar)
        {
            return valor != valorAComparar; 
        }
        
        public static bool EhAnoFuturo(this int year)
        {
            return year > DateTimeExtension.HorarioBrasilia().Year; 
        }
        
        public static int? ObterInteiroOuNuloPorValorDoCampo(this string valorDoCampo)
        {
            if (valorDoCampo.NaoEhNulo() && valorDoCampo.EstaPreenchido())
                return int.Parse(valorDoCampo);

            return default;
        }
        
        public static int ObterFimDaDecadaOuSeculo(this int anoBase)
        {
            return anoBase.ContemSeculo() ? anoBase + 99  : anoBase + 9;
        }
        
        public static bool ContemSeculo(this int anoBase)
        {
            return anoBase % 100 == 0;
        }
        
        public static bool EhDiaValido(this int dia)
        {
            return dia > 0 && dia <= 31;
        }
        
        public static bool EhMesValido(this int mes)
        {
            return mes > 0 && mes <= 12;
        }
    }
}