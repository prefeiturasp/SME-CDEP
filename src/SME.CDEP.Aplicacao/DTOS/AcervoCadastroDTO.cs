using SME.CDEP.Dominio.Enumerados;
using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoCadastroDTO
{
    [Required(ErrorMessage = "É necessário informar o título do acervo")]
    [MaxLength(500, ErrorMessage = "A localização do acervo não pode conter mais que 500 caracteres")]
    public string Titulo { get; set; } = null!;
    public string? Descricao { get; set; }

    [MaxLength(200, ErrorMessage = "O código do acervo não pode conter mais que 200 caracteres")]
    public string? Codigo { get; set; }

    [MaxLength(200, ErrorMessage = "O código novo do acervo não pode conter mais que 200 caracteres")]
    public string? CodigoNovo { get; set; }
    public long[]? CreditosAutoresIds { get; set; }
    public CoAutorDTO[]? CoAutores { get; set; }
    public string? SubTitulo { get; set; }
    public string? DataAcervo { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o ano do acervo")]
    public string Ano { get; set; } = null!;
    public SituacaoAcervo SituacaoAcervo { get; set; }
}