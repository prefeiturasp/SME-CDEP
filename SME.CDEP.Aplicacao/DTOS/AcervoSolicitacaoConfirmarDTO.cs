using System.ComponentModel.DataAnnotations;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoSolicitacaoConfirmarDTO
{
    [Required(ErrorMessage = "É necessário informar o identificador do atendimento da solicitação")]
    public long Id { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o Responsável do atendimento")]
    public string ResponsavelRf { get; set; }
    
    [Required(ErrorMessage = "É necessário informar os itens do atendimento da solicitação")]
    public IEnumerable<AcervoSolicitacaoItemConfirmarDTO> Itens { get; set; } = Enumerable.Empty<AcervoSolicitacaoItemConfirmarDTO>();
}