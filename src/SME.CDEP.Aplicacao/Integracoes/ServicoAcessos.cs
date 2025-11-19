using Newtonsoft.Json;
using System.Text;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;

namespace SME.CDEP.Aplicacao.Integracoes
{
    public class ServicoAcessos(HttpClient httpClient) : IServicoAcessos
    {
        private const int Sistema_Cdep = 1006;

        public async Task<UsuarioAutenticacaoRetornoDTO> Autenticar(string login, string senha)
        {
            var parametros = JsonConvert.SerializeObject(new { login, senha });
            var resposta = await httpClient.PostAsync(ConstantesServicoAcessos.URL_AUTENTICACAO_AUTENTICAR, new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException(MensagemNegocio.USUARIO_OU_SENHA_INVALIDOS);
            
            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UsuarioAutenticacaoRetornoDTO>(json)!;
        }

        public async Task<RetornoPerfilUsuarioDTO> ObterPerfisUsuario(string login, Guid? perfilUsuarioId)
        {
            var resposta = await httpClient.GetAsync(string.Format(ConstantesServicoAcessos.URL_AUTENTICACAO_USUARIOS_X_SISTEMAS_Y_PERFIS_Z,login,Sistema_Cdep,perfilUsuarioId));

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException(MensagemNegocio.PERFIS_DO_USUARIO_NAO_LOCALIZADOS_VERIFIQUE_O_LOGIN);

            var json = await resposta.Content.ReadAsStringAsync();
            return json.JsonParaObjeto<RetornoPerfilUsuarioDTO>();
        }

        public async Task<RetornoPerfilUsuarioDTO> ObterPerfisUsuario(string login)
        {
           var resposta = await httpClient.GetAsync(string.Format(ConstantesServicoAcessos.URL_AUTENTICACAO_USUARIOS_X_SISTEMAS_Y_PERFIS,login,Sistema_Cdep));

           if (!resposta.IsSuccessStatusCode)
               throw new NegocioException(MensagemNegocio.PERFIS_DO_USUARIO_NAO_LOCALIZADOS_VERIFIQUE_O_LOGIN);
           
           var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RetornoPerfilUsuarioDTO>(json)!;
        }

        public async Task<bool> UsuarioCadastradoCoreSSO(string login)
        {
            var resposta = await httpClient.GetAsync(string.Format(ConstantesServicoAcessos.URL_USUARIOS_X_CADASTRADO,login));

            if (!resposta.IsSuccessStatusCode) return false;
            
            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(json);
        }

        public async Task<bool> CadastrarUsuarioCoreSSO(string login, string nome, string email, string senha)
        {
            var parametros = JsonConvert.SerializeObject(new { login, nome, email, senha });
            var resposta = await httpClient.PostAsync(ConstantesServicoAcessos.URL_USUARIOS_CADASTRAR, new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode) return false;
            
            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(json);
        }

        public async Task<bool> VincularPerfilExternoCoreSSO(string login, Guid perfilId)
        {
            var resposta = await httpClient.PostAsync(string.Format(ConstantesServicoAcessos.URL_USUARIOS_X_VINCULAR_PERFIL_Y,login,perfilId),null);

            if (!resposta.IsSuccessStatusCode) return false;
            
            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(json);
        }

        public async Task<DadosUsuarioDTO> ObterMeusDados(string login)
        {
            var resposta = await httpClient.GetAsync(string.Format(ConstantesServicoAcessos.URL_USUARIOS_X,login));

            if (!resposta.IsSuccessStatusCode) return new DadosUsuarioDTO();
            
            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DadosUsuarioDTO>(json)!;
        }

        public Task<bool> AlterarSenha(string login, string senhaAtual, string senhaNova)
        {
            return InvocarPutServicoAcessosRetornandoBool(string.Format(ConstantesServicoAcessos.URL_USUARIOS_X_SENHA,login), 
                JsonConvert.SerializeObject(new { login, senhaAtual, senhaNova, sistemaId = Sistema_Cdep }));
        }

        public Task<bool> AlterarEmail(string login, string email)
        {
            return InvocarPutServicoAcessosRetornandoBool(string.Format(ConstantesServicoAcessos.URL_USUARIOS_X_EMAIL,login), JsonConvert.SerializeObject(new { login, email }));
        }

        private async Task<bool> InvocarPutServicoAcessosRetornandoBool(string rota, string parametros)
        {
            var resposta = await httpClient.PutAsync(rota,new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode) return false;

            var json = await resposta.Content.ReadAsStringAsync();
            var retorno = JsonConvert.DeserializeObject<bool>(json);
            return retorno;
        }
        
        public async Task<string> SolicitarRecuperacaoSenha(string login)
        {
            var resposta = await httpClient.GetAsync(string.Format(ConstantesServicoAcessos.URL_USUARIOS_X_SISTEMAS_Y_RECUPERAR_SENHA,login,Sistema_Cdep));

            if (!resposta.IsSuccessStatusCode) return string.Empty;

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<string>(json)!;
        }

        public async Task<bool> TokenRecuperacaoSenhaEstaValido(Guid token)
        {
            var resposta = await httpClient.GetAsync(string.Format(ConstantesServicoAcessos.URL_USUARIOS_X_SISTEMAS_Y_VALIDAR,token,Sistema_Cdep));

            if (!resposta.IsSuccessStatusCode) return false;

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(json);
        }

        public async Task<string> AlterarSenhaComTokenRecuperacao(RecuperacaoSenhaDto recuperacaoSenhaDto)
        {
            var parametros = JsonConvert.SerializeObject(new { token = recuperacaoSenhaDto.Token, senha = recuperacaoSenhaDto.NovaSenha });
            
            var resposta = await httpClient.PutAsync(string.Format(ConstantesServicoAcessos.URL_USUARIOS_SISTEMAS_X_SENHA,Sistema_Cdep), new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode) return string.Empty;

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<string>(json)!;
        }

        public async Task<RetornoPerfilUsuarioDTO> RevalidarToken(string token)
        {
            var parametros = new { token }.ObjetoParaJson();
            
            var resposta = await httpClient.PostAsync(ConstantesServicoAcessos.URL_AUTENTICACAO_REVALIDAR, new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException(MensagemNegocio.TOKEN_INVALIDO);

            var json = await resposta.Content.ReadAsStringAsync();
            return json.JsonParaObjeto<RetornoPerfilUsuarioDTO>();
        }

        public async Task<IEnumerable<ResponsavelDTO>> ObterUsuariosComPerfisResponsavel(Guid[] perfis)
        {
            var parametros = perfis.ToList().Aggregate("?", (current, perfil) => current + $"&perfis={perfil}");

            parametros += $"&sistemaid={Sistema_Cdep}";

            var resposta = await httpClient.GetAsync($"{ConstantesServicoAcessos.URL_USUARIOS_PERFIS_RESPONSAVEIS}{parametros}");

            if (!resposta.IsSuccessStatusCode) return default!;

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ResponsavelDTO>>(json)!;
        }
    }
}