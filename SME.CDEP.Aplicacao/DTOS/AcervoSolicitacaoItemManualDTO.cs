using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class AcervoSolicitacaoItemManualDTO : DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO
    {
        public long? Id { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o identificador do acervo para realizar a solicitação manual de acervos")]
        public long AcervoId { get; set; }
    }
}
