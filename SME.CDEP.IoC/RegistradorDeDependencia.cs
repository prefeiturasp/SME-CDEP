using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Elastic.Apm.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Dominios;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Dados.Mapeamentos;
using SME.CDEP.Infra.Dados.Repositorios;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Servicos.Polly;
using SME.CDEP.Infra.Servicos.Telemetria.IoC;

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
        RegistrarPolly();
        RegistrarMapeamentos();
        RegistrarServicos();
    }

    private void RegistrarMapeamentos()
    {
        FluentMapper.Initialize(config =>
        {
            config.AddMap(new UsuarioMap());

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
    }

    protected virtual void RegistrarServicos()
    {
        _serviceCollection.TryAddScoped<IServicoUsuario, ServicoUsuario>();
    }
}
