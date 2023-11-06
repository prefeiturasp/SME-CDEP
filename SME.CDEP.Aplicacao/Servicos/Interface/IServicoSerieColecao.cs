using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoSerieColecao : IServicoAplicacao,IServicoIdNomeExcluidoAuditavel
    {
        Task<long> ObterPorNome(string nome);
    }
}
