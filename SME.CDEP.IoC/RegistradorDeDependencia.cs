using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Elastic.Apm.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using SME.CDEP.Aplicacao.Integracoes;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.Aplicacao.Mapeamentos;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Dados.Mapeamentos;
using SME.CDEP.Infra.Dados.Repositorios;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Servicos.Log;
using SME.CDEP.Infra.Servicos.Options;
using SME.CDEP.Infra.Servicos.Polly;
using SME.CDEP.Infra.Servicos.Telemetria.IoC;
using SME.CDEP.IoC.Extensions;

namespace SME.CDEP.IoC;

public class RegistradorDeDependencia
{
    private readonly IServiceCollection _serviceCollection;
    private readonly IConfiguration _configuration;

    public RegistradorDeDependencia(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        _serviceCollection = serviceCollection;
        _configuration = configuration;
    }
    public virtual void Registrar()
    {
        RegistrarTelemetria();
        RegistrarConexao();
        RegistrarRepositorios();
        RegistrarLogs();
        RegistrarPolly();
        RegistrarMapeamentos();
        RegistrarServicos();
        RegistrarProfiles();
        RegistrarHttpClients();
    }
    
    protected virtual void RegistrarProfiles()
    {
        _serviceCollection.AddAutoMapper(typeof(DominioParaDTOProfile));
    }
    
    protected virtual void RegistrarLogs()
    {
        _serviceCollection.AddOptions<ConfiguracaoRabbitLogsOptions>()
            .Bind(_configuration.GetSection(ConfiguracaoRabbitLogsOptions.Secao), c => c.BindNonPublicProperties = true);

        _serviceCollection.AddSingleton<ConfiguracaoRabbitLogsOptions>();
        _serviceCollection.AddSingleton<IConexoesRabbitLogs>(serviceProvider =>
        {
            var options = serviceProvider.GetService<IOptions<ConfiguracaoRabbitLogsOptions>>().Value;
            var provider = serviceProvider.GetService<IOptions<DefaultObjectPoolProvider>>().Value;
            return new ConexoesRabbitLogs(options, provider);
        });

        _serviceCollection.AddSingleton<IServicoLogs, ServicoLogs>();
    }

    protected virtual void RegistrarMapeamentos()
    {
        FluentMapper.Initialize(config =>
        {
            config.AddMap(new UsuarioMap());
            config.AddMap(new AcessoDocumentoMap());
            config.AddMap(new ConservacaoMap());
            config.AddMap(new CromiaMap());
            config.AddMap(new FormatoMap());
            config.AddMap(new IdiomaMap());
            config.AddMap(new MaterialMap());
            config.AddMap(new SuporteMap());
            config.AddMap(new TipoAnexoMap());

            config.ForDommel();
        });
    }

    protected virtual void RegistrarTelemetria()
    {
        _serviceCollection.ConfigurarTelemetria(_configuration);
    }

    protected virtual void RegistrarConexao()
    {
        _serviceCollection.AddScoped<ICdepConexao, CdepConexao>(_ => new CdepConexao(_configuration.GetConnectionString("conexao")));
        _serviceCollection.AddScoped<ITransacao, Transacao>();
    }

    protected virtual void RegistrarPolly()
    {
        _serviceCollection.ConfigurarPolly();
    }

    protected virtual void RegistrarRepositorios()
    {
        _serviceCollection.TryAddScoped<IRepositorioUsuario, RepositorioUsuario>();
        _serviceCollection.TryAddScoped<IRepositorioAcessoDocumento, RepositorioAcessoDocumento>();
        _serviceCollection.TryAddScoped<IRepositorioConservacao, RepositorioConservacao>();
        _serviceCollection.TryAddScoped<IRepositorioCromia, RepositorioCromia>();
        _serviceCollection.TryAddScoped<IRepositorioFormato, RepositorioFormato>();
        _serviceCollection.TryAddScoped<IRepositorioIdioma, RepositorioIdioma>();
        _serviceCollection.TryAddScoped<IRepositorioMaterial, RepositorioMaterial>();
        _serviceCollection.TryAddScoped<IRepositorioSuporte, RepositorioSuporte>();
        _serviceCollection.TryAddScoped<IRepositorioTipoAnexo, RepositorioTipoAnexo>();
    }

    protected virtual void RegistrarServicos()
    {
        _serviceCollection.TryAddScoped<IServicoUsuario, ServicoUsuario>();
        _serviceCollection.TryAddScoped<IServicoPerfilUsuario, ServicoPerfilUsuario>();
        _serviceCollection.TryAddScoped<IServicoAcessos, ServicoAcessos>();
        _serviceCollection.TryAddScoped<IServicoAcessoDocumento, ServicoAcessoDocumento>();
        _serviceCollection.TryAddScoped<IServicoConservacao, ServicoConservacao>();
        _serviceCollection.TryAddScoped<IServicoCromia, ServicoCromia>();
        _serviceCollection.TryAddScoped<IServicoFormato, ServicoFormato>();
    }
    protected virtual void RegistrarHttpClients()
    {
        _serviceCollection.AdicionarHttpClients(_configuration);
    }
}
