using SME.CDEP.Aplicacao.Extensions;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;
using System.Drawing;
using System.Net;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoGerarMiniatura : IServicoGerarMiniatura
    {
        private readonly IServicoArmazenamento servicoArmazenamento;
        private readonly IRepositorioArquivo repositorioArquivo;

        public ServicoGerarMiniatura(IServicoArmazenamento servicoArmazenamento, IRepositorioArquivo repositorioArquivo)
        {
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
        }

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

            using (WebClient webClient = new WebClient())
            {
                using (Stream stream = webClient.OpenRead(url))
                {
                    using (Image imagem = Image.FromStream(stream))
                    {
                        var miniatura = imagem.GetThumbnailImage(320, 200, () => false, IntPtr.Zero);

                        using (var msImagem = new MemoryStream())
                        {
                            miniatura.Save(msImagem, tipoConteudo.ObterFormato());

                            msImagem.Seek(0, SeekOrigin.Begin);

                            await servicoArmazenamento.Armazenar(codigoArquivoMiniaturaComExtensao, msImagem, tipoConteudo);
                        }
                    }
                }
            }
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