using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoSolicitacaoConfirmarDto : DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO
{
    [Required(ErrorMessage = "É necessário informar o identificador do atendimento da solicitação")]
    public long Id { get; set; }

    [Required(ErrorMessage = "É necessário informar o identificador do item do acervo")]
    public long ItemId { get; set; }
}