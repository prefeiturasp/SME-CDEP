using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoSolicitacaoItemCompleto : AcervoSolicitacaoItemResumido
    {
        public IEnumerable<CreditoAutorNomeAcervoId> AutoresCreditos { get; set; }
        public SituacaoSolicitacaoItem Situacao { get; set; }
        public IEnumerable<ArquivoCodigoNomeAcervoId> Arquivos  { get; set; }
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
