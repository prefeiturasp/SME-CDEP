using Dommel;
using SME.CDEP.Dominio;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Repositorios;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios;

public abstract class RepositorioBase<TEntidade> : IRepositorioBase<TEntidade>
    where TEntidade : EntidadeBase    
{
    protected readonly IContextoAplicacao contexto;
    protected readonly ICdepConexao conexao;

    public RepositorioBase(IContextoAplicacao contexto,ICdepConexao conexao)
    {
        this.contexto = contexto;
        this.conexao = conexao;
    }

    public Task<TEntidade> ObterPorId(long id)
        => conexao.Obter().GetAsync<TEntidade>(id);

    public async Task<IList<TEntidade>> ObterTodos()
        => (await conexao.Obter().GetAllAsync<TEntidade>())
            .ToList();

    public async Task<long> Inserir(TEntidade entidade)
    {
        try
        {
            entidade.Id = (long)await conexao.Obter().InsertAsync(entidade);
            return entidade.Id;
        }
        catch (Exception e)
        {
            if (e.Message.Contains(Constantes.VIOLACAO_CONSTRAINT_DUPLICACAO_REGISTROS_CODIGO))
                throw new NegocioException(Constantes.VIOLACAO_CONSTRAINT_DUPLICACAO_REGISTROS_MENSAGEM);
            throw;
        }
    }

    public async Task<TEntidade> Atualizar(TEntidade entidade)
    {
        try
        {
            await conexao.Obter().UpdateAsync(entidade);
            return entidade;
        }
        catch (Exception e)
        {
            if (e.Message.Contains(Constantes.VIOLACAO_CONSTRAINT_DUPLICACAO_REGISTROS_CODIGO))
                throw new NegocioException(Constantes.VIOLACAO_CONSTRAINT_DUPLICACAO_REGISTROS_MENSAGEM);
            throw;
        }
    }

    public async Task Remover(TEntidade entidade)
    {
        throw new NotImplementedException();
    }

    public async Task Remover(long id)
    {
        throw new NotImplementedException();
    }
}
