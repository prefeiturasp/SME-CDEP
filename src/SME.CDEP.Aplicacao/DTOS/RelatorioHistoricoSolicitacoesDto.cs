using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public record RelatorioHistoricoSolicitacoesDto(
    RelatorioHistoricoSolicitacoesRequest BaseRequest,
    string Usuario,
    string UsuarioRF)
    {
        public string? Solicitante => BaseRequest.Solicitante;
        public DateTime DataInicio => BaseRequest.DataInicio;
        public DateTime DataFim => BaseRequest.DataFim;
        public List<TipoAcervo>? TipoAcervo => BaseRequest.TipoAcervo;
        public List<SituacaoSolicitacaoItem>? SituacaoSolicitacao => BaseRequest.SituacaoSolicitacao;
    }
}
