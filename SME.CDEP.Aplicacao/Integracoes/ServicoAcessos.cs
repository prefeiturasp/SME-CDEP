﻿using Newtonsoft.Json;
using System.Text;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;

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

            if (!resposta.IsSuccessStatusCode) return null;
            
            var json = await resposta.Content.ReadAsStringAsync();
            var retorno = JsonConvert.DeserializeObject<UsuarioAutenticacaoRetornoDTO>(json);
            return retorno;
        }
        
        public async Task<RetornoPerfilUsuarioDTO> ObterPerfisUsuario(string login)
        {
           var resposta = await httpClient.GetAsync($"v1/autenticacao/usuarios/{login}/sistemas/{Sistema_Cdep}/perfis");

           if (!resposta.IsSuccessStatusCode) return null;
           
           var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RetornoPerfilUsuarioDTO>(json);
        }

        public async Task<bool> UsuarioCadastradoCoreSSO(string login)
        {
            var resposta = await httpClient.GetAsync($"v1/usuarios/{login}/cadastrado");

            if (!resposta.IsSuccessStatusCode) return false;
            
            var json = await resposta.Content.ReadAsStringAsync();
            var usuarioCadastradoCoreSSO = JsonConvert.DeserializeObject<bool>(json);
            return usuarioCadastradoCoreSSO;
        }

        public async Task<bool> CadastrarUsuarioCoreSSO(string login, string nome, string email, string senha)
        {
            var parametros = JsonConvert.SerializeObject(new { login, nome, email, senha });
            var resposta = await httpClient.PostAsync($"v1/usuarios/cadastrar", new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode) return false;
            
            var json = await resposta.Content.ReadAsStringAsync();
            var usuarioCadastradoCoreSSO = JsonConvert.DeserializeObject<bool>(json);
            return usuarioCadastradoCoreSSO;
        }

        public async Task<bool> VincularPerfilExternoCoreSSO(string login, Guid perfilId)
        {
            var resposta = await httpClient.PostAsync($"v1/usuarios/{login}/vincular-perfil/{perfilId}",null);

            if (!resposta.IsSuccessStatusCode) return false;
            
            var json = await resposta.Content.ReadAsStringAsync();
            var usuarioVinculadoCoreSSO = JsonConvert.DeserializeObject<bool>(json);
            return usuarioVinculadoCoreSSO;
        }
    }
}