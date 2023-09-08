namespace SME.CDEP.Dominio.Repositorios;

public interface IRepositorioBaseAuditavel<TEntidade> 
    where TEntidade : EntidadeBaseAuditavel    
{
    Task<TEntidade> ObterPorId(long id);
    Task<IEnumerable<TEntidade>> ObterTodos();
    Task<long> Inserir(TEntidade entidade);
    Task<TEntidade> Atualizar(TEntidade entidade);
    Task Remover(TEntidade entidade);
    Task Remover(long id);
}
