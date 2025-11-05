namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoSerieColecao : IServicoIdNomeExcluidoAuditavel
    {
        Task<long> ObterPorNome(string nome);
    }
}
