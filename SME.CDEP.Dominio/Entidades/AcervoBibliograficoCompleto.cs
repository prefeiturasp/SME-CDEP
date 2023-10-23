namespace SME.CDEP.Dominio.Entidades;

public class AcervoBibliograficoCompleto : EntidadeBase
{
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public string SubTitulo { get; set; }
    public long TipoAcervoId { get; set; }
    public string Codigo { get; set; }
    public long CreditoAutorId { get; set; }
    public string CreditoAutorNome { get; set; }
    public long CreditoCoAutorId { get; set; }
    public string CreditoCoAutorNome { get; set; }
    public long? MaterialId { get; set; }
    public string MaterialNome { get; set; }
    public long EditoraId { get; set; }
    public string EditoraNome { get; set; }
    public string Ano { get; set; }
    public string Edicao { get; set; }
    public float? NumeroPagina { get; set; }
    public float? Largura { get; set; }
    public float? Altura { get; set; }
    public long SerieColecaoId { get; set; }
    public string SerieColecaoNome { get; set; }
    public string Volume { get; set; }
    public long IdiomaId { get; set; }
    public string IdiomaNome { get; set; }
    public string Localizacaocdd { get; set; }
    public string Localizacaopha { get; set; }
    public string NotasGerais { get; set; }
    public string Isbn { get; set; }
    public long[] CreditosAutoresIds { get; set; }
    public long[] CreditosCoAutores { get; set; } //Aqui passa o tipo
}