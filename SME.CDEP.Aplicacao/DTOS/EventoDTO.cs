using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class EventoDTO
    {
        public long Id { get; set; }
        public DateTime Data { get; set; }
        public TipoEvento Tipo { get; set; }
        public string Descricao { get; set; }
        public string Justificativa { get; set; }
        public long? AcervoSolicitacaoItemId { get; set; }
    }
}
