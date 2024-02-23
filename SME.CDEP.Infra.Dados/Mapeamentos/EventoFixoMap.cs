namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class EventoFixoMap : BaseMapAuditavel<CDEP.Dominio.Entidades.EventoFixo>
    {
        public EventoFixoMap()
        {
            ToTable("evento_fixo");
            Map(c => c.Data).ToColumn("data");
            Map(c => c.Tipo).ToColumn("tipo");
            Map(c => c.Descricao).ToColumn("descricao");
        }
    }
}