using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoTridimensionalAlteracaoDTO : AcervoTridimensionalCadastroDTO
{
    [Required(ErrorMessage = "É necessário informar o identificador do acervo tridimensional")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador do acervo tridimensional deve ser maior que zero")]
    public long Id { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador do acervo")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador do acervo deve ser maior que zero")]
    public long AcervoId { get; set; }
}