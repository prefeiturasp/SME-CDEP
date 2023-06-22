using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dados;
using SME.CDEP.IoC;
using SME.CDEP.TesteIntegracao.ServicosFakes;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.Aplicacao.Mapeamentos;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Webapi.Contexto;

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
            RegistrarLogs();
            RegistrarPolly();
            RegistrarMapeamentos();
            RegistrarServicos();
            RegistrarProfiles();
            RegistrarContextos();
            RegistrarHttpClients();
        }

        protected void RegistrarContextos()
        {
            _serviceCollection.TryAddScoped<IHttpContextAccessor, HttpContextAccessorFake>();
            _serviceCollection.TryAddScoped<IContextoAplicacao, ContextoHttp>();
        }
        
        protected override void RegistrarProfiles()
        {
            _serviceCollection.AddAutoMapper(typeof(DominioParaDTOProfile));
        }
        
        protected override void RegistrarConexao()
        {
            _serviceCollection.AddScoped<ICdepConexao, CdepConexao>();
            _serviceCollection.AddScoped<ITransacao, Transacao>();
        }
        protected override void RegistrarServicos()
        {
            _serviceCollection.TryAddScoped<IServicoUsuario, ServicoUsuario>();
            _serviceCollection.TryAddScoped<IServicoAcessos, ServicoAcessosFake>();
        }
        protected override void RegistrarHttpClients()
        {}

        protected virtual void RegistrarLogs()
        { }
    }
}