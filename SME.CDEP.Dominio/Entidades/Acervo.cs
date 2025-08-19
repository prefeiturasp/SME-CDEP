using SME.CDEP.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class Acervo : EntidadeBaseAuditavel
    {
        public string Titulo { get; set; }
        public long TipoAcervoId { get; set; }
        public string Editora { get; set; }
        public string Codigo { get; set; }
        public CreditoAutor? CreditoAutor { get; set; }
        public List<CreditoAutor> CreditosAutores { get; set; }
        public long[]? CreditosAutoresIds { get; set; }
        public string CodigoNovo { get; set; }
        public string SubTitulo { get; set; }
        public string Descricao { get; set; }
        public string Ano { get; set; }
        public string DataAcervo { get; set; }
        public IEnumerable<CoAutor>? CoAutores { get; set; }
        public int AnoInicio { get; set; }
        public int AnoFim { get; set; }
        public string? CapaDocumento { get; set; }
        public SituacaoAcervo SituacaoAcervo { get; set; }
    }
}
