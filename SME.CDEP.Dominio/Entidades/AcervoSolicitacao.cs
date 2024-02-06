using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoSolicitacao : EntidadeBaseAuditavel
    {
        public long UsuarioId { get; set; }
        public long ResponsavelId { get; set; }
        public SituacaoSolicitacao Situacao { get; set; }
        public IEnumerable<AcervoSolicitacaoItem> Itens { get; set; }
    }
}
