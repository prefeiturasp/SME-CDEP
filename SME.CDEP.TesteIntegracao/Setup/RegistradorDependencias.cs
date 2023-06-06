using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Servicos.Polly;
using SME.CDEP.Infra.Servicos.Telemetria.IoC;
using SME.CDEP.IoC;

namespace SME.CDEP.TesteIntegracao.Setup
{
    public class RegistradorDependencias : RegistradorDeDependencia
    {
        private readonly IServiceCollection _serviceCollection;
        private readonly IConfiguration _configuration;
        
        public RegistradorDependencias(IServiceCollection serviceCollection, IConfiguration configuration) : base(serviceCollection, configuration)
        {
            _serviceCollection = serviceCollection;
            _configuration = configuration;
        }
        
        public override void Registrar()
        {
            RegistrarTelemetria();
            RegistrarConexao();
            RegistrarRepositorios();
            RegistrarPolly();
            RegistrarMapeamentos();
            RegistrarServicos();
        }

        protected override void RegistrarConexao()
        {
            _serviceCollection.AddScoped<ICdepConexao, CdepConexao>();
            _serviceCollection.AddScoped<ITransacao, Transacao>();
        }
        
        protected override void RegistrarMapeamentos()
        {
            FluentMapper.Initialize(config =>
            {
                config.ForDommel();
            });
        }

        protected override void RegistrarTelemetria()
        {
            _serviceCollection.ConfigurarTelemetria(_configuration);
        }
        
        protected override void RegistrarPolly()
        {
            _serviceCollection.ConfigurarPolly();
        }

        protected override void RegistrarRepositorios()
        {}

        protected override void RegistrarServicos()
        {}
        
    }
}
