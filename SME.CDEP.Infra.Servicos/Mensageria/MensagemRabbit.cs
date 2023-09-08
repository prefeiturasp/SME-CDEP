namespace SME.CDEP.Infra.Servicos.Mensageria
{
    public class MensagemRabbit
    {
        public MensagemRabbit(string mensagem)
        {
            Mensagem = mensagem;
        }

        public string Mensagem { get; }
    }
}
