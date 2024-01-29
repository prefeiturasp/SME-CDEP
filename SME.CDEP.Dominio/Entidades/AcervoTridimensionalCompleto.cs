namespace SME.CDEP.Dominio.Entidades;

public class AcervoTridimensionalCompleto: EntidadeBaseAuditavel
{
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public long TipoAcervoId { get; set; }
    public string Codigo { get; set; }
    public string Procedencia { get; set; }
    public int Ano { get; set; }
    public string DataAcervo { get; set; }
    public long ConservacaoId { get; set; }
    public int Quantidade { get; set; }
    public string Descricao { get; set; }
    public string? Largura { get; set; }
    public string? Altura { get; set; }
    public string? Profundidade { get; set; }
    public string? Diametro { get; set; }
    public ArquivoResumido[] Arquivos  { get; set; }
    public long[] CreditosAutoresIds { get; set; }
}