using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioCromia : IRepositorioBase<Cromia>
    {
        Task<long> ObterPorNome(string nome);
    }
}