using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Aplicacao.UseCase.Interface;
using SME.CDEP.Dominio.Contexto;
using System.Net;
using System.Text;

namespace SME.CDEP.Aplicacao.UseCase
{
    public class RelatorioControleLivrosEmprestadosUseCase(IHttpClientFactory httpClientFactory, IServicoAcervo servicoAcervo, IContextoAplicacao contextoAplicacao) : 
        IRelatorioControleLivrosEmprestadosUseCase
    {
        private readonly IServicoAcervo servicoAcervo = servicoAcervo ?? throw new ArgumentNullException(nameof(servicoAcervo));

        public async Task<Stream?> ExecutarAsync(RelatorioControleLivroEmprestadosRequest filtros)
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


            using var httpClient = httpClientFactory.CreateClient("apiSR");
            var resposta = await httpClient.PostAsync("v1/cdep/controle-livros-emprestados", new StringContent(mensagem, Encoding.UTF8, "application/json"));

            if (!resposta.IsSuccessStatusCode || resposta.StatusCode == HttpStatusCode.NoContent)
                return null;

            return await resposta.Content.ReadAsStreamAsync();
        }
    }
}