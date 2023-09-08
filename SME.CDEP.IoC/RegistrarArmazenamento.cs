using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.CDEP.Infra.Servicos.Mensageria;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;

namespace SME.CDEP.IoC
{
    internal static class RegistrarArmazenamento
    {
        internal static void ConfigurarArmazenamento(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<ConfiguracaoArmazenamentoOptions>()
                .Bind(configuration.GetSection(ConfiguracaoArmazenamentoOptions.Secao), c => c.BindNonPublicProperties = true);

            services.AddSingleton<ConfiguracaoArmazenamentoOptions>();
            services.AddSingleton<IServicoMensageria, ServicoMensageria>();
            services.AddSingleton<IServicoArmazenamento, ServicoArmazenamento>();
        }
    }
}
