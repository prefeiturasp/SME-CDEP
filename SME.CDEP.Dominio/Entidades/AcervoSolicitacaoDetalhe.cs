using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoSolicitacaoDetalhe
    {
        public long Id { get; set; }
        public long UsuarioId { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public string Responsavel { get; set; }
        public SituacaoSolicitacao Situacao { get; set; }
        public IEnumerable<AcervoSolicitacaoItemDetalheResumido> Itens { get; set; } = Enumerable.Empty<AcervoSolicitacaoItemDetalheResumido>();
    }
}
