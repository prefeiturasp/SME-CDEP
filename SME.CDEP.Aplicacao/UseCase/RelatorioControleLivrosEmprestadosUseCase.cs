using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Aplicacao.UseCase.Interface;
using SME.CDEP.Dominio.Contexto;
using System.Net;
using System.Text;

namespace SME.CDEP.Aplicacao.UseCase
{
    public class RelatorioControleLivrosEmprestadosUseCase : IRelatorioControleLivrosEmprestadosUseCase
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;
        private readonly IServicoAcervo servicoAcervo;
        private readonly IContextoAplicacao contextoAplicacao;

        public RelatorioControleLivrosEmprestadosUseCase(IHttpClientFactory httpClientFactory, IConfiguration configuration, IServicoAcervo servicoAcervo, IContextoAplicacao contextoAplicacao)
        {
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
            this.servicoAcervo = servicoAcervo ?? throw new ArgumentNullException(nameof(servicoAcervo));
            this.contextoAplicacao = contextoAplicacao;
        }

        public async Task<Stream> Executar(RelatorioControleLivroEmprestadosRequest filtros)
        {
            var tiposAcervosPermitidos = servicoAcervo.ObterTiposAcervosPermitidosDoPerfilLogado();
            var filtrosValidos = new RelatorioControleLivroEmprestadosDTO()
            {
                Solicitante = filtros.Solicitante,
                Tombo = filtros.Tombo,
                Modelo = filtros.Modelo,
                Usuario = contextoAplicacao.NomeUsuario,
                UsuarioRF = contextoAplicacao.UsuarioLogado,
                SituacaoSolicitacaoItem = filtros.SituacaoSolicitacaoItem,
                SituacaoEmprestimo = filtros.SituacaoEmprestimo,
                SomenteDevolvidos = filtros.SomenteDevolvidos,
                TiposAcervosPermitidos = tiposAcervosPermitidos
            };

            var mensagem = JsonConvert.SerializeObject(new { Mensagem = filtrosValidos });


            using (var httpClient = httpClientFactory.CreateClient("apiSR"))
            {
                var resposta = await httpClient.PostAsync("v1/cdep/controle-livros-emprestados", new StringContent(mensagem, Encoding.UTF8, "application/json"));

                if (!resposta.IsSuccessStatusCode || resposta.StatusCode == HttpStatusCode.NoContent)
                    return null;

                return await resposta.Content.ReadAsStreamAsync();
            }
        }
    }
}
