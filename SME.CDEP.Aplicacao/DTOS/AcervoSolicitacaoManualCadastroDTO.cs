using System.ComponentModel.DataAnnotations;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class AcervoSolicitacaoManualCadastroDTO
    {
        [Required(ErrorMessage = "É necessário informar o identificador do usuario para realizar a solicitação manual de acervos")]
        public long UsuarioId { get; set; }
        
        [Required(ErrorMessage = "É necessário informar a data da solicitação para realizar a solicitação manual de acervos")]
        public DateTime DataSolicitacao { get; set; }

        public IEnumerable<AcervoSolicitacaoItemManualCadastroDTO> Itens { get; set; } = Enumerable.Empty<AcervoSolicitacaoItemManualCadastroDTO>();
    }
}
