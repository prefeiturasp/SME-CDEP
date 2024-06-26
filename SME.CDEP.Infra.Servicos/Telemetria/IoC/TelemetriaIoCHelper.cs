﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Servicos.Telemetria.Options;

namespace SME.CDEP.Infra.Servicos.Telemetria.IoC
{
    public static class TelemetriaIoCHelper
    {
        public static void ConfigurarTelemetria(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.EhNulo())
                return;

            services.AddOptions<TelemetriaOptions>()
                .Bind(configuration.GetSection(TelemetriaOptions.Secao), c => c.BindNonPublicProperties = true);

            services.AddSingleton<TelemetriaOptions>();
            services.AddSingleton<IServicoTelemetria, ServicoTelemetria>();
        }
    }
}
