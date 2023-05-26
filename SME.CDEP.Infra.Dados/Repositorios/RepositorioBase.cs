using SME.CDEP.Dominio;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios;

public abstract class RepositorioBase<TEntidade,TChave> : IRepositorioBase<TEntidade,TChave>
    where TEntidade : EntidadeBase<TChave>
    where TChave : struct
{
    private readonly ICdepConexao _conexao;

    public RepositorioBase(ICdepConexao conexao)
    {
        _conexao = conexao;
    }

    public Task<TEntidade> ObterPorId(TChave id)
    {
        throw new NotImplementedException();
    }

    public Task<IList<TEntidade>> ObterTodos()
    {
        throw new NotImplementedException();
    }

    public Task<TChave> Inserir(TEntidade entidade)
    {
        throw new NotImplementedException();
    }

    public Task<TEntidade> Atualizar(TEntidade entidade)
    {
        throw new NotImplementedException();
    }
}
