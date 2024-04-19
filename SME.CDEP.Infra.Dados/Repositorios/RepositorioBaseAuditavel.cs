using Dapper;
using Dommel;
using SME.CDEP.Dominio;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Repositorios;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios;

public abstract class RepositorioBaseAuditavel<TEntidade> : IRepositorioBaseAuditavel<TEntidade>
    where TEntidade : EntidadeBaseAuditavel    
{
    protected readonly IContextoAplicacao contexto;
    protected readonly ICdepConexao conexao;

    public RepositorioBaseAuditavel(IContextoAplicacao contexto,ICdepConexao conexao)
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
        entidade.CriadoEm = DateTimeExtension.HorarioBrasilia();
        entidade.CriadoPor = contexto.NomeUsuario;
        entidade.CriadoLogin = contexto.UsuarioLogado;
        entidade.Id = (long)await conexao.Obter().InsertAsync(entidade);
        return entidade.Id;
    }

    public async Task<TEntidade> Atualizar(TEntidade entidade)
    { 
        DefinirAtualização(entidade);
        await conexao.Obter().UpdateAsync(entidade);
        return entidade;
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
        entidade.AlteradoEm = DateTimeExtension.HorarioBrasilia();
        entidade.AlteradoPor = contexto.NomeUsuario;
        entidade.AlteradoLogin = contexto.UsuarioLogado;
        entidade.Excluido = excluir;
    }
}
