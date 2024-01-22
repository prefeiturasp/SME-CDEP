using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoSolicitacao : IServicoAplicacao
    {
        Task<long> Inserir(AcervoSolicitacaoDTO acervoSolicitacao);
        Task<AcervoSolicitacaoDTO> ObterPorId(long acervoSolicitacaoId);
        Task<IEnumerable<AcervoSolicitacaoDTO>> ObterTodosPorUsuario(int usuarioId);
        Task<AcervoSolicitacaoDTO> Alterar(AcervoSolicitacaoDTO acervoSolicitacao);
        Task<bool> Remover(long acervoSolicitacaoId);
    }
}
