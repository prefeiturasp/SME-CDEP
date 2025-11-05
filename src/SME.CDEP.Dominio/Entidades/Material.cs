using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class Material : EntidadeBase
    {
        public string Nome { get; set; }
        public TipoMaterial Tipo { get; set; }
    }
}
