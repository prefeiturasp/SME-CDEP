namespace SME.CDEP.Dominio.Entidades;

public interface IDominioBaseSemAuditoria<TEntidade> 
    where TEntidade : EntidadeBaseSemAuditoria
{
    Task<TEntidade> ObterPorId(long id);
    Task<IList<TEntidade>> ObterTodos();
    Task<long> Inserir(TEntidade entidade);
    Task<TEntidade> Atualizar(TEntidade entidade);
}
