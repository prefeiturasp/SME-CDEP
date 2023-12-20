using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Minio;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Servicos.Mensageria;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;

namespace SME.CDEP.Infra.Servicos.ServicoArmazenamento
{
    public class ServicoArmazenamento : IServicoArmazenamento
    {
        private MinioClient minioClient;
        private ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions;
        private IServicoMensageria servicoMensageria;
        private readonly IConfiguration configuration;

        public ServicoArmazenamento(IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions, IConfiguration configuration, IServicoMensageria servicoMensageria)
        {
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions?.Value ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));

            Inicializar();
        }

        private void Inicializar()
        {
            minioClient = new MinioClient()
                .WithEndpoint(configuracaoArmazenamentoOptions.EndPoint, configuracaoArmazenamentoOptions.Port)
                .WithCredentials(configuracaoArmazenamentoOptions.AccessKey, configuracaoArmazenamentoOptions.SecretKey)
                .WithSSL()
                .Build();
        }

        public async Task<string> ArmazenarTemporaria(string nomeArquivo, Stream stream, string contentType)
        {
            await ArmazenarArquivo(nomeArquivo, stream, contentType, configuracaoArmazenamentoOptions.BucketTemp);

            return await ObterUrl(nomeArquivo, configuracaoArmazenamentoOptions.BucketTemp);
        }

        public async Task<string> Armazenar(string nomeArquivo, Stream stream, string contentType)
        {
            return await ArmazenarArquivo(nomeArquivo, stream, contentType, configuracaoArmazenamentoOptions.BucketArquivos);
        }

        private async Task<string> ArmazenarArquivo(string nomeArquivo, Stream stream, string contentType, string bucket)
        {
            var args = new PutObjectArgs()
                .WithBucket(bucket)
                .WithObject(nomeArquivo)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithVersionId("1.0")
                .WithContentType(contentType);

            await minioClient.PutObjectAsync(args);

            return await ObterUrl(nomeArquivo, bucket);
        }

        private async Task<string> Copiar(string nomeArquivo)
        {
            if (!configuracaoArmazenamentoOptions.BucketTemp.Equals(configuracaoArmazenamentoOptions.BucketArquivos))
            {
                var cpSrcArgs = new CopySourceObjectArgs()
                    .WithBucket(configuracaoArmazenamentoOptions.BucketTemp)
                    .WithObject(nomeArquivo);

                var args = new CopyObjectArgs()
                    .WithBucket(configuracaoArmazenamentoOptions.BucketArquivos)
                    .WithObject(nomeArquivo)
                    .WithCopyObjectSource(cpSrcArgs);

                await minioClient.CopyObjectAsync(args);
            }

            return $"{configuracaoArmazenamentoOptions.BucketArquivos}/{nomeArquivo}";
        }

        public async Task<string> Mover(string nomeArquivo)
        {
            if (!configuracaoArmazenamentoOptions.BucketTemp.Equals(configuracaoArmazenamentoOptions.BucketArquivos))
            {
                await Copiar(nomeArquivo);

                await Excluir(nomeArquivo, configuracaoArmazenamentoOptions.BucketTemp);
            }

            return $"{configuracaoArmazenamentoOptions.BucketArquivos}/{nomeArquivo}";
        }

        public async Task<bool> Excluir(string nomeArquivo, string nomeBucket = "")
        {
            nomeBucket = nomeBucket.NaoEstaPreenchido()
                ? configuracaoArmazenamentoOptions.BucketArquivos
                : nomeBucket;

            var args = new RemoveObjectArgs()
                .WithBucket(nomeBucket)
                .WithObject(nomeArquivo);

            await minioClient.RemoveObjectAsync(args);
            return true;
        }

        public async Task<IEnumerable<string>> ObterBuckets()
        {
            var nomesBuckets = new List<string>();

            var buckets = await minioClient.ListBucketsAsync();

            foreach (var bucket in buckets.Buckets)
                nomesBuckets.Add(bucket.Name);

            return nomesBuckets;
        }

        public async Task<string> Obter(string nomeArquivo, bool ehPastaTemp)
        {
            var bucketNome = ehPastaTemp
                ? configuracaoArmazenamentoOptions.BucketTemp
                : configuracaoArmazenamentoOptions.BucketArquivos;

            return await ObterUrl(nomeArquivo, bucketNome);
        }

        private async Task<string> ObterUrl(string nomeArquivo, string bucketName)
        {
            var hostAplicacao = configuration["UrlFrontEnd"];
            return $"{hostAplicacao}{bucketName}/{nomeArquivo}";
        }
    }
}