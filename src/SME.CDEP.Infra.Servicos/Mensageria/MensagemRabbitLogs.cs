using System.Diagnostics.CodeAnalysis;

namespace SME.CDEP.Infra.Servicos.Mensageria
{
    [ExcludeFromCodeCoverage]
    public class MensagemRabbitLogs
    {
        public MensagemRabbitLogs(string mensagem)
        {
            Mensagem = mensagem;
        }

        public string Mensagem { get; }
    }
}
