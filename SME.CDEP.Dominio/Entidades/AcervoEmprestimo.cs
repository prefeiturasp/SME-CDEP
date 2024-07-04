using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoEmprestimo : EntidadeBaseAuditavel
    {
        public long AcervoSolicitacaoItemId { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucao { get; set; }
        public SituacaoEmprestimo Situacao { get; set; }

        public void DefinirDevoluvaoEmAtraso()
        {
            Id = 0;
            Situacao = SituacaoEmprestimo.DEVOLUCAO_EM_ATRASO;
        }
    }
}
