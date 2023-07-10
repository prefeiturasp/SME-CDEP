using Dommel;
using SME.CDEP.Dominio;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Repositorios;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios;

public abstract class RepositorioBaseSemAuditoria<TEntidade> : IRepositorioBaseSemAuditoria<TEntidade>
    where TEntidade : EntidadeBaseSemAuditoria    
{
    protected readonly IContextoAplicacao contexto;
    protected readonly ICdepConexao conexao;

    public RepositorioBaseSemAuditoria(IContextoAplicacao contexto,ICdepConexao conexao)
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
        entidade.Id = (long)await conexao.Obter().InsertAsync(entidade);
        return entidade.Id;
    }

    public async Task<TEntidade> Atualizar(TEntidade entidade)
    {
       await conexao.Obter().UpdateAsync(entidade);
       return entidade;
    }

    public async Task Remover(TEntidade entidade)
    {
        entidade.Excluido = true;
        await Atualizar(entidade);
    }

    public async Task Remover(long id)
    {
        var retorno = await ObterPorId(id);
        await Remover(retorno);
    }
}
