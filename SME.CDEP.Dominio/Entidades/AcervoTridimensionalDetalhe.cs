namespace SME.CDEP.Dominio.Entidades;

public class AcervoTridimensionalDetalhe
{
    public long Id { get; set; }
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public string Codigo { get; set; }
    public string Procedencia { get; set; }
    public int Ano { get; set; }
    public string DataAcervo { get; set; }
    public string Conservacao { get; set; }
    public long Quantidade { get; set; }
    public string Descricao { get; set; }
    public double? Largura { get; set; }
    public double? Altura { get; set; }
    public double? Profundidade { get; set; }
    public double? Diametro { get; set; }
    public IEnumerable<ImagemDetalhe> Imagens { get; set; }
}