using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoAudiovisualCadastroDTO : AcervoCadastroDTO
{
    [MaxLength(100, ErrorMessage = "A localização do acervo audiovisual não pode conter mais que 100 caracteres")]
    public string? Localizacao { get; set; }
    
    [MaxLength(200, ErrorMessage = "A procedência do acervo audiovisual não pode conter mais que 200 caracteres")]
    public string? Procedencia { get; set; }
    
    [Required(ErrorMessage = "É necessário informar a data do acervo audiovisual")]
    [MaxLength(50, ErrorMessage = "A data do acervo audiovisual não pode conter mais que 50 caracteres")]
    public string DataAcervo { get; set; }
    
    [MaxLength(100, ErrorMessage = "A cópia do acervo audiovisual não pode conter mais que 100 caracteres")]
    public string? Copia { get; set; }
    
    public bool PermiteUsoImagem { get; set; }
    
    public long? ConservacaoId { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o suporte do acervo audiovisual")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador do suporte do acervo audiovisual deve ser maior que zero")]
    public long SuporteId { get; set; }
    
    [MaxLength(15, ErrorMessage = "A duração do acervo audiovisual não pode conter mais que 15 caracteres")]
    public string? Duracao { get; set; }
    
    public long? CromiaId { get; set; }
    
    [MaxLength(15, ErrorMessage = "A localização do acervo audiovisual não pode conter mais que 100 caracteres")]
    public string? TamanhoArquivo { get; set; }
    
    [MaxLength(100, ErrorMessage = "A localização do acervo audiovisual não pode conter mais que 100 caracteres")]
    public string? Acessibilidade { get; set; }
    
    [MaxLength(200, ErrorMessage = "A localização do acervo audiovisual não pode conter mais que 100 caracteres")]
    public string? Disponibilizacao { get; set; }
}