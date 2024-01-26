
namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoIdioma : IServicoIdNomeExcluido
    {
        Task<long> ObterPorNome(string nome);
    }
}
