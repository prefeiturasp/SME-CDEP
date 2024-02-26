using System.ComponentModel.DataAnnotations;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoSolicitacaoItem : EntidadeBaseAuditavel
    {
        public long AcervoSolicitacaoId { get; set; }
        public long AcervoId { get; set; }
        public DateTime? DataVisita { get; set; }
        public SituacaoSolicitacaoItem Situacao { get; set; }
        public TipoAtendimento? TipoAtendimento { get; set; }

        public void Validar()
        {
            if (TipoAtendimento.EhInvalido())
                throw new NegocioException(MensagemNegocio.TIPO_ATENDIMENTO_INVALIDO);
            
            if (TipoAtendimento.EhAtendimentoPresencial() && DataVisita.Value < DateTimeExtension.HorarioBrasilia().Date )
                throw new NegocioException(MensagemNegocio.ITENS_ACERVOS_PRESENCIAL_NAO_DEVEM_TER_DATA_ACERVO_PASSADAS);
            
            if (TipoAtendimento.EhAtendimentoViaEmail() && DataVisita.HasValue )
                throw new NegocioException(MensagemNegocio.ITENS_ACERVOS_EMAIL_NAO_DEVEM_TER_DATA_ACERVO);
        }
    }
}
