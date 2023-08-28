using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class CreditoAutor : EntidadeBaseAuditavel
    {
        public string Nome { get; set; }
        public TipoCreditoAutoria Tipo { get; set; }
    }
}
