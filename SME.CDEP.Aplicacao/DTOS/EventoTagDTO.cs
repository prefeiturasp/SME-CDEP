
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class EventoTagDTO
    {
        public string TipoId { get; set; }
        public TipoEvento Tipo { get; set; }
    }
}
