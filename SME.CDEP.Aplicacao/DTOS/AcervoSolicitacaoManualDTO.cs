using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class AcervoSolicitacaoManualDTO
    {
        public long Id { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o identificador do usuario para realizar a solicitação manual de acervos")]
        public long UsuarioId { get; set; }
        
        [Required(ErrorMessage = "É necessário informar a data da solicitação para realizar a solicitação manual de acervos")]
        public DateTime DataSolicitacao { get; set; }

        public IEnumerable<AcervoAtendimentoSolicitacaoItemManualDto> Itens { get; set; } = Enumerable.Empty<AcervoAtendimentoSolicitacaoItemManualDto>();
    }
}
