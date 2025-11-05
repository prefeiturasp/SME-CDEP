using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class CreditoAutorMap : BaseMapAuditavel<CreditoAutor>
    {
        public CreditoAutorMap()
        {
            ToTable("credito_autor");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Tipo).ToColumn("tipo");
        }
    }
}