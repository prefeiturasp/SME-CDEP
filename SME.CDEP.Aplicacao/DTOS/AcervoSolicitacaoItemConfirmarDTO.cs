using System.ComponentModel.DataAnnotations;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoSolicitacaoItemConfirmarDTO
{
    [Required(ErrorMessage = "É necessário informar o identificador do item do acervo")]
    public long Id { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o tipo de atendimento no item do acervo")]
    public TipoAtendimento TipoAtendimento { get; set; }
    
    public DateTime? DataVisita { get; set; }
}