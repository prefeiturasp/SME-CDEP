using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;

namespace SME.CDEP.TesteIntegracao.ServicosFakes
{
    public class ServicoArmazenamentoFake : IServicoArmazenamento
    {
        public Task<string> ArmazenarTemporaria(string nomeArquivo, Stream stream, string contentType)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> Armazenar(string nomeArquivo, Stream stream, string contentType)
        {
            return Task.FromResult(string.Empty);
        }

        private Task<string> ArmazenarArquivo(string nomeArquivo, Stream stream, string contentType, string bucket)
        {
            return Task.FromResult(string.Empty);
        }

        private Task<string> Copiar(string nomeArquivo)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> Mover(string nomeArquivo)
        {
            return Task.FromResult(string.Empty);
        }

        private Task OtimizarArquivos(string nomeArquivo)
        {
            return Task.CompletedTask;
        }

        public Task<bool> Excluir(string nomeArquivo, string nomeBucket = "")
        {
            return Task.FromResult(true);
        }

        public Task<IEnumerable<string>> ObterBuckets()
        {
            return Task.FromResult(Enumerable.Empty<string>());
        }

        public Task<string> Obter(string nomeArquivo, bool ehPastaTemp)
        {
            return Task.FromResult(string.Empty);
        }

        private Task<string> ObterUrl(string nomeArquivo, string bucketName)
        {
            return Task.FromResult(string.Empty);
        }
    }
}