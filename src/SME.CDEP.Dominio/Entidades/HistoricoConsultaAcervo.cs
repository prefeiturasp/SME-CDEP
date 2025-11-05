using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class HistoricoConsultaAcervo : EntidadeBaseSomenteId
    {
        public string? TermoPesquisado { get; set; }
        public short? AnoInicial { get; set; }
        public short? AnoFinal { get; set; }
        public TipoAcervo? TipoAcervo { get; set; }
        public DateTime DataConsulta { get; set; } = DateTime.UtcNow;
        public int QuantidadeResultados { get; set; }
    }
}
