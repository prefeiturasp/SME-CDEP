namespace SME.CDEP.Dominio.Entidades;

public class AcervoTridimensionalCompleto: EntidadeBase
{
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public long TipoAcervoId { get; set; }
    public string Codigo { get; set; }
    public long CreditoAutorId { get; set; }
    public string CreditoAutorNome { get; set; }
    public string Procedencia { get; set; }
    public string DataAcervo { get; set; }
    public long ConservacaoId { get; set; }
    public long Quantidade { get; set; }
    public string Descricao { get; set; }
    public float? Largura { get; set; }
    public float? Altura { get; set; }
    public float? Profundidade { get; set; }
    public float? Diametro { get; set; }
    public DateTime? AlteradoEm { get; set; }
    public string? AlteradoPor { get; set; }
    public string? AlteradoLogin { get; set; }
    public DateTime CriadoEm { get; set; }
    public string CriadoPor { get; set; }
    public string CriadoLogin { get; set; }
    public ArquivoResumido[] Arquivos  { get; set; }
    public long ArquivoId { get; set; }
    public string ArquivoNome { get; set; }
    public Guid ArquivoCodigo { get; set; }
    public long[] CreditosAutoresIds { get; set; }
}