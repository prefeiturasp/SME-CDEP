using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoTridimensionalCadastroDTO : AcervoCadastroDTO
{
    [Required(ErrorMessage = "É necessário informar a procedência do acervo arte gráfica")]
    [MaxLength(200, ErrorMessage = "A procedência do acervo arte gráfica não pode conter mais que 200 caracteres")]
    public string Procedencia { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador da conservação do acervo arte gráfica")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador da conservação do acervo arte gráfica deve ser maior que zero")]
    public long ConservacaoId { get; set; }
    
    [Required(ErrorMessage = "É necessário informar a quantidade do acervo arte gráfica")]
    [Range(1, long.MaxValue, ErrorMessage = "A quantidade do acervo arte gráfica deve ser maior que zero")]
    public long Quantidade { get; set; }
    public double? Largura { get; set; }
    public double? Altura { get; set; }
    public double? Profundidade { get; set; }
    public double? Diametro { get; set; }
    
    public long[]? Arquivos { get; set; }
}