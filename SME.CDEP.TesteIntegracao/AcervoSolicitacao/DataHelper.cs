namespace SME.CDEP.TesteIntegracao
{
    public class DataHelper
    {
        public static DateTime ProximaDataUtil(DateTime data)
        {
            while (data.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
                data = data.AddDays(1);

            return data.Date;
        }
    }
}
