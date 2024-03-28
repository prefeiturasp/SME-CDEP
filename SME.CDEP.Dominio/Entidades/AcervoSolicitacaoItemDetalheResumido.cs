using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Extensions;
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
            return SituacaoSaldo switch
            {
                SituacaoSaldo.DISPONIVEL => Constantes.Constantes.ACERVO_DISPONIVEL,
                SituacaoSaldo.RESERVADO => string.Format(MensagemNegocio.ACERVO_RESERVADO, acervoSolicitacaoId),
                SituacaoSaldo.EMPRESTADO => string.Format(MensagemNegocio.ACERVO_EMPRESTADO, acervoSolicitacaoId),
                SituacaoSaldo.INDISPONIVEL_PARA_RESERVA_EMPRESTIMO => MensagemNegocio.ACERVO_INDISPONIVEL,
            };
        }
        
        public bool PodeEditar()
        {
            if (TipoAcervo.EhAcervoBibliografico())
            {
                return Situacao.EstaAguardandoVisita()
                       || (Situacao.EstaAguardandoAtendimento() && SituacaoSaldo.EstaDisponivel())
                       || Situacao.EstaFinalizadoManualmente();
            }
            
            return Situacao.EstaAguardandoAtendimento()|| Situacao.EstaAguardandoVisita();
        }
    }
}
