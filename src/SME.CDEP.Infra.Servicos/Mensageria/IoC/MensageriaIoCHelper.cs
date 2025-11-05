using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.CDEP.Infra.Servicos.Mensageria.Options;
using SME.ConectaFormacao.Infra.Servicos.Mensageria;

namespace SME.CDEP.Infra.Servicos.Mensageria.IoC
{
    public static class MensageriaIoCHelper
    {
        public static void ConfigurarMensageria(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
                return;

            services.AddOptions<MensageriaOptions>()
                .Bind(configuration.GetSection(MensageriaOptions.Secao), c => c.BindNonPublicProperties = true);

            services.AddTransient<MensageriaOptions>();
            services.AddTransient<IServicoMensageriaLogs, ServicoMensageriaLogs>();
            services.AddTransient<IServicoMensageriaCDEP, ServicoMensageriaCDEP>();
            services.AddTransient<IServicoMensageriaMetricas, ServicoMensageriaMetricas>();
        }
    }
}
