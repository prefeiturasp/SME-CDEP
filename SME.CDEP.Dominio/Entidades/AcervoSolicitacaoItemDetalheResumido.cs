using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoSolicitacaoItemDetalheResumido
    {
        public long Id { get; set; }
        public string Codigo { get; set; }
        public TipoAcervo TipoAcervo { get; set; }
        public string Titulo { get; set; }
        public DateTime? DataVisita { get; set; }
        public SituacaoSolicitacaoItem Situacao { get; set; }
        public TipoAtendimento? TipoAtendimento { get; set; }
        public long AcervoId { get; set; }
        public string Responsavel { get; set; }
        public DateTime? DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public SituacaoEmprestimo? SituacaoEmprestimo { get; set; }
        public SituacaoSaldo SituacaoSaldo { get; set; }
        public long acervoSolicitacaoId { get; set; }

        public string SituacaoSaldoDescricao()
        {
            return SituacaoSaldo.EstaDisponivel() 
                ? Constantes.Constantes.ACERVO_DISPONIVEL 
                : SituacaoSaldo.EstaReservado() 
                    ? $"Acervo reservado pela solicitação '{acervoSolicitacaoId}'" 
                    : $"Acervo emprestado pela solicitação '{acervoSolicitacaoId}'";
        }
    }
}
