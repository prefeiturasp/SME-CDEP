namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoTridimensionalDetalheDTO : AcervoDetalheDTO
{
    public string Procedencia { get; set; }
    public string Conservacao { get; set; }
    public long Quantidade { get; set; }
    public string Dimensoes { get; set; }
    public ImagemDTO[] Imagens { get; set; }
}