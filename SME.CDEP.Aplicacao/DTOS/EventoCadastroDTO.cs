using System.ComponentModel.DataAnnotations;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class EventoCadastroDTO : DiaMesDTO
    {
        public long? Id { get; set; }
        public TipoEvento Tipo { get; set; }
        
        [Required(ErrorMessage = "É necessário informar a descrição do evento")]
        public string Descricao { get; set; }
        public string? Justificativa { get; set; }
        public long? AcervoSolicitacaoItemId { get; set; }
    }
}
