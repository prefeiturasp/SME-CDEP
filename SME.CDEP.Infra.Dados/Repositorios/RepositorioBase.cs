using Dapper;
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
    
    public async Task Remover(TEntidade entidade)
    {
        DefinirAtualização(entidade, true);
        await conexao.Obter().UpdateAsync(entidade);
    }

    public async Task Remover(long id)
    {
        var entidade = await ObterPorId(id);
        DefinirAtualização(entidade,true);
        await conexao.Obter().UpdateAsync(entidade);
    }

    private void DefinirAtualização(TEntidade entidade, bool excluir = false)
    {
        entidade.Excluido = excluir;
    }
    
    protected async Task<string> ObterCreditosAutores(long acervoId)
    {
        var query = @" select ca.nome 
                           from acervo_credito_autor aca
                              join credito_autor ca on aca.credito_autor_id = ca.id 
                           where aca.acervo_id = @acervoId";

        var creditos = await conexao.Obter().QueryAsync<string>(query, new { acervoId });
        return string.Join(" | ", creditos.Select(s=> s));
    }
}
