﻿using Microsoft.OpenApi.Models;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Configuracoes;
 
    public static class RegistraDocumentacaoSwagger
    {
        public static void Registrar(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();

            var versaoService = sp.GetService<IServicoGithub>();
            var versaoAtual = versaoService?.RecuperarUltimaVersao().Result;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = $"SME.CDEP.Webapi",
                    Version = versaoAtual
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Para autenticação, incluir 'Bearer' seguido do token JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                c.OperationFilter<BasicAuthOperationsFilter>();
            });

            services.AddSwaggerGen(o =>
            {
                o.OperationFilter<FiltroIntegracaoExterna>();
            });
        }
    }
