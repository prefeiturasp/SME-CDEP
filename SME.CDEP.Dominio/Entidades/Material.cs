using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class Material : EntidadeBaseSemAuditoria
    {
        public string Nome { get; set; }
        public TipoMaterial TipoMaterial { get; set; }
    }
}
