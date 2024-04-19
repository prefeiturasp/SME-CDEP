using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;

namespace SME.CDEP.Aplicacao.Integracoes
{
    public class ServicoCEP : IServicoCEP
    {
        private readonly HttpClient httpClient;

        public ServicoCEP(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<CEPDTO> BuscarCEP(string cep)
        {
            var resposta = await httpClient.GetAsync($"https://opencep.com/v1/{cep}");

            if (!resposta.IsSuccessStatusCode) return default;

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CEPDTO>(json);
        }
    }
}