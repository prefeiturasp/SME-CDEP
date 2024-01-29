namespace SME.CDEP.Dominio.Entidades;

public class AcervoBibliograficoDetalhe
{
    public long Id { get; set; }
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public string SubTitulo { get; set; }
    public string Material { get; set; }
    public string Editora { get; set; }
    public string Ano { get; set; }
    public string Edicao { get; set; }
    public int? NumeroPagina { get; set; }
    public string? Largura { get; set; }
    public string? Altura { get; set; }
    public string SerieColecao { get; set; }
    public string Volume { get; set; }
    public string Idioma { get; set; }
    public string Localizacaocdd { get; set; }
    public string Localizacaopha { get; set; }
    public string NotasGerais { get; set; }
    public string Isbn { get; set; }
    public string Codigo { get; set; }
    public string Autores { get; set; }
    public string Assuntos { get; set; }
}