namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoFotograficoDetalheDTO : AcervoDetalheDTO
{
    public string CreditosAutores { get; set; }
    public string DataAcervo { get; set; }
    public string Localizacao { get; set; }
    public string Procedencia { get; set; }
    public string CopiaDigital { get; set; }
    public string PermiteUsoImagem { get; set; }
    public string Conservacao { get; set; }
    public long Quantidade { get; set; }
    public string Dimensoes { get; set; }
    public string Suporte { get; set; }
    public string Formato { get; set; }
    public string TamanhoArquivo { get; set; }
    public string Cromia { get; set; }
    public string Resolucao { get; set; }
    public ImagemDTO[] Imagens { get; set; }
}