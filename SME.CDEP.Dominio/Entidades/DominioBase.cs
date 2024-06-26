using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Dominio.Entidades;

public abstract class DominioBase<TEntidade> : IDominioBase<TEntidade>
    where TEntidade : EntidadeBase
{
    private readonly IRepositorioBase<TEntidade> _repositorio;

    public DominioBase(IRepositorioBase<TEntidade> repositorio)
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
