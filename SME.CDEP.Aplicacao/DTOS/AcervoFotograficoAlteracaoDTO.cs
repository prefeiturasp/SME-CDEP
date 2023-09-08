using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoFotograficoAlteracaoDTO : AcervoFotograficoCadastroDTO
{
    [Required(ErrorMessage = "É necessário informar o identificador do acervo fotográfico")]
    public long Id { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador do acervo")]
    public long AcervoId { get; set; }
}