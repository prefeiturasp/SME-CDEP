using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoSolicitacao : EntidadeBaseAuditavel
    {
        public long UsuarioId { get; set; }
        public SituacaoSolicitacao Situacao { get; set; }
        public IEnumerable<AcervoSolicitacaoItem> Itens { get; set; }
        public Origem Origem { get; set; } = Origem.Portal;
        public DateTime DataSolicitacao { get; set; }
    }
}
