namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoSuporte : IServicoIdNomeTipoExcluido
    {
        Task<long> ObterPorNomeETipo(string nome, int tipoSuporte);
    }
}
