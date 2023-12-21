namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoBibliograficoDetalheDTO : AcervoDetalheDTO
{
    public AcervoBibliograficoDetalheDTO()
    {}
    public string Material { get; set; }
    public string Editora { get; set; }
    public string Assuntos { get; set; }
    public string Edicao { get; set; }
    public int NumeroPagina { get; set; }
    public string Dimensoes { get; set; }
    public string SerieColecao { get; set; }
    public string Volume { get; set; }
    public string Idioma { get; set; }
    public string LocalizacaoCDD { get; set; }
    public string LocalizacaoPHA { get; set; }
    public string NotasGerais { get; set; }
    public string Isbn { get; set; }
}