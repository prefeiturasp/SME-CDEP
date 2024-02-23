namespace SME.CDEP.Infra.Servicos.Mensageria.Exchange
{
    public static class ExchangeRabbit
    {
        public static string Logs = "EnterpriseApplicationLog";
        public static string QueueLogs => "EnterpriseQueueLog";
        public static string CDEP => "sme.cdep.workers";
        public static string CDEPDeadLetter => "sme.cdep.workers.deadletter";
        public static int SgpDeadLetterTTL => 10 * 60 * 1000; /*10 Min * 60 Seg * 1000 milisegundos = 10 minutos em milisegundos*/
        public static int SgpDeadLetterTTL_3 => 3 * 60 * 1000; /*3 Min * 60 Seg * 1000 milisegundos = 3 minutos em milisegundos*/
        public static int SgpDeadLetterTTL_1 => 1 * 60 * 1000; /*1 Min * 60 Seg * 1000 milisegundos = 1 minuto em milisegundos*/
    }
}
