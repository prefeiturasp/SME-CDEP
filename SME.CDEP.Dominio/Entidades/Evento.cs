using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class Evento : EntidadeBaseAuditavel
    {
        public DateTime Data { get; set; }
        public TipoEvento Tipo { get; set; }
        public string Descricao { get; set; }
        public string Justificativa { get; set; }
        public long? AcervoSolicitacaoItemId { get; set; }

        public void Validar()
        {
            if (Justificativa.NaoEstaPreenchido() && Tipo.EhSuspensao())
                throw new NegocioException(MensagemNegocio.JUSTIFICATIVA_NAO_INFORMADA);    
        }
    }
}
