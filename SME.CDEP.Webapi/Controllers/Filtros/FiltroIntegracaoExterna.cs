
using Microsoft.OpenApi.Models;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Webapi.Middlewares;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SME.CDEP.Webapi.Filtros
{
    public class FiltroIntegracaoExterna : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

            var attributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                                    .Union(context.MethodInfo.GetCustomAttributes(true))
                                    .OfType<ChaveIntegracaoCdepApi>();

            if (attributes.NaoEhNulo() && attributes.Any())
            {

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "x-cdep-api-key",
                    In = ParameterLocation.Header,
                    Required = false
                });
            }
        }
    }
}
