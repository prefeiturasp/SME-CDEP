using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.CDEP.IoC;

namespace SME.CDEP.TesteIntegracao.Setup
{
    public class RegistradorDependencias : RegistradorDeDependencia
    {
        public RegistradorDependencias(IServiceCollection serviceCollection, IConfiguration configuration) : base(serviceCollection, configuration)
        {
           
        }
    }
}
