using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.UseCase.Interface;
using SME.CDEP.Dominio.Contexto;
using System.Net;
using System.Text;

namespace SME.CDEP.Aplicacao.UseCase
{
    public class RelatorioControleDownloadAcervoUseCase(
        IHttpClientFactory httpClientFactory,
        IContextoAplicacao contextoAplicacao) : IRelatorioControleDownloadAcervoUseCase
    {
        public async Task<Stream?> ExecutarAsync(RelatorioControleDownloadAcervoRequest filtros)
        {
            var filtrosDto = new RelatorioControleDownloadAcervoDto()
            {
                TipoAcervo = filtros.TipoAcervo,
                Titulo = filtros.Titulo,
                Usuario = contextoAplicacao.NomeUsuario,
                UsuarioRF = contextoAplicacao.UsuarioLogado
            };
            var mensagem = JsonConvert.SerializeObject(new { Mensagem = filtrosDto });
            using var httpClient = httpClientFactory.CreateClient("apiSR");
            var resposta = await httpClient.PostAsync("v1/cdep/controle-download-acervo", new StringContent(mensagem, Encoding.UTF8, "application/json"));
            if (!resposta.IsSuccessStatusCode || resposta.StatusCode == HttpStatusCode.NoContent)
                return null;
            return await resposta.Content.ReadAsStreamAsync();
        }
    }
}
