namespace SME.CDEP.Dominio.Entidades;

public interface IDominioBase<TEntidade> 
    where TEntidade : EntidadeBaseAuditavel
{
    Task<TEntidade> ObterPorId(long id);
    Task<IList<TEntidade>> ObterTodos();
    Task<long> Inserir(TEntidade entidade);
    Task<TEntidade> Atualizar(TEntidade entidade);
}
