using Newtonsoft.Json;
using System.Text;
using SME.CDEP.Aplicacao.Dtos;
using SSME.CDEP.Aplicacao.Integracoes.Interfaces;

namespace SSME.CDEP.Aplicacao.Integracoes
{
    public class ServicoAcessos : IServicoAcessos
    {
        private readonly HttpClient httpClient;

        public ServicoAcessos(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<UsuarioAutenticacaoRetornoDto> Autenticar(string login, string senha)
        {
            var parametros = JsonConvert.SerializeObject(new { login, senha });
            var resposta = await httpClient.PostAsync($"v1/autenticacao-cdep/autenticar", new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode) return null;
            
            var json = await resposta.Content.ReadAsStringAsync();
            var retorno = JsonConvert.DeserializeObject<UsuarioAutenticacaoRetornoDto>(json);
            return retorno;
        }
    }
}