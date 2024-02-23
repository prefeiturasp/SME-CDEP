using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class EventoFixo : EntidadeBaseAuditavel
    {
        public DateTime Data { get; set; }
        public TipoEvento Tipo { get; set; }
        public string Descricao { get; set; }
    }
}
