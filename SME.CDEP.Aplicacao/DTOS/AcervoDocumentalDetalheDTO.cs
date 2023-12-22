namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoDocumentalDetalheDTO : AcervoDetalheDTO
{
    public string Descricao { get; set; }
    public string CreditosAutores { get; set; }
    public string CodigoNovo { get; set; }
    public string Material { get; set; }
    public string Idioma { get; set; }
    public string NumeroPagina { get; set; }
    public string Volume { get; set; }
    public string TipoAnexo { get; set; }
    public string Dimensoes { get; set; }
    public string TamanhoArquivo { get; set; }
    public string Localizacao { get; set; }
    public string CopiaDigital { get; set; }
    public string Conservacao { get; set; }
    public string AcessosDocumentos { get; set; }
    public ImagemDTO[] Imagens { get; set; }
}