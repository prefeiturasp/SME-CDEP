using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoAudiovisualAlteracaoDTO : AcervoAudiovisualCadastroDTO
{
    [Required(ErrorMessage = "É necessário informar o identificador do acervo arte gráfica")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador do acervo arte gráfica deve ser maior que zero")]
    public long Id { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador do acervo")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador do acervo deve ser maior que zero")]
    public long AcervoId { get; set; }
}