namespace SME.CDEP.Dominio.Dominios;

public interface IDominioBase<TEntidade, TChave> 
    where TEntidade : EntidadeBase<TChave>
    where TChave : struct
{
    Task<TEntidade> ObterPorId(TChave id);
    Task<IList<TEntidade>> ObterTodos();
    Task<TChave> Inserir(TEntidade entidade);
    Task<TEntidade> Atualizar(TEntidade entidade);
}
