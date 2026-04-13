using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoGerarMiniatura(IServicoArmazenamento servicoArmazenamento, IRepositorioArquivo repositorioArquivo,
        IHttpClientFactory httpClientFactory) : IServicoGerarMiniatura
    {
        public async Task<long> GerarMiniatura(string tipoConteudo, string nomeArquivoFisico, string nomeArquivoMiniatura, TipoArquivo tipoArquivo)
        {
            var codigoArquivoMiniatura = Guid.NewGuid();

            var codigoArquivoMiniaturaComExtensao = $"{codigoArquivoMiniatura}{nomeArquivoFisico.ObterExtensao()}";

            await ArmazenarMiniatura(tipoConteudo, nomeArquivoFisico, codigoArquivoMiniaturaComExtensao);

            return await SalvarArquivoMiniaturaAsync(nomeArquivoMiniatura, tipoConteudo, tipoArquivo, codigoArquivoMiniatura);
        }

        protected async Task ArmazenarMiniatura(string tipoConteudo, string nomeArquivoFisico, string codigoArquivoMiniaturaComExtensao)
        {
            var url = await servicoArmazenamento.Obter(nomeArquivoFisico, false);

            using var httpClient = httpClientFactory.CreateClient();
            using var stream = await httpClient.GetStreamAsync(url);

            using var imagem = await Image.LoadAsync(stream);

            imagem.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(320, 200),
                Mode = ResizeMode.Max
            }));

            using var msImagem = new MemoryStream();

            var formatoOriginal = imagem.Metadata.DecodedImageFormat;
            await imagem.SaveAsync(msImagem, formatoOriginal!);

            msImagem.Seek(0, SeekOrigin.Begin);

            await servicoArmazenamento.Armazenar(codigoArquivoMiniaturaComExtensao, msImagem, tipoConteudo);
        }

        protected async Task<long> SalvarArquivoMiniaturaAsync(string nomeArquivoMiniatura, string tipoConteudo, TipoArquivo tipoArquivo, Guid codigoArquivoMiniatura)
        {
            return await repositorioArquivo.SalvarAsync(new Arquivo()
            {
                Nome = nomeArquivoMiniatura,
                TipoConteudo = tipoConteudo,
                Codigo = codigoArquivoMiniatura,
                Tipo = tipoArquivo
            });
        }
    }
}