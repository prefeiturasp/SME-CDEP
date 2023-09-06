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
using SME.CDEP.Infra.Servicos.Mensageria;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;
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
            _serviceCollection.TryAddScoped<IServicoPerfilUsuario, ServicoPerfilUsuario>();
            _serviceCollection.TryAddScoped<IServicoAcessoDocumento, ServicoAcessoDocumento>();
            _serviceCollection.TryAddScoped<IServicoConservacao, ServicoConservacao>();
            _serviceCollection.TryAddScoped<IServicoCromia, ServicoCromia>();
            _serviceCollection.TryAddScoped<IServicoFormato, ServicoFormato>();
            _serviceCollection.TryAddScoped<IServicoIdioma, ServicoIdioma>();
            _serviceCollection.TryAddScoped<IServicoMaterial, ServicoMaterial>();
            _serviceCollection.TryAddScoped<IServicoSuporte, ServicoSuporte>();
            _serviceCollection.TryAddScoped<IServicoCreditoAutor, ServicoCreditoAutor>();
            _serviceCollection.TryAddScoped<IServicoEditora, ServicoEditora>();
            _serviceCollection.TryAddScoped<IServicoAssunto, ServicoAssunto>();
            _serviceCollection.TryAddScoped<IServicoSerieColecao, ServicoSerieColecao>();
            _serviceCollection.TryAddScoped<IServicoMenu, ServicoMenu>();
            _serviceCollection.TryAddScoped<IServicoAcervo, ServicoAcervoAuditavel>();
            _serviceCollection.TryAddScoped<IServicoAcervoFotografico, ServicoAcervoFotografico>();
            _serviceCollection.TryAddScoped<IServicoUploadArquivo, ServicoUploadArquivo>();
            _serviceCollection.TryAddScoped<IServicoExcluirArquivo, ServicoExcluirArquivo>();
            _serviceCollection.TryAddScoped<IServicoArmazenamentoArquivoFisico, ServicoArmazenamentoArquivoFisico>();
            _serviceCollection.TryAddScoped<IServicoArmazenamento, ServicoArmazenamento>();
            _serviceCollection.TryAddScoped<IServicoDownloadArquivo, ServicoDownloadArquivo>();
            _serviceCollection.TryAddScoped<IServicoMoverArquivoTemporario, ServicoMoverArquivoTemporario>();
            _serviceCollection.TryAddScoped<IServicoMensageria, ServicoMensageria>();
        }
        protected override void RegistrarHttpClients()
        {}

        protected virtual void RegistrarLogs()
        { }
    }
}