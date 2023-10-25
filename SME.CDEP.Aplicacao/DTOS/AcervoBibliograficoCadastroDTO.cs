using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoBibliograficoCadastroDTO : AcervoCadastroDTO
{
    [Required(ErrorMessage = "É necessário informar o identificador do material do acervo bibliográfico")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador do material do acervo bibliográfico deve ser maior que zero")]
    public long MaterialId { get; set; }
    
    public long? EditoraId { get; set; }
    
    public long[] AssuntosIds { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o ano do acervo bibliográfico")]
    [MaxLength(15, ErrorMessage = "O ano do acervo bibliográfico não pode conter mais que 15 caracteres")]
    public string Ano { get; set; }
    
    public string? Edicao { get; set; }
    public double? NumeroPagina { get; set; }
    public double? Largura { get; set; }
    public double? Altura { get; set; }
    public long? SerieColecaoId { get; set; }
    public string? Volume { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador do idioma do acervo bibliográfico")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador do idioma do acervo bibliográfico deve ser maior que zero")]
    public long IdiomaId { get; set; }
    
    [Required(ErrorMessage = "É necessário informar a localizção CDD do acervo bibliográfico")]
    [MaxLength(50, ErrorMessage = "A localizção CDD do acervo bibliográfico não pode conter mais que 50 caracteres")]
    public string LocalizacaoCDD { get; set; }
    
    [Required(ErrorMessage = "É necessário informar a localizção PHA do acervo bibliográfico")]
    [MaxLength(50, ErrorMessage = "A localizção PHA do acervo bibliográfico não pode conter mais que 50 caracteres")]
    public string LocalizacaoPHA { get; set; }
    
    public string? NotasGerais { get; set; }
    public string? Isbn { get; set; }
}