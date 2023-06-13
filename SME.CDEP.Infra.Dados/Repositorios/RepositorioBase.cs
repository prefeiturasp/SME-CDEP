using Dommel;
using SME.CDEP.Dominio;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios;

public abstract class RepositorioBase<TEntidade> : IRepositorioBase<TEntidade>
    where TEntidade : EntidadeBase    
{
    protected readonly ICdepConexao conexao;

    public RepositorioBase(ICdepConexao conexao)
    {
        this.conexao = conexao;
    }

    public Task<TEntidade> ObterPorId(long id)
        => conexao.Obter().GetAsync<TEntidade>(id);

    public async Task<IList<TEntidade>> ObterTodos()
        => (await conexao.Obter().GetAllAsync<TEntidade>())
            .ToList();

    public async Task<long> Inserir(TEntidade entidade)
    {
        entidade.Id = (long)await conexao.Obter().InsertAsync(entidade);
        return entidade.Id;
    }

    public async Task<TEntidade> Atualizar(TEntidade entidade)
    {
       await conexao.Obter().UpdateAsync(entidade);
       return entidade;
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
