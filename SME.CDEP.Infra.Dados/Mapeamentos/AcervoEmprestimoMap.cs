namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AcervoEmprestimoMap : BaseMapAuditavel<CDEP.Dominio.Entidades.AcervoEmprestimo>
    {
        public AcervoEmprestimoMap()
        {
            ToTable("acervo_emprestimo");
            Map(c => c.AcervoSolicitacaoItemId).ToColumn("acervo_solicitacao_item_id");
            Map(c => c.DataEmprestimo).ToColumn("dt_emprestimo");
            Map(c => c.DataDevolucao).ToColumn("dt_devolucao");
            Map(c => c.Situacao).ToColumn("situacao");
        }
    }
}