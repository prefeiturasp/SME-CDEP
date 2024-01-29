using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoDocumentalCadastroDTO : AcervoCadastroDTO
{
    public long? MaterialId { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador do idioma do acervo documental")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador do idioma do acervo documental deve ser maior que zero")]
    public long IdiomaId { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o número de página do acervo documental")]
    [MaxLength(4, ErrorMessage = "O número de página do acervo documental não pode conter mais que 4 caracteres")]
    public string NumeroPagina { get; set; }
    
    [MaxLength(15, ErrorMessage = "O volume do acervo documental não pode conter mais que 15 caracteres")]
    public string? Volume { get; set; }
    
    [MaxLength(50, ErrorMessage = "O tipo anexo do acervo documental não pode conter mais que 50 caracteres")]
    public string? TipoAnexo { get; set; }
    
    public string? Largura { get; set; }
    public string? Altura { get; set; }
    
    [MaxLength(15, ErrorMessage = "O tamanho do arquivo do acervo documental não pode conter mais que 15 caracteres")]
    public string? TamanhoArquivo { get; set; }
    
    [MaxLength(100, ErrorMessage = "A localização do acervo documental não pode conter mais que 100 caracteres")]
    public string? Localizacao { get; set; }
    
    public bool? CopiaDigital { get; set; }
    
    public long? ConservacaoId { get; set; }
    
    public long[]? Arquivos { get; set; }
    
    public long[] AcessoDocumentosIds { get; set; }
}