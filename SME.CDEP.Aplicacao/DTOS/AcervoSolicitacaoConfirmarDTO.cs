using System.ComponentModel.DataAnnotations;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoSolicitacaoConfirmarDTO : DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO
{
    [Required(ErrorMessage = "É necessário informar o identificador do atendimento da solicitação")]
    public long Id { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador do item do acervo")]
    public long ItemId { get; set; }
}