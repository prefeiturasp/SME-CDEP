namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcessoDocumento : IServicoIdNomeExcluido
    {
        Task<long> ObterPorNome(string nome);
    }
}
