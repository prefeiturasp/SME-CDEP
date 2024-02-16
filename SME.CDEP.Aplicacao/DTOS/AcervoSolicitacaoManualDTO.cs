using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class AcervoSolicitacaoManualDTO : AcervoSolicitacaoManualCadastroDTO
    {
        [Required(ErrorMessage = "É necessário informar o identificador da solicitação manual de acervos")]
        public long Id { get; set; }

        public IEnumerable<AcervoSolicitacaoItemManualDTO> Itens { get; set; } = Enumerable.Empty<AcervoSolicitacaoItemManualDTO>();
    }
}
