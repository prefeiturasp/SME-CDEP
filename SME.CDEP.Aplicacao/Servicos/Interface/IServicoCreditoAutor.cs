using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoCreditoAutor : IServicoAplicacao, IServicoIdNomeTipoExcluidoAuditavel
    {
        Task<IEnumerable<IdNomeTipoExcluidoAuditavelDTO>> ObterTodos(TipoCreditoAutoria? tipo);
        Task<long> ObterPorNomeETipo(string nome, TipoCreditoAutoria tipoCreditoAutoria);
    }
}
