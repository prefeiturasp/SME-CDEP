using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Repositorios;

public interface IRepositorioBaseSomenteId<TEntidade> where TEntidade : EntidadeBaseSomenteId    
{
    Task<TEntidade> ObterPorId(long id);
    Task<IEnumerable<TEntidade>> ObterTodos();
    Task<long> Inserir(TEntidade entidade);
    Task<TEntidade> Atualizar(TEntidade entidade);
}
