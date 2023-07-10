namespace SME.CDEP.Dominio.Repositorios;

public interface IRepositorioBaseSemAuditoria<TEntidade> 
    where TEntidade : EntidadeBaseSemAuditoria    
{
    Task<TEntidade> ObterPorId(long id);
    Task<IList<TEntidade>> ObterTodos();
    Task<long> Inserir(TEntidade entidade);
    Task<TEntidade> Atualizar(TEntidade entidade);
    Task Remover(TEntidade entidade);
    Task Remover(long id);
}
