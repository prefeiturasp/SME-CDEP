using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class PainelGerencialQuantidadeDeSolicitacoesPorTipoAcervo
    {
        public TipoAcervo TipoAcervo { get; set; }
        public int Quantidade { get; set; }
    }
}