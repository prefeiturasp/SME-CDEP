using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Aplicacao.UseCase.Interface;
using SME.CDEP.Dominio.Contexto;
using System.Net;
using System.Text;

namespace SME.CDEP.Aplicacao.UseCase
{
    public class RelatorioControleAcervoAutorUseCase : IRelatorioControleAcervoAutorUseCase
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IServicoAcervo servicoAcervo;
        private readonly IContextoAplicacao contextoAplicacao;

        public RelatorioControleAcervoAutorUseCase(IHttpClientFactory httpClientFactory, IServicoAcervo servicoAcervo, IContextoAplicacao contextoAplicacao)
        {
            this.httpClientFactory = httpClientFactory;
            this.servicoAcervo = servicoAcervo;
            this.contextoAplicacao = contextoAplicacao;
        }

        public async Task<Stream> Executar(RelatorioControleAcervoAutorRequest filtros)
        {
            var tiposAcervosPermitidos = servicoAcervo.ObterTiposAcervosPermitidosDoPerfilLogado();
            var filtrosValidos = new RelatorioControleAcervoAutorDTO()
            {
                Usuario = contextoAplicacao.NomeUsuario,
                UsuarioRF = contextoAplicacao.UsuarioLogado,
                Autores = filtros.Autores,
                TipoAcervo = filtros.TipoAcervo,
                TiposAcervosPermitidos = tiposAcervosPermitidos
            };

            var mensagem = JsonConvert.SerializeObject(new { Mensagem = filtrosValidos });


            using (var httpClient = httpClientFactory.CreateClient("apiSR"))
            {
                var resposta = await httpClient.PostAsync("v1/cdep/controle-acervo-autor", new StringContent(mensagem, Encoding.UTF8, "application/json"));

                if (!resposta.IsSuccessStatusCode || resposta.StatusCode == HttpStatusCode.NoContent)
                    return null;

                return await resposta.Content.ReadAsStreamAsync();
            }
        }
    }
}
