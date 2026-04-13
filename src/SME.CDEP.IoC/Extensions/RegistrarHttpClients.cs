using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using SME.CDEP.Aplicacao.Integracoes;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SME.CDEP.IoC.Extensions;

[ExcludeFromCodeCoverage]
internal static class RegistrarHttpClients
{
    internal static void AdicionarHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IServicoAcessos, ServicoAcessos>(c =>
         {
             c.BaseAddress = new Uri(configuration.GetSection("UrlApiAcessos").Value);
             c.DefaultRequestHeaders.Add("Accept", "application/json");
             c.DefaultRequestHeaders.Add("x-api-acessos-key", configuration.GetSection("ApiKeyAcessosApi").Value);
         });

        services.AddHttpClient(name: "servicoAcessos", c =>
        {
            c.BaseAddress = new Uri(configuration.GetSection("UrlApiAcessos").Value);
            c.DefaultRequestHeaders.Add("Accept", "application/json");
            c.DefaultRequestHeaders.Add("x-api-acessos-key", configuration.GetSection("ApiKeyAcessosApi").Value);

        }).AddPolicyHandler(GetRetryPolicy());
        services.AddHttpClient();
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(3, retryAttempt)));
    }
}