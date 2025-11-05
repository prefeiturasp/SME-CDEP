namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoDeConsolidacao : IServicoAplicacao
    {
        Task ConsolidarMesDoHistoricoDeConsultasDoDiaAnteriorAsync();
    }
}
