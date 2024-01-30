namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoTridimensionalDetalheDTO : AcervoDetalheDTO
{
    public string Descricao { get; set; }
    public string DataAcervo { get; set; }
    public string Procedencia { get; set; }
    public string Conservacao { get; set; }
    public long Quantidade { get; set; }
    public string Dimensoes { get; set; }
    public string DimensoesLegendas { get; set; } = "L = largura | A = altura | P = Profundidade | D = diâmetro";
    public ImagemDTO[] Imagens { get; set; }
}