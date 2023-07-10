namespace SME.CDEP.Dominio.Entidades;

public interface IDominioBaseSemAuditoria<TEntidade> 
    where TEntidade : EntidadeBase
{
    Task<TEntidade> ObterPorId(long id);
    Task<IList<TEntidade>> ObterTodos();
    Task<long> Inserir(TEntidade entidade);
    Task<TEntidade> Atualizar(TEntidade entidade);
}
