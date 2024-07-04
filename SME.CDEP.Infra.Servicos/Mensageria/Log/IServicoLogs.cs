using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Servicos.Mensageria.Log
{
    public interface IServicoLogs 
    {
        Task Enviar(string mensagem, LogContexto contexto = LogContexto.Geral, LogNivel nivel = LogNivel.Critico, string observacao = "", string rastreamento = "");
    }
}
