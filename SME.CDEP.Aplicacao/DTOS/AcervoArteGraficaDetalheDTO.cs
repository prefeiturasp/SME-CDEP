
namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoArteGraficaDetalheDTO : AcervoDetalheDTO
{
    public string Descricao { get; set; }
    public string CreditosAutores { get; set; }
    public string DataAcervo { get; set; }
    public string Localizacao { get; set; }
    public string Procedencia { get; set; }
    public string CopiaDigital { get; set; }
    public string PermiteUsoImagem { get; set; }
    public string Conservacao { get; set; }
    public string Cromia { get; set; }
    public string Tecnica { get; set; }
    public string Suporte { get; set; }
    public long Quantidade { get; set; }
    public ImagemDTO[] Imagens { get; set; }
    public string Dimensoes { get; set; }
    public string DimensoesLegendas { get; set; } = "L = largura | A = altura | D = diâmetro";
}