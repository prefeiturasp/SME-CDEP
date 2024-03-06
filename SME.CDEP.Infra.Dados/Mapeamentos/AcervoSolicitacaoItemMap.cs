namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AcervoSolicitacaoItemMap : BaseMapAuditavel<CDEP.Dominio.Entidades.AcervoSolicitacaoItem>
    {
        public AcervoSolicitacaoItemMap()
        {
            ToTable("acervo_solicitacao_item");
            Map(c => c.AcervoSolicitacaoId).ToColumn("acervo_solicitacao_id");
            Map(c => c.AcervoId).ToColumn("acervo_id");
            Map(c => c.DataVisita).ToColumn("dt_visita");
            Map(c => c.Situacao).ToColumn("situacao");
            Map(c => c.TipoAtendimento).ToColumn("tipo_atendimento");
            Map(c => c.ResponsavelId).ToColumn("usuario_responsavel_id");
        }
    }
}