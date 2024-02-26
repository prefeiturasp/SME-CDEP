using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoSolicitacaoItemDetalhe
    {
        public long Id { get; set; }
        public long AcervoSolicitacaoId { get; set; }
        public TipoAcervo TipoAcervo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataVisita { get; set; }
        public string Solicitante { get; set; }
        public string Responsavel { get; set; }
        public string Titulo { get; set; }
        public string Codigo { get; set; }
        public string codigoNovo { get; set; }
        public SituacaoSolicitacaoItem Situacao { get; set; }
    }
}
