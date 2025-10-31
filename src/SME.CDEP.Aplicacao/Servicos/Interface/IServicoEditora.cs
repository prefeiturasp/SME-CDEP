namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoEditora : IServicoIdNomeExcluidoAuditavel
    {
        Task<long> ObterPorNome(string nome);
    }
}
