using Dommel;
using SME.CDEP.Dominio;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios;

public abstract class RepositorioBase<TEntidade> : IRepositorioBase<TEntidade>
    where TEntidade : EntidadeBase    
{
    private readonly ICdepConexao _conexao;

    public RepositorioBase(ICdepConexao conexao)
    {
        _conexao = conexao;
    }

    public Task<TEntidade> ObterPorId(long id)
    {
        return Task.FromResult<TEntidade>(_conexao.Obter().Get<TEntidade>(id));
    }

    public Task<IList<TEntidade>> ObterTodos()
    {
        return Task.FromResult((IList<TEntidade>)_conexao.Obter().GetAll<TEntidade>());
    }

    public async Task<long> Inserir(TEntidade entidade)
    {
        return (long)(await _conexao.Obter().InsertAsync(entidade));
    }

    public Task<TEntidade> Atualizar(TEntidade entidade)
    {
        _conexao.Obter().UpdateAsync(entidade);
        return Task.FromResult(entidade);
    }

    public Task Remover(TEntidade entidade)
    {
        throw new NotImplementedException();
    }

    public Task Remover(long id)
    {
        throw new NotImplementedException();
    }
}
