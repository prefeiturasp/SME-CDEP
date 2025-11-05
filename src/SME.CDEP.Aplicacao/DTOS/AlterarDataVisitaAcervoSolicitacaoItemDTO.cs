using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS;

public class AlterarDataVisitaAcervoSolicitacaoItemDTO
{
    [Required(ErrorMessage = "É necessário informar o identificador do item do atendimento da solicitação")]
    public long Id { get; set; }
    
    [Required(ErrorMessage = "É necessário informar a data da visita")]
    public DateTime DataVisita { get; set; }
}