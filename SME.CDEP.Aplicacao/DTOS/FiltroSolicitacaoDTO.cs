using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class FiltroSolicitacaoDTO
{
    public long? AcervoSolicitacaoId { get; set; }
    public TipoAcervo? TipoAcervo { get; set; }
    public DateTime? DataSolicitacaoInicio { get; set; }
    public DateTime? DataSolicitacaoFim { get; set; }
    public DateTime? DataVisitaInicio { get; set; }
    public DateTime? DataVisitaFim { get; set; }
    public string? Responsavel { get; set; }
    public SituacaoSolicitacaoItem? SituacaoItem { get; set; }
}