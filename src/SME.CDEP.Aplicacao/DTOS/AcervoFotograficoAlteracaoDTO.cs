using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoFotograficoAlteracaoDTO : AcervoFotograficoCadastroDTO
{
    [Required(ErrorMessage = "É necessário informar o identificador do acervo fotográfico")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador do acervo fotografico deve ser maior que zero")]
    public long Id { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador do acervo")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador do acervodeve ser maior que zero")]
    public long AcervoId { get; set; }
}