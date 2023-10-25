using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoCreditoAutor : EntidadeBaseSomenteId
    {
        public long AcervoId { get; set; }
        public long CreditoAutorId { get; set; }
        public string TipoAutoria { get; set; }
    }
}