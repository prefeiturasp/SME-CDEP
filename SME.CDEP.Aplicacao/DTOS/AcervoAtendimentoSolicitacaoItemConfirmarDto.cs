using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoAtendimentoSolicitacaoItemConfirmarDto : DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO
{
    [Required(ErrorMessage = "É necessário informar o identificador do item do acervo")]
    public long Id { get; set; }
}