using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoSolicitacaoItemCompleto
    {
        public long Id { get; set; }
        public TipoAcervo TipoAcervo { get; set; }
        public long AcervoId { get; set; }
        public string Titulo  { get; set; }
        public IEnumerable<CreditoAutorNomeAcervoId> AutoresCreditos { get; set; }
        public SituacaoSolicitacaoItem SituacaoItem { get; set; }
        public IEnumerable<ArquivoCodigoNomeAcervoId> Arquivos  { get; set; }
        public TipoAtendimento? TipoAtendimento  { get; set; }
        public DateTime? DataVisita  { get; set; }
        public SituacaoSolicitacao Situacao { get; set; }
        public SituacaoSaldo SituacaoSaldo { get; set; }
        public long? AcervoSolicitacaoId { get; set; }
        
        public SituacaoEmprestimo? SituacaoEmprestimo { get; set; }
        public bool TemControleDisponibilidade { get; set; }
        public string SituacaoDisponibilidade { get; set; }

        public string SituacaoSaldoDescricao()
        {
            return SituacaoSaldo switch
            {
                SituacaoSaldo.DISPONIVEL => Dominio.Constantes.Constantes.ACERVO_DISPONIVEL,
                SituacaoSaldo.RESERVADO => string.Format(MensagemNegocio.ACERVO_RESERVADO, AcervoSolicitacaoId),
                SituacaoSaldo.EMPRESTADO => string.Format(MensagemNegocio.ACERVO_EMPRESTADO, AcervoSolicitacaoId),
                SituacaoSaldo.INDISPONIVEL_PARA_RESERVA_EMPRESTIMO => MensagemNegocio.ACERVO_INDISPONIVEL,
                _ => string.Empty
            };
        }
    }
    
    public class CreditoAutorNomeAcervoId 
    {
        public long AcervoId  { get; set; }
        public string Nome  { get; set; }
    }
    
    public class ArquivoCodigoNomeAcervoId
    {
        public long AcervoId  { get; set; }
        public Guid Codigo { get; set; }
        public string Nome  { get; set; }
    }
}
