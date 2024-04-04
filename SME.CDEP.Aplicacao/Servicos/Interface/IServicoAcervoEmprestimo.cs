using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoEmprestimo : IServicoAplicacao
    {
        Task<IEnumerable<SituacaoItemDTO>> ObterSituacoesEmprestimo();
        Task<bool> ProrrogarEmprestimo(AcervoEmprestimoProrrogacaoDTO acervoEmprestimoProrrogacaoDTO);
        Task<bool> DevolverItemEmprestado(long acervoSolicitacaoItemId);
        Task NotificarVencimentoEmprestimo();
    }
}
