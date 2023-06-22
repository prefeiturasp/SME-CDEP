using Newtonsoft.Json;
using System.Text;
using SME.CDEP.Aplicacao.DTO;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Excecoes;

namespace SME.CDEP.Aplicacao.Integracoes
{
    public class ServicoAcessos : IServicoAcessos
    {
        private readonly HttpClient httpClient;
        private const int Sistema_Cdep = 1006;

        public ServicoAcessos(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<UsuarioAutenticacaoRetornoDTO> Autenticar(string login, string senha)
        {
            var parametros = JsonConvert.SerializeObject(new { login, senha });
            var resposta = await httpClient.PostAsync($"v1/autenticacao/autenticar", new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException(MensagemNegocio.USUARIO_OU_SENHA_INVALIDOS);
            
            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UsuarioAutenticacaoRetornoDTO>(json);
        }
        
        public async Task<RetornoPerfilUsuarioDTO> ObterPerfisUsuario(string login)
        {
           var resposta = await httpClient.GetAsync($"v1/autenticacao/usuarios/{login}/sistemas/{Sistema_Cdep}/perfis");

           if (!resposta.IsSuccessStatusCode)
               throw new NegocioException(MensagemNegocio.PERFIS_DO_USUARIO_NAO_LOCALIZADOS_VERIFIQUE_O_LOGIN);
           
           var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RetornoPerfilUsuarioDTO>(json);
        }

        public async Task<bool> UsuarioCadastradoCoreSSO(string login)
        {
            var resposta = await httpClient.GetAsync($"v1/usuarios/{login}/cadastrado");

            if (!resposta.IsSuccessStatusCode) return false;
            
            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(json);
        }

        public async Task<bool> CadastrarUsuarioCoreSSO(string login, string nome, string email, string senha)
        {
            var parametros = JsonConvert.SerializeObject(new { login, nome, email, senha });
            var resposta = await httpClient.PostAsync($"v1/usuarios/cadastrar", new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode) return false;
            
            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(json);
        }

        public async Task<bool> VincularPerfilExternoCoreSSO(string login, Guid perfilId)
        {
            var resposta = await httpClient.PostAsync($"v1/usuarios/{login}/vincular-perfil/{perfilId}",null);

            if (!resposta.IsSuccessStatusCode) return false;
            
            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(json);
        }

        public async Task<DadosUsuarioDTO> ObterMeusDados(string login)
        {
            var resposta = await httpClient.GetAsync($"v1/usuarios/{login}/sistemas/{Sistema_Cdep}/meus-dados");

            if (!resposta.IsSuccessStatusCode) return new DadosUsuarioDTO();
            
            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DadosUsuarioDTO>(json);
        }
    }
}