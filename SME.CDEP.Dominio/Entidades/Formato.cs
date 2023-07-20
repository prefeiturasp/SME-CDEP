using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class Formato : EntidadeBase
    {
        public string Nome { get; set; }
        public TipoFormato Tipo { get; set; }
    }
}
