using System;
using SME.CDEP.Dominio.Extensions;

namespace SME.CDEP.Infra.Dominio.Enumerados
{
    public static class DateTimeExtension
    {
        private static readonly TimeZoneInfo fusoHorarioBrasil = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

        public static DateTime Local(this DateTime data)
        {
            return data;
        }
       
        public static DateTime HorarioBrasilia()
        {
            return DateTime.UtcNow.AddHours(-3);
        }

        public static DateTime ObterDomingo(this DateTime data)
        {
            if (data.DayOfWeek == DayOfWeek.Sunday)
                return data;
            int diferenca = (7 + (data.DayOfWeek - DayOfWeek.Sunday)) % 7;
            return data.AddDays(-1 * diferenca).Date;
        }

        public static DateTime ObterSabado(this DateTime data)
        {
            if (data.DayOfWeek == DayOfWeek.Saturday)
                return data;
            int diferenca = (((int)DayOfWeek.Saturday - (int)data.DayOfWeek + 7) % 7);
            return data.AddDays(diferenca);
        }

        public static bool FimDeSemana(this DateTime data)
            => data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday;

        public static bool Domingo(this DateTime data)
            => data.DayOfWeek == DayOfWeek.Sunday;

        public static int Semestre(this DateTime data)
            => data.Month > 6 ? 2 : 1;

        public static DateTime DiaRetroativo(this DateTime data, int nrDias)
        {
            int contadorDias = nrDias;
            DateTime dataRetorno = data;

            while (contadorDias > 0)
            {
                if (!dataRetorno.FimDeSemana())
                    contadorDias--;

                dataRetorno = dataRetorno.AddDays(-1);
            }

            return dataRetorno;
        }
        public static DateTime ObterDomingoRetroativo(this DateTime data)
        {
            if (data.DayOfWeek == DayOfWeek.Sunday)
                return data;
            int diferenca = (((int)DayOfWeek.Sunday - (int)data.DayOfWeek - 7) % 7);
            return data.AddDays(diferenca);
        }
        
        public static bool EhMaiorOuIgualQue(this DateTime? dataAvaliada, DateTime? dataReferencia)
        {
            if (!dataAvaliada.HasValue)
                return false;
            
            if (!dataReferencia.HasValue)
                return false;

            return dataReferencia.Value.Date <= dataAvaliada.Value.Date;
        }
        
        public static bool EhMenorQue(this DateTime? dataAvaliada, DateTime? dataReferencia)
        {
            if (!dataAvaliada.HasValue)
                return false;
            
            if (!dataReferencia.HasValue)
                return false;

            return dataAvaliada.Value.EhMenorQue(dataReferencia.Value);
        }
        
        public static bool EhDataFutura(this DateTime? dataAvaliada)
        {
            if (!dataAvaliada.HasValue)
                return false;

            return dataAvaliada.Value.EhDataFutura();
        }
        
        public static bool EhDataFutura(this DateTime dataAvaliada)
        {
            return dataAvaliada.Date > HorarioBrasilia().Date;
        }
        
        public static bool NaoEhDataFutura(this DateTime dataAvaliada)
        {
            return HorarioBrasilia().Date >= dataAvaliada.Date;
        }
        
        public static bool EhMenorQue(this DateTime dataAvaliada, DateTime dataReferencia)
        {
            return dataAvaliada.Date < dataReferencia.Date;
        }
    }
}