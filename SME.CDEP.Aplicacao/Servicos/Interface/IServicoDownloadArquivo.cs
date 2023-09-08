
namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoDownloadArquivo
    {
        Task<(byte[], string, string)> Download(Guid codigoArquivo);
    }
}
