using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoCromia : IServicoIdNomeExcluido
    {
        Task<long> ObterPorNome(string nome);
    }
}
