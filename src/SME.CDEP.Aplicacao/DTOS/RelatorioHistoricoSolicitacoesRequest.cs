using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public record RelatorioHistoricoSolicitacoesRequest(
    string? Solicitante,
    DateTime DataInicio,
    DateTime DataFim,
    List<TipoAcervo>? TipoAcervo,
    List<SituacaoSolicitacaoItem>? SituacaoSolicitacao);
}
