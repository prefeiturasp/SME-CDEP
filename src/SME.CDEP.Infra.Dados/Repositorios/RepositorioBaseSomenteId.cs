using Dommel;
using SME.CDEP.Dominio;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Repositorios;
using System.Diagnostics.CodeAnalysis;

namespace SME.CDEP.Infra.Dados.Repositorios;

[ExcludeFromCodeCoverage]
public abstract class RepositorioBaseSomenteId<TEntidade> : IRepositorioBaseSomenteId<TEntidade>
    where TEntidade : EntidadeBaseSomenteId
{
    protected readonly IContextoAplicacao contexto;
    protected readonly ICdepConexao conexao;

    public RepositorioBaseSomenteId(IContextoAplicacao contexto, ICdepConexao conexao)
    {
        this.contexto = contexto;
        this.conexao = conexao;
    }

    public Task<TEntidade> ObterPorId(long id)
        => conexao.Obter().GetAsync<TEntidade>(id);

    public async Task<IEnumerable<TEntidade>> ObterTodos()
        => await conexao.Obter().GetAllAsync<TEntidade>();

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
}
