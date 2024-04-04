using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoEmprestimoAntesVencimentoDevolucao
    {
        public long AcervoSolicitacaoId { get; set; }
        public long AcervoSolicitacaoItemId { get; set; }
        public string Solicitante { get; set; }
        public string Titulo { get; set; }
        public string Codigo { get; set; }
        public string Email { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucao { get; set; }
    }
}
