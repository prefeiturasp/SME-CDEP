using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Contexto;

namespace SME.CDEP.Aplicacao.Servicos.Fachadas
{
    public record ContextoRegrasAcervoSolicitacao(
        IServicoAcervo ServicoAcervo,
        IServicoAcervoBibliografico ServicoAcervoBibliografico,
        IServicoConfirmacaoAtendimentoAcervo ServicoConfirmacao,
        IServicoEvento ServicoEvento,
        IContextoAplicacao ContextoAplicacao,
        IServicoProcessamentoSituacaoSolicitacao ServicoProcessamentoSituacao);
}
