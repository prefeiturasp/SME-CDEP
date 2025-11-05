using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioCreditoAutor : IRepositorioBase<CreditoAutor>
    {
        Task<IEnumerable<CreditoAutor>> ObterTodosPorTipo(TipoCreditoAutoria tipo);
        Task<IEnumerable<CreditoAutor>> PesquisarPorNomeTipo(string nome, TipoCreditoAutoria tipo);
        Task<bool> Existe(string nome, long id, int tipo);
        Task<IEnumerable<CreditoAutor>> PesquisarPorNome(string nome, int tipo);
        Task<long> ObterPorNomeTipo(string nome, TipoCreditoAutoria tipoCreditoAutoria);
    }
}