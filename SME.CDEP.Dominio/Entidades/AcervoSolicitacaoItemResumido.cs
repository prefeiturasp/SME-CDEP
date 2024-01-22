using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoSolicitacaoItemResumido
    {
        public TipoAcervo TipoAcervo { get; set; }
        public long AcervoId { get; set; }
        public string Titulo  { get; set; }
    }
}
