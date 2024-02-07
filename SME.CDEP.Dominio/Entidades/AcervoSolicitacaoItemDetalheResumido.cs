using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoSolicitacaoItemDetalheResumido
    {
        public long Id { get; set; }
        public string Codigo { get; set; }
        public TipoAcervo TipoAcervo { get; set; }
        public string Titulo { get; set; }
        public DateTime? DataVisita { get; set; }
        public SituacaoSolicitacaoItem Situacao { get; set; }
    }
}
