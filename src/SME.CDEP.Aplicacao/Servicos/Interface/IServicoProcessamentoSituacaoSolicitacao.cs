using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoProcessamentoSituacaoSolicitacao
    {
        Task AtualizarSituacaoGeralSolicitacaoAsync(AcervoSolicitacao acervoSolicitacao, bool todosItensEstaoCancelados = false);
    }
}