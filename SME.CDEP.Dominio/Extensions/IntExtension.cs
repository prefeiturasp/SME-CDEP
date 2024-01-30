﻿
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
    }
}