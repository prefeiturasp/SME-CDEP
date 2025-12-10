using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class PainelGerencialQuantidadeSolicitacaoPorSituacao
    {
        public SituacaoSolicitacaoItem Situacao { get; set; }
        public int Quantidade { get; set; }
    }
}