namespace SME.CDEP.Dominio.Entidades;

public class AcervoDocumentalDetalhe
{
    public long Id { get; set; }
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public string Codigo { get; set; }
    public string CodigoNovo { get; set; }
    public string Material { get; set; }
    public string Idioma { get; set; }
    public string Ano { get; set; }
    public int NumeroPagina { get; set; }
    public string Volume { get; set; }
    public string Descricao { get; set; }
    public string TipoAnexo { get; set; }
    public string? Largura { get; set; }
    public string? Altura { get; set; }
    public string TamanhoArquivo { get; set; }
    public string Localizacao { get; set; }
    public bool? CopiaDigital { get; set; }
    public string Conservacao { get; set; }
    public string Autores { get; set; }
    public string AcessosDocumentos { get; set; }
    public IEnumerable<ImagemDetalhe>? Imagens { get; set; }
}