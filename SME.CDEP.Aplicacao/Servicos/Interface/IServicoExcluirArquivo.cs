
namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoExcluirArquivo
    {
        Task<bool> Excluir(long[] ids);
    }
}
