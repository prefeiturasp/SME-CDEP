using System.ComponentModel.DataAnnotations;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoEmprestimoProrrogacaoDTO
{
    [Required(ErrorMessage = "É necessário informar o identificador do item do acervo solicitação")]
    public long AcervoSolicitacaoItemId { get; set; }
    
    [Required(ErrorMessage = "É necessário informar a data de devolucao")]
    public DateTime DataDevolucao { get; set; }
}