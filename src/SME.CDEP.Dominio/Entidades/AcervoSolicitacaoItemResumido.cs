using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoSolicitacaoItemResumido
    {
        public long AcervoSolicitacaoId { get; set; }
        public TipoAcervo TipoAcervo { get; set; }
        public string Titulo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataVisita { get; set; }
        public SituacaoSolicitacaoItem Situacao { get; set; }
    }
}
