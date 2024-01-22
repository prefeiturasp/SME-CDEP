using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class AcervoSolicitacaoItemDTO
    {
        public long Id { get; set; }
        public long AcervoSolicitacaoId { get; set; }
        public long AcervoId { get; set; }
        public SituacaoSolicitacaoItem Situacao { get; set; }
        public DateTime? DataVisita { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoLogin { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoLogin { get; set; }
    }
}
