using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Repositorios;

public interface IRepositorioBase<TEntidade> where TEntidade : EntidadeBase    
{
    Task<TEntidade> ObterPorId(long id);
    Task<IEnumerable<TEntidade>> ObterTodos();
    Task<long> Inserir(TEntidade entidade);
    Task<TEntidade> Atualizar(TEntidade entidade);
    Task Remover(TEntidade entidade);
    Task Remover(long id);
}
