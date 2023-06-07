using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Dados.Repositorios;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Servicos.Polly;
using SME.CDEP.Infra.Servicos.Telemetria.IoC;
using SME.CDEP.IoC;
using SME.CDEP.TesteIntegracao.ServicosFakes;
using SSME.CDEP.Aplicacao.Integracoes;
using SSME.CDEP.Aplicacao.Integracoes.Interfaces;

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
            RegistrarHttpClients();
        }

        protected override void RegistrarConexao()
        {
            _serviceCollection.AddScoped<ICdepConexao, CdepConexao>();
            _serviceCollection.AddScoped<ITransacao, Transacao>();
        }

        // protected override void RegistrarMapeamentos()
        // {
        //     FluentMapper.Initialize(config =>
        //     {
        //         config.ForDommel();
        //     });
        // }

        // protected override void RegistrarTelemetria()
        // {
        //     _serviceCollection.ConfigurarTelemetria(_configuration);
        // }
        //
        // protected override void RegistrarPolly()
        // {
        //     _serviceCollection.ConfigurarPolly();
        // }

        // protected override void RegistrarRepositorios()
        // {
        //     _serviceCollection.TryAddScoped<IRepositorioUsuario, RepositorioUsuario>();
        // }
        //
        protected override void RegistrarServicos()
        {
            _serviceCollection.TryAddScoped<IServicoUsuario, ServicoUsuario>();
            _serviceCollection.TryAddScoped<IServicoAcessos, ServicoAcessosFake>();
        }
        protected override void RegistrarHttpClients()
        {}
    }
}