using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioSerieColecao : IRepositorioBase<SerieColecao>
    {
        Task<bool> Existe(string nome, long id);
        Task<IEnumerable<SerieColecao>> PesquisarPorNome(string nome);
    }
}