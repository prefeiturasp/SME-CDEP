using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Dominio.Entidades;

public abstract class DominioBaseSemAuditoria<TEntidade> : IDominioBaseSemAuditoria<TEntidade>
    where TEntidade : EntidadeBase
{
    private readonly IRepositorioBase<TEntidade> _repositorio;

    public DominioBaseSemAuditoria(IRepositorioBase<TEntidade> repositorio)
    {
        _repositorio = repositorio;
    }
    
    public async Task<TEntidade> ObterPorId(long id)
    {
        return await _repositorio.ObterPorId(id);
    }

    public async Task<IList<TEntidade>> ObterTodos()
    {
        return await _repositorio.ObterTodos();
    }

    public async Task<long> Inserir(TEntidade entidade)
    {
        return await _repositorio.Inserir(entidade);
    }

    public async Task<TEntidade> Atualizar(TEntidade entidade)
    {
        return await _repositorio.Atualizar(entidade);
    }
}
