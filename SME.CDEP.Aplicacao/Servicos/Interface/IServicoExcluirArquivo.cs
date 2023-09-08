
namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoExcluirArquivo
    {
        Task<bool> Excluir(Guid[] codigos);
    }
}
