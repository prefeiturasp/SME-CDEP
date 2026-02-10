using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoConfirmacaoAtendimentoAcervo
    {
        Task<bool> Executar(AcervoSolicitacaoConfirmarDto dto);
    }
}