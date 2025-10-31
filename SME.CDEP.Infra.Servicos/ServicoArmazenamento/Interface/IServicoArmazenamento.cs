using Minio.DataModel;

namespace SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface
{
    public interface IServicoArmazenamento
    {
        Task<string> ArmazenarTemporaria(string nomeArquivo, Stream stream, string contentType);
        Task<string> Armazenar(string nomeArquivo, Stream stream, string contentType);
        Task<string> Mover(string nomeArquivo);
        Task<bool> Excluir(string nomeArquivo, string nomeBucket = "");
        Task<IEnumerable<string>> ObterBuckets();
        Task<string> Obter(string nomeArquivo, bool ehPastaTemp);
        Task<Stream?> ObterStream(string nomeArquivo, string? nomeBucket = null);
        Task<ObjectStat?> ObterMetadadosObjeto(string nomeArquivo, string? nomeBucket = null);
    }
}