using SME.CDEP.Dominio.Extensions;

namespace SME.CDEP.Dominio.Entidades;

public class AcervoTridimensionalDetalhe
{
    public long Id { get; set; }
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public string Codigo { get; set; }
    public string Procedencia { get; set; }
    public string Ano { get; set; }
    public string DataAcervo { get; set; }
    public string Conservacao { get; set; }
    public int Quantidade { get; set; }
    public string Descricao { get; set; }
    public string? Largura { get; set; }
    public string? Altura { get; set; }
    public string? Profundidade { get; set; }
    public string? Diametro { get; set; }
    public IEnumerable<ImagemDetalhe> Imagens { get; set; }
    public string Dimensoes
    {
        get
        {
            var dimensoes = string.Empty;

            if (Largura.PossuiElementos())
                dimensoes = $"{Largura}(Largura)";

            if (Altura.PossuiElementos())
                dimensoes += dimensoes.EstaPreenchido() ? $" x {Altura}(Altura)" : $"{Altura}(Altura)";
            
            if (Profundidade.PossuiElementos())
                dimensoes += dimensoes.EstaPreenchido() ? $" x {Profundidade}(Profundidade)" : $"{Profundidade}(Profundidade)";
            
            if (Diametro.PossuiElementos())
                dimensoes += dimensoes.EstaPreenchido() ? $" x {Diametro}(Diâmetro)" : $"{Diametro}(Diâmetro)";

            return dimensoes;
        }
    }
}