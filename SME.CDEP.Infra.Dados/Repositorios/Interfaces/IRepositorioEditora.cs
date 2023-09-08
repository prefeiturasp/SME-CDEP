using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioEditora : IRepositorioBase<Editora>
    {
        Task<bool> Existe(string nome, long id);
        Task<IEnumerable<Editora>> PesquisarPorNome(string nome);
    }
}