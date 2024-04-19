using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class EventoDetalheDTO
    {
        public long Id { get; set; }
        public TipoEvento TipoId { get; set; }
        public string Tipo { get; set; }
        public string Solicitante { get; set; }
        public string Titulo { get; set; }
        public string CodigoTombo { get; set; }
        public long? AcervoSolicitacaoId { get; set; }
        public string Descricao { get; set; }
        public string Justificativa { get; set; }
        public SituacaoSolicitacaoItem? SituacaoSolicitacaoItemId { get; set; }
        public string SituacaoSolicitacaoItemDescricao { get; set; }
        public string Horario { get; set; }
    }
}
