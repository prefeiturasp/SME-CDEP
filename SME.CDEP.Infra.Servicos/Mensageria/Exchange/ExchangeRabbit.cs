namespace SME.CDEP.Infra.Servicos.Mensageria.Exchange
{
    public static class ExchangeRabbit
    {
        public static string Logs = "EnterpriseApplicationLog";
        public static string QueueLogs => "EnterpriseQueueLog";
        public static string Cdep => "sme.cdep.workers";
        public static string CdepDeadLetter => "sme.cdep.workers.deadletter";
        public static int CdepDeadLetterTTL => 10 * 60 * 1000; /*10 Min * 60 Seg * 1000 milisegundos = 10 minutos em milisegundos*/
    }
}
