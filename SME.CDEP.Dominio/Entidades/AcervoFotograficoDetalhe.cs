namespace SME.CDEP.Dominio.Entidades;

public class AcervoFotograficoDetalhe
{
    public long Id { get; set; }
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public string Codigo { get; set; }
    public string Localizacao { get; set; }
    public string Procedencia { get; set; }
    public string Ano { get; set; }
    public string DataAcervo { get; set; }
    public bool? CopiaDigital { get; set; }
    public bool? PermiteUsoImagem { get; set; }
    public string Conservacao { get; set; }
    public string Descricao { get; set; }
    public int Quantidade { get; set; }
    public string? Largura { get; set; }
    public string? Altura { get; set; }
    public string Suporte { get; set; }
    public string Formato { get; set; }
    public string TamanhoArquivo { get; set; }
    public string Cromia { get; set; }
    public string Resolucao { get; set; }
    public string Creditos { get; set; }
    public IEnumerable<ImagemDetalhe> Imagens { get; set; }
}