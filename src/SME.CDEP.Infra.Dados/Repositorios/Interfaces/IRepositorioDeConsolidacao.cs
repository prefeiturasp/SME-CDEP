namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioDeConsolidacao
    {
        Task ConsolidarMesDoHistoricoDeConsultasAsync(DateTime inicio, DateTime fim);
        Task ConsolidarMesDasSolicitacoesDeAcervosAsync(DateTime inicio, DateTime fim);
    }
}