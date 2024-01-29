namespace SME.CDEP.Dominio.Entidades;

public class AcervoBibliograficoCompleto : EntidadeBaseAuditavel
{
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public string SubTitulo { get; set; }
    public long TipoAcervoId { get; set; }
    public string Codigo { get; set; }
    public long? MaterialId { get; set; }
    public long? EditoraId { get; set; }
    public string Ano { get; set; }
    public string Edicao { get; set; }
    public int? NumeroPagina { get; set; }
    public string? Largura { get; set; }
    public string? Altura { get; set; }
    public long? SerieColecaoId { get; set; }
    public string Volume { get; set; }
    public long IdiomaId { get; set; }
    public string Localizacaocdd { get; set; }
    public string Localizacaopha { get; set; }
    public string NotasGerais { get; set; }
    public string Isbn { get; set; }
    public long[] CreditosAutoresIds { get; set; }
    public CoAutor[]? CoAutores { get; set; }
    public long[] AssuntosIds { get; set; }
}