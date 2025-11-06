using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.UseCase.Interface;
using SME.CDEP.Dominio.Contexto;
using System.Net;
using System.Text;

namespace SME.CDEP.Aplicacao.UseCase
{
    public class RelatorioTitulosMaisPesquisadosUseCase(
        IHttpClientFactory httpClientFactory,
        IContextoAplicacao contextoAplicacao) : IRelatorioTitulosMaisPesquisadosUseCase
    {
        public async Task<Stream?> ExecutarAsync(RelatorioTitulosMaisPesquisadosRequest filtros)
        {
            var filtrosDto = new RelatorioTitulosMaisPesquisadosDto()
            {
                DataInicio = filtros.DataInicio,
                DataFim = filtros.DataFim,
                TipoAcervos = filtros.TipoAcervos,
                Usuario = contextoAplicacao.NomeUsuario,
                UsuarioRF = contextoAplicacao.UsuarioLogado
            };

            var mensagem = JsonConvert.SerializeObject(new { Mensagem = filtrosDto });

            using var httpClient = httpClientFactory.CreateClient("apiSR");
            var resposta = await httpClient.PostAsync("v1/cdep/titulos-mais-pesquisados", new StringContent(mensagem, Encoding.UTF8, "application/json"));
            if (!resposta.IsSuccessStatusCode || resposta.StatusCode == HttpStatusCode.NoContent)
                return null;
            return await resposta.Content.ReadAsStreamAsync();
        }
    }
}
