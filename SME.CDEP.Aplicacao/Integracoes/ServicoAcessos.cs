using Newtonsoft.Json;
using System.Text;
using SME.CDEP.Aplicacao.DTOS;
using SSME.CDEP.Aplicacao.Integracoes.Interfaces;

namespace SSME.CDEP.Aplicacao.Integracoes
{
    public class ServicoAcessos : IServicoAcessos
    {
        private readonly HttpClient httpClient;
        private const int Sistema_Cdep = 1000;

        public ServicoAcessos(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<UsuarioAutenticacaoRetornoDTO> Autenticar(string login, string senha)
        {
            var parametros = JsonConvert.SerializeObject(new { login, senha });
            var resposta = await httpClient.PostAsync($"v1/autenticacao/autenticar", new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode) return null;
            
            var json = await resposta.Content.ReadAsStringAsync();
            var retorno = JsonConvert.DeserializeObject<UsuarioAutenticacaoRetornoDTO>(json);
            return retorno;
        }
        
        public async Task<RetornoPerfilUsuarioDTO> ObterPerfisUsuario(string login)
        {
           var resposta = await httpClient.GetAsync($"v1/autenticacao/{login}/{Sistema_Cdep}/perfis/listar");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<RetornoPerfilUsuarioDTO>(json);
            }
            return null;
        }
    }
}