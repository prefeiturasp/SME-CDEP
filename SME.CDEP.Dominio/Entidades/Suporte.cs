using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class Suporte : EntidadeBaseSemAuditoria
    {
        public string Nome { get; set; }
        public TipoSuporte TipoSuporte { get; set; }
    }
}
