using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class PainelGerencialQuantidadeAcervoEmprestadoPorSituacao
    {
        public SituacaoEmprestimo Situacao { get; set; }
        public int Quantidade { get; set; }
    }
}