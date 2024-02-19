using System.ComponentModel.DataAnnotations;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class AcervoSolicitacaoItemManualDTO
    {
        public long? Id { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o identificador do acervo para realizar a solicitação manual de acervos")]
        public long AcervoId { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o tipo de atendimentopara realizar a solicitação manual de acervos")]
        public TipoAtendimento TipoAtendimento { get; set; }
        public DateTime? DataVisita { get; set; }
    }
}
