using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class HistoricoConsultaAcervoDto
    {
        public long Id { get; set; }
        public string? TermoPesquisado { get; set; }
        public short? AnoInicial { get; set; }
        public short? AnoFinal { get; set; }
        public TipoAcervo? TipoAcervo { get; set; }
        public DateTime DataConsulta { get; set; }
        public int QuantidadeResultados { get; set; }
    }
}