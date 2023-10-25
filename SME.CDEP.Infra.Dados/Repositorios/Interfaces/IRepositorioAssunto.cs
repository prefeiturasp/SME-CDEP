using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAssunto : IRepositorioBase<Assunto>
    {
        Task<bool> Existe(string nome, long id);
        Task<IEnumerable<Assunto>> PesquisarPorNome(string nome);
        Task<IEnumerable<Assunto>> ObterPorIds(long[] ids);
    }
}