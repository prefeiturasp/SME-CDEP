namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoBibliograficoDetalheDTO : AcervoDetalheDTO
{
    public string CreditosAutores { get; set; }
    public string SubTitulo { get; set; }
    public string Material { get; set; }
    public string Editora { get; set; }
    public string Assuntos { get; set; }
    public string Edicao { get; set; }
    public int NumeroPagina { get; set; }
    public string Dimensoes { get; set; }
    public string DimensoesLegendas { get; set; } = "L = largura | A = altura";
    public string SerieColecao { get; set; }
    public string Volume { get; set; }
    public string Idioma { get; set; }
    public string Localizacao { get; set; }
    public string NotasGerais { get; set; }
    public string Isbn { get; set; }
}