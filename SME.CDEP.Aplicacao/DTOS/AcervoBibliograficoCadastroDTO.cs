using System.ComponentModel.DataAnnotations;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoBibliograficoCadastroDTO : AcervoCadastroDTO
{
    [Required(ErrorMessage = "É necessário informar o identificador do material do acervo bibliográfico")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador do material do acervo bibliográfico deve ser maior que zero")]
    public long MaterialId { get; set; }
    
    public long? EditoraId { get; set; }
    
    public long[] AssuntosIds { get; set; }
    public string? Edicao { get; set; }
    public int? NumeroPagina { get; set; }
    public string? Largura { get; set; }
    public string? Altura { get; set; }
    public long? SerieColecaoId { get; set; }
    public string? Volume { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador do idioma do acervo bibliográfico")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador do idioma do acervo bibliográfico deve ser maior que zero")]
    public long IdiomaId { get; set; }
    
    [Required(ErrorMessage = "É necessário informar a localizção CDD do acervo bibliográfico")]
    [MaxLength(50, ErrorMessage = "A localizção CDD do acervo bibliográfico não pode conter mais que 50 caracteres")]
    public string LocalizacaoCDD { get; set; }
    
    public string? LocalizacaoPHA { get; set; }
    
    public string? NotasGerais { get; set; }
    public string? Isbn { get; set; }
    public SituacaoSaldo? SituacaoSaldo { get; set; } = Infra.Dominio.Enumerados.SituacaoSaldo.DISPONIVEL;
}