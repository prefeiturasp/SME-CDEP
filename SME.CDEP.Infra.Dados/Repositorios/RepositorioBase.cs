using Dommel;
using SME.CDEP.Dominio;
using SME.CDEP.Dominio.Contexto;
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
        entidade.CriadoEm = DateTimeExtension.HorarioBrasilia();
        entidade.CriadoPor = contexto.NomeUsuario;
        entidade.CriadoLogin = contexto.UsuarioLogado;
        entidade.Id = (long)await conexao.Obter().InsertAsync(entidade);
        return entidade.Id;
    }

    public async Task<TEntidade> Atualizar(TEntidade entidade)
    {
        entidade.AlteradoEm = DateTimeExtension.HorarioBrasilia();
        entidade.AlteradoPor = contexto.NomeUsuario;
        entidade.AlteradoLogin = contexto.UsuarioLogado;
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
