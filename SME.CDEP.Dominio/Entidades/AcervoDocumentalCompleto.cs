namespace SME.CDEP.Dominio.Entidades;

public class AcervoDocumentalCompleto : EntidadeBaseAuditavel
{
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public long TipoAcervoId { get; set; }
    public string Codigo { get; set; }
    public string CodigoNovo { get; set; }
    public long CreditoAutorId { get; set; }
    public string CreditoAutorNome { get; set; }
    public long? MaterialId { get; set; }
    public string MaterialNome { get; set; }
    public long IdiomaId { get; set; }
    public string IdiomaNome { get; set; }
    public string Ano { get; set; }
    public string NumeroPagina { get; set; }
    public string Volume { get; set; }
    public string TipoAnexo { get; set; }
    public double? Largura { get; set; }
    public double? Altura { get; set; }
    public string TamanhoArquivo { get; set; }
    public string Localizacao { get; set; }
    public bool? CopiaDigital { get; set; }
    public long? ConservacaoId { get; set; }
    public string ConservacaoNome { get; set; }
    public string Descricao { get; set; }
    public ArquivoResumido[]? Arquivos  { get; set; }
    public long? ArquivoId { get; set; }
    public string ArquivoNome { get; set; }
    public Guid ArquivoCodigo { get; set; }
    public string ArquivoNomeMiniatura { get; set; }
    public Guid ArquivoCodigoMiniatura { get; set; }
    public long[] CreditosAutoresIds { get; set; }
    public long[] AcessoDocumentosIds  { get; set; }
    public long AcessoDocumentoId { get; set; }
    public string CreditosAutores { get; set; }
    public string AcessoDocumentos { get; set; }
    public string AcessoDocumentoNome { get; set; }
    public IEnumerable<ImagemDetalhe>? Imagens { get; set; }
}