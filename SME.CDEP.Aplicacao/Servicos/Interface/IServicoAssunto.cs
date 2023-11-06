using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAssunto : IServicoAplicacao,IServicoIdNomeExcluidoAuditavel
    {
        Task<long> ObterPorNome(string nome);
    }
}