namespace SME.CDEP.Infra.Servicos.Mensageria
{
    public class MensagemRabbitLogs
    {
        public MensagemRabbitLogs(string mensagem)
        {
            Mensagem = mensagem;
        }

        public string Mensagem { get; }
    }
}
