using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Dominio.Entidades;

public abstract class DominioBaseAuditavel<TEntidade> : IDominioBaseAuditavel<TEntidade>
    where TEntidade : EntidadeBaseAuditavel
{
    private readonly IRepositorioBaseAuditavel<TEntidade> _repositorio;

    public DominioBaseAuditavel(IRepositorioBaseAuditavel<TEntidade> repositorio)
    {
        _repositorio = repositorio;
    }
    
    public Task<TEntidade> ObterPorId(long id)
    {
        return _repositorio.ObterPorId(id);
    }

    public Task<IEnumerable<TEntidade>> ObterTodos()
    {
        return _repositorio.ObterTodos();
    }

    public Task<long> Inserir(TEntidade entidade)
    {
        return _repositorio.Inserir(entidade);
    }

    public Task<TEntidade> Atualizar(TEntidade entidade)
    {
        return _repositorio.Atualizar(entidade);
    }
}
