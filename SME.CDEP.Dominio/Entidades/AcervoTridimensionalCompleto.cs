namespace SME.CDEP.Dominio.Entidades;

public class AcervoTridimensionalCompleto: EntidadeBaseAuditavel
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
    public double? Largura { get; set; }
    public double? Altura { get; set; }
    public double? Profundidade { get; set; }
    public double? Diametro { get; set; }
    public ArquivoResumido[] Arquivos  { get; set; }
    public long ArquivoId { get; set; }
    public string ArquivoNome { get; set; }
    public Guid ArquivoCodigo { get; set; }
    public long[] CreditosAutoresIds { get; set; }
}