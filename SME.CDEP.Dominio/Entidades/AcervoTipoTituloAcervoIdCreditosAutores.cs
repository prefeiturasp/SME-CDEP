using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoTipoTituloAcervoIdCreditosAutores
    {
        public TipoAcervo TipoAcervo { get; set; }
        public long AcervoId { get; set; }
        public string Titulo  { get; set; }
        public SituacaoSaldo SituacaoSaldo  { get; set; }
        public IEnumerable<CreditoAutorNomeAcervoId> AutoresCreditos { get; set; }
    }
}
