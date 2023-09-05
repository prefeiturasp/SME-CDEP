using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoFotograficoCadastroDTO
{
    [Required(ErrorMessage = "É necessário informar o título do acervo fotográfico")]
    [MaxLength(500, ErrorMessage = "A localização do acervo fotográfico não pode conter mais que 500 caracteres")]
    public string Titulo { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o código do acervo fotográfico")]
    [MaxLength(15, ErrorMessage = "A localização do acervo fotográfico não pode conter mais que 15 caracteres")]
    public string Codigo { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o crédito ou autoria do acervo fotográfico")]
    public long CreditoAutorId { get; set; }
    
    [MaxLength(100, ErrorMessage = "A localização do acervo fotográfico não pode conter mais que 100 caracteres")]
    public string? Localizacao { get; set; }
    
    [Required(ErrorMessage = "É necessário informar a procedência do acervo fotográfico")]
    [MaxLength(200, ErrorMessage = "A localização do acervo fotográfico não pode conter mais que 200 caracteres")]
    public string Procedencia { get; set; }
    
    [Required(ErrorMessage = "É necessário informar a data do acervo fotográfico")]
    public string DataAcervo { get; set; }
    
    public bool CopiaDigital { get; set; }
    public bool PermiteUsoImagem { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador da conservação do acervo fotografico")]
    public long ConservacaoId { get; set; }
    
    [Required(ErrorMessage = "É necessário informar a descrição do acervo fotografico")]
    public string Descricao { get; set; }
    
    [Required(ErrorMessage = "É necessário informar a quantidade do acervo fotografico")]
    public long Quantidade { get; set; }
    
    public float? Largura { get; set; }
    public float? Altura { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador do suporte do acervo fotografico")]
    public long SuporteId { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador do formato do acervo fotografico")]
    public long FormatoId { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o tamanho do arquivo do acervo fotografico")]
    [MaxLength(15, ErrorMessage = "A localização do acervo fotográfico não pode conter mais que 15 caracteres")]
    public string TamanhoArquivo { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador da cromia do acervo fotografico")]
    public long CromiaId { get; set; }
    
    [Required(ErrorMessage = "É necessário informar a resolução do acervo fotografico")]
    [MaxLength(15, ErrorMessage = "A localização do acervo fotográfico não pode conter mais que 15 caracteres")]
    public string Resolucao { get; set; }
    
    public string[]? Arquivos { get; set; }
}