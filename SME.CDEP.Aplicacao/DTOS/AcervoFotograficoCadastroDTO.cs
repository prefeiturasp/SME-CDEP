using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoFotograficoCadastroDTO : AcervoCadastroDTO
{
    [MaxLength(100, ErrorMessage = "A localização do acervo fotográfico não pode conter mais que 100 caracteres")]
    public string? Localizacao { get; set; }
    
    [Required(ErrorMessage = "É necessário informar a procedência do acervo fotográfico")]
    [MaxLength(200, ErrorMessage = "A procedência do acervo fotográfico não pode conter mais que 200 caracteres")]
    public string Procedencia { get; set; }
    
    [Required(ErrorMessage = "É necessário informar a data do acervo fotográfico")]
    [MaxLength(50, ErrorMessage = "A data do acervo fotográfico não pode conter mais que 50 caracteres")]
    public string DataAcervo { get; set; }
    
    public bool CopiaDigital { get; set; }
    public bool PermiteUsoImagem { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador da conservação do acervo fotografico")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador da conservação do acervo fotográfico deve ser maior que zero")]
    public long ConservacaoId { get; set; }
    
    [Required(ErrorMessage = "É necessário informar a descrição do acervo fotografico")]
    public string Descricao { get; set; }
    
    [Required(ErrorMessage = "É necessário informar a quantidade do acervo fotografico")]
    [Range(1, long.MaxValue, ErrorMessage = "A quantidade do acervo fotográfico deve ser maior que zero")]
    public long Quantidade { get; set; }
    
    public float? Largura { get; set; }
    public float? Altura { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador do suporte do acervo fotografico")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador do suporte do acervo fotográfico deve ser maior que zero")]
    public long SuporteId { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador do formato do acervo fotografico")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador do formato do acervo fotográfico deve ser maior que zero")]
    public long FormatoId { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o tamanho do arquivo do acervo fotografico")]
    [MaxLength(15, ErrorMessage = "O tamanho do arquivo do acervo fotográfico não pode conter mais que 15 caracteres")]
    public string TamanhoArquivo { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador da cromia do acervo fotografico")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador da cromia do acervo fotográfico deve ser maior que zero")]
    public long CromiaId { get; set; }
    
    [Required(ErrorMessage = "É necessário informar a resolução do acervo fotografico")]
    [MaxLength(15, ErrorMessage = "A resolução do acervo fotográfico não pode conter mais que 15 caracteres")]
    public string Resolucao { get; set; }
    
    public long[]? Arquivos { get; set; }
}