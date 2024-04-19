namespace SME.CDEP.Infra.Servicos.Mensageria
{
    public interface IServicoMensageriaLogs
    {
        Task Enviar(string mensagem, string rota, string exchange);
    }
}
