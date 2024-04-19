using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoConservacao : IServicoIdNomeExcluido
    {
        Task<long> ObterPorNome(string nome);
    }
}
