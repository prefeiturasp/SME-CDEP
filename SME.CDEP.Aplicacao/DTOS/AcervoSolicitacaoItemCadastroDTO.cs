using System.ComponentModel.DataAnnotations;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class AcervoSolicitacaoItemCadastroDTO
    {
        [Required(ErrorMessage = "É necessário informar o identificador do acervo para realizar a solicitação")]
        public long AcervoId { get; set; }
        
        public DateTime? DataVisita { get; set; }
    }
}
