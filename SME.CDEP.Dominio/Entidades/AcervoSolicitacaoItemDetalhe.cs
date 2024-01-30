using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoSolicitacaoItemDetalhe
    {
        public long Id { get; set; }
        public long AcervoSolicitacaoId { get; set; }
        public TipoAcervo TipoAcervo { get; set; }
        public DateTime dataCriacao { get; set; }
        public DateTime? DataVisita { get; set; }
        public string Solicitante { get; set; }
        public SituacaoSolicitacaoItem Situacao { get; set; }
    }
}
