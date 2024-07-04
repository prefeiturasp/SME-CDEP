using System.ComponentModel.DataAnnotations;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class EventoDTO
    {
        public long Id { get; set; }
        public TipoEvento Tipo { get; set; }
        
        [Required(ErrorMessage = "É necessário informar a descrição do evento")]
        public string Descricao { get; set; }
        public string Justificativa { get; set; }
        public long? AcervoSolicitacaoItemId { get; set; }
        public DateTime Data { get; set; }
    }
}
