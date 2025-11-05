using System.ComponentModel.DataAnnotations;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO
{
    public DateTime? DataVisita { get; set; }
    public DateTime? DataEmprestimo { get; set; }
    public DateTime? DataDevolucao { get; set; }
    public TipoAcervo TipoAcervo { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o tipo de atendimentopara realizar a solicitação de acervos")]
    public TipoAtendimento TipoAtendimento { get; set; }
}