using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
using Minio.Exceptions;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Servicos.Mensageria;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;
using System.Diagnostics.CodeAnalysis;

namespace SME.CDEP.Infra.Servicos.ServicoArmazenamento
{
    [ExcludeFromCodeCoverage]
    public class ServicoArmazenamento : IServicoArmazenamento
    {
        private MinioClient minioClient;
        private ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions;
        private IServicoMensageriaLogs servicoMensageriaLogs;
        private readonly IConfiguration configuration;

        public ServicoArmazenamento(IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions, IConfiguration configuration, IServicoMensageriaLogs servicoMensageriaLogs)
        {
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions?.Value ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.servicoMensageriaLogs = servicoMensageriaLogs ?? throw new ArgumentNullException(nameof(servicoMensageriaLogs));

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

            return ObterUrl(nomeArquivo, configuracaoArmazenamentoOptions.BucketTemp);
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

            return ObterUrl(nomeArquivo, bucket);
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

            return await Task.Run(() => ObterUrl(nomeArquivo, bucketNome));
        }

        private string ObterUrl(string nomeArquivo, string bucketName)
        {
            return $"{configuracaoArmazenamentoOptions.EnderecoCompletoPadrao()}/{bucketName}/{nomeArquivo}";
        }

        public async Task<Stream?> ObterStream(string nomeArquivo, string? nomeBucket = null)
        {
            try
            {
                var bucket = string.IsNullOrWhiteSpace(nomeBucket)
                ? configuracaoArmazenamentoOptions.BucketArquivos
                : nomeBucket;

                var memoryStream = new MemoryStream();

                var args = new GetObjectArgs()
                    .WithBucket(bucket)
                    .WithObject(nomeArquivo)
                    .WithCallbackStream(async (stream, cancellationToken) =>
                    {
                        await stream.CopyToAsync(memoryStream, cancellationToken);
                    });

                await minioClient.GetObjectAsync(args);
                memoryStream.Position = 0;
                return memoryStream;
            }
            catch (ObjectNotFoundException)
            {
                return null;
            }
            catch (Exception ex)
            {
                await servicoMensageriaLogs.Enviar($"Erro ao obter o arquivo {nomeArquivo} do bucket {nomeBucket}: {ex.Message}", "ErroObterArquivo", "CDEP");
                return null;
            }
        }
        public async Task<ObjectStat?> ObterMetadadosObjeto(string nomeArquivo, string? nomeBucket = null)
        {
            try
            {
                var bucket = nomeBucket.NaoEstaPreenchido()
                    ? configuracaoArmazenamentoOptions.BucketArquivos
                    : nomeBucket;

                var args = new StatObjectArgs()
                    .WithBucket(bucket)
                    .WithObject(nomeArquivo);

                return await minioClient.StatObjectAsync(args);
            }
            catch (ObjectNotFoundException)
            {
                return null;
            }
            catch (Exception ex)
            {
                await servicoMensageriaLogs.Enviar($"Erro ao obter metadados do arquivo {nomeArquivo} do bucket {nomeBucket}: {ex.Message}", "ErroObterMetadadosArquivo", "CDEP");
                return null;
            }
        }
    }
}