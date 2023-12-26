namespace SME.CDEP.Dominio.Entidades;

public class AcervoArteGraficaDetalhe
{
    public long Id { get; set; }
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public string Codigo { get; set; }
    public string Localizacao { get; set; }
    public string Procedencia { get; set; }
    public int Ano { get; set; }
    public string DataAcervo { get; set; }
    public bool CopiaDigital { get; set; }
    public bool PermiteUsoImagem { get; set; }
    public string Conservacao { get; set; }
    public string Cromia { get; set; }
    public double? Largura { get; set; }
    public double? Altura { get; set; }
    public double? Diametro { get; set; }
    public string Tecnica { get; set; }
    public string Suporte { get; set; }
    public long Quantidade { get; set; }
    public string Descricao { get; set; }
    public IEnumerable<ImagemDetalhe>? Imagens { get; set; }
    public string Creditos { get; set; }
}