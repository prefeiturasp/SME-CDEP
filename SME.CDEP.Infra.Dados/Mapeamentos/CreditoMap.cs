using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class CreditoMap : BaseMapAuditavel<Credito>
    {
        public CreditoMap()
        {
            ToTable("credito");
            Map(c => c.Nome).ToColumn("nome");
        }
    }
}