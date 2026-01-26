using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoManutencaoSolicitacaoAcervo
    {
        Task<long> Inserir(AcervoSolicitacaoManualDTO dto);
        Task<long> Alterar(AcervoSolicitacaoManualDTO dto);
    }
}
