namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class EventoMap : BaseMapAuditavel<CDEP.Dominio.Entidades.Evento>
    {
        public EventoMap()
        {
            ToTable("evento");
            Map(c => c.Data).ToColumn("data");
            Map(c => c.Tipo).ToColumn("tipo");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.Justificativa).ToColumn("justificativa");
            Map(c => c.AcervoSolicitacaoItemId).ToColumn("acervo_solicitacao_item_id");
        }
    }
}