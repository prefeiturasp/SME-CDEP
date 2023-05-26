using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Dominio.Dominios;

public abstract class DominioBase<TEntidade, TChave> : IDominioBase<TEntidade,TChave>
    where TEntidade : EntidadeBase<TChave>
    where TChave : struct
{
    private readonly IRepositorioBase<TEntidade, TChave> _repositorio;

    public DominioBase(IRepositorioBase<TEntidade, TChave> repositorio)
    {
        _repositorio = repositorio;
    }
    
    public async Task<TEntidade> ObterPorId(TChave id)
    {
        return await _repositorio.ObterPorId(id);
    }

    public async Task<IList<TEntidade>> ObterTodos()
    {
        return await _repositorio.ObterTodos();
    }

    public async Task<TChave> Inserir(TEntidade entidade)
    {
        return await _repositorio.Inserir(entidade);
    }

    public async Task<TEntidade> Atualizar(TEntidade entidade)
    {
        return await _repositorio.Atualizar(entidade);
    }
}
