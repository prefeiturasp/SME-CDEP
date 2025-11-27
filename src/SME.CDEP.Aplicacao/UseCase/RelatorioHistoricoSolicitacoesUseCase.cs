
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.UseCase.Interface;
using SME.CDEP.Dominio.Contexto;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace SME.CDEP.Aplicacao.UseCase
{
    [JsonSerializable(typeof(RequestWrapper))]
    internal partial class RelatorioJsonContext : JsonSerializerContext { }
    internal record RequestWrapper(RelatorioHistoricoSolicitacoesDto Mensagem);
    public class RelatorioHistoricoSolicitacoesUseCase(
        IHttpClientFactory httpClientFactory,
        IContextoAplicacao contextoAplicacao) : IRelatorioHistoricoSolicitacoesUseCase
    {
        public async Task<Stream?> ExecutarAsync(RelatorioHistoricoSolicitacoesRequest filtros)
        {
            var dto = new RelatorioHistoricoSolicitacoesDto(
            filtros,
            contextoAplicacao.NomeUsuario,
            contextoAplicacao.UsuarioLogado);

            var payload = new RequestWrapper(dto);
            var client = httpClientFactory.CreateClient("apiSR");

            var resposta = await client.PostAsJsonAsync(
            "v1/cdep/historico-solicitacao-acervo",
            payload,
            RelatorioJsonContext.Default.RequestWrapper);

            if (!resposta.IsSuccessStatusCode || resposta.StatusCode == HttpStatusCode.NoContent)
                return null;
            return await resposta.Content.ReadAsStreamAsync();
        }
    }
}