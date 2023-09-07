using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioCreditoAutor : IRepositorioBase<CreditoAutor>
    {
        Task<IList<CreditoAutor>> ObterTodosPorTipo(TipoCreditoAutoria tipo);
        Task<IList<CreditoAutor>> PesquisarPorNomeTipo(string nome, TipoCreditoAutoria tipo);
    }
}