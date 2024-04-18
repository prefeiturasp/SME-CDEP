using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using SME.CDEP.Aplicacao;
using SME.CDEP.Aplicacao.Integracoes;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.Aplicacao.Mapeamentos;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Dados.Mapeamentos;
using SME.CDEP.Infra.Dados.Repositorios;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Servicos.Mensageria;
using SME.CDEP.Infra.Servicos.Mensageria.IoC;
using SME.CDEP.Infra.Servicos.Mensageria.Log;
using SME.CDEP.Infra.Servicos.Options;
using SME.CDEP.Infra.Servicos.Polly;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;
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
        RegistrarRabbit();
        RegistrarPolly();
        RegistrarMapeamentos();
        RegistrarServicos();
        RegistrarCasosDeUso();
        RegistrarProfiles();
        RegistrarHttpClients();
        RegistrarServicoArmazenamento(_serviceCollection, _configuration);
        ConfigurarMensageria();
    }

    protected virtual void RegistrarCasosDeUso()
    {
        _serviceCollection.TryAddScoped<IExecutarCriacaoDeEventosTipoFeriadoAnoAtualUseCase, ExecutarCriacaoDeEventosTipoFeriadoAnoAtualUseCase>();
        _serviceCollection.TryAddScoped<IExecutarCriacaoDeEventosTipoFeriadoAnoAtualPorDataUseCase, ExecutarCriacaoDeEventosTipoFeriadoAnoAtualPorDataUseCase>();
        
        _serviceCollection.TryAddScoped<INotificarViaEmailCancelamentoAtendimentoUseCase, NotificarViaEmailCancelamentoAtendimentoUseCaseUseCase>();
        _serviceCollection.TryAddScoped<INotificarViaEmailCancelamentoAtendimentoItemUseCase, NotificarViaEmailCancelamentoAtendimentoItemUseCaseUseCase>();
        _serviceCollection.TryAddScoped<INotificarViaEmailConfirmacaoAtendimentoPresencialUseCase, NotificarViaEmailConfirmacaoAtendimentoPresencialUseCaseUseCase>();
        
        _serviceCollection.TryAddScoped<IExecutarAtualizacaoSituacaoParaEmprestimoComDevolucaoEmAtrasoUseCase, ExecutarAtualizacaoSituacaoParaEmprestimoComDevolucaoEmAtrasoUseCase>();
        
        _serviceCollection.TryAddScoped<INotificacaoVencimentoEmprestimoUseCase, NotificacaoVencimentoEmprestimoUseCase>();
        _serviceCollection.TryAddScoped<INotificacaoVencimentoEmprestimoUsuarioUseCase, NotificacaoVencimentoEmprestimoUsuarioUseCase>();
        
        _serviceCollection.TryAddScoped<INotificacaoDevolucaoEmprestimoAtrasadoUseCase, NotificacaoDevolucaoEmprestimoAtrasadoUseCase>();
        _serviceCollection.TryAddScoped<INotificacaoDevolucaoEmprestimoAtrasadoUsuarioUseCase, NotificacaoDevolucaoEmprestimoAtrasadoUsuarioUseCase>();
        
        _serviceCollection.TryAddScoped<INotificacaoDevolucaoEmprestimoAtrasoProlongadoUseCase, NotificacaoDevolucaoEmprestimoAtrasoProlongadoUseCase>();
        _serviceCollection.TryAddScoped<INotificacaoDevolucaoEmprestimoAtrasadoProlongadoUsuarioUseCase, NotificacaoDevolucaoEmprestimoAtrasadoProlongadoUsuarioUseCase>();
        
        _serviceCollection.TryAddScoped<IExecutarImportacaoArquivoAcervoBibliograficoUseCase, ExecutarImportacaoArquivoAcervoBibliograficoUseCase>();
        _serviceCollection.TryAddScoped<IExecutarImportacaoArquivoAcervoDocumentalUseCase, ExecutarImportacaoArquivoAcervoDocumentalUseCase>();
    }

    protected virtual void RegistrarRabbit()
    {
        _serviceCollection.AddOptions<ConfiguracaoRabbitOptions>()
            .Bind(_configuration.GetSection(ConfiguracaoRabbitOptions.Secao), c => c.BindNonPublicProperties = true);

        _serviceCollection.AddSingleton<ConfiguracaoRabbitOptions>();
        _serviceCollection.AddSingleton<IConexoesRabbit>(serviceProvider =>
        {
            var options = serviceProvider.GetService<IOptions<ConfiguracaoRabbitOptions>>().Value;
            var provider = serviceProvider.GetService<IOptions<DefaultObjectPoolProvider>>().Value;
            return new ConexoesRabbitAcessos(options, provider);
        });

        _serviceCollection.AddSingleton<IServicoLogs, ServicoLogs>();
    }
    
    protected virtual void ConfigurarMensageria()
    {
        _serviceCollection.ConfigurarMensageria(_configuration);
    }
    
    protected virtual void RegistrarServicoArmazenamento(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigurarArmazenamento(configuration);
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
            config.AddMap(new CreditoAutorMap());
            config.AddMap(new EditoraMap());
            config.AddMap(new AssuntoMap());
            config.AddMap(new SerieColecaoMap());
            config.AddMap(new AcervoMap());
            config.AddMap(new AcervoFotograficoMap());
            config.AddMap(new AcervoCreditoAutorMap());
            config.AddMap(new AcervoFotograficoArquivoMap());
            config.AddMap(new ArquivoMap());
            config.AddMap(new AcervoArteGraficaMap());
            config.AddMap(new AcervoArteGraficaArquivoMap());
            config.AddMap(new AcervoTridimensionalMap());
            config.AddMap(new AcervoTridimensionalArquivoMap());
            config.AddMap(new AcervoAudiovisualMap());
            config.AddMap(new AcervoDocumentalMap());
            config.AddMap(new AcervoDocumentalArquivoMap());
            config.AddMap(new AcervoDocumentalAcessoDocumentoMap());
            config.AddMap(new AcervoBibliograficoMap());
            config.AddMap(new AcervoBibliograficoAssuntoMap());
            config.AddMap(new ImportacaoArquivoMap());
            config.AddMap(new AcervoSolicitacaoMap());
            config.AddMap(new AcervoSolicitacaoItemMap());
            config.AddMap(new ParametroSistemaMap());
            config.AddMap(new EventoMap());
            config.AddMap(new EventoFixoMap());
            config.AddMap(new AcervoEmprestimoMap());
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
        _serviceCollection.TryAddScoped<IRepositorioCreditoAutor, RepositorioCreditoAutor>();
        _serviceCollection.TryAddScoped<IRepositorioEditora, RepositorioEditora>();
        _serviceCollection.TryAddScoped<IRepositorioAssunto, RepositorioAssunto>();
        _serviceCollection.TryAddScoped<IRepositorioSerieColecao, RepositorioSerieColecao>();
        _serviceCollection.TryAddScoped<IRepositorioAcervo, RepositorioAcervo>();
        _serviceCollection.TryAddScoped<IRepositorioAcervoCreditoAutor, RepositorioAcervoCreditoAutor>();
        _serviceCollection.TryAddScoped<IRepositorioAcervoFotografico, RepositorioAcervoFotografico>();
        _serviceCollection.TryAddScoped<IRepositorioAcervoFotograficoArquivo, RepositorioAcervoFotograficoArquivo>();
        _serviceCollection.TryAddScoped<IRepositorioArquivo, RepositorioArquivo>();
        _serviceCollection.TryAddScoped<IRepositorioAcervoArteGrafica, RepositorioAcervoArteGrafica>();
        _serviceCollection.TryAddScoped<IRepositorioAcervoArteGraficaArquivo, RepositorioAcervoArteGraficaArquivo>();
        _serviceCollection.TryAddScoped<IRepositorioAcervoTridimensional, RepositorioAcervoTridimensional>();
        _serviceCollection.TryAddScoped<IRepositorioAcervoTridimensionalArquivo, RepositorioAcervoTridimensionalArquivo>();
        _serviceCollection.TryAddScoped<IRepositorioAcervoAudiovisual, RepositorioAcervoAudiovisual>();
        _serviceCollection.TryAddScoped<IRepositorioAcervoDocumental, RepositorioAcervoDocumental>();
        _serviceCollection.TryAddScoped<IRepositorioAcervoDocumentalArquivo, RepositorioAcervoDocumentalArquivo>();
        _serviceCollection.TryAddScoped<IRepositorioAcervoDocumentalAcessoDocumento, RepositorioAcervoDocumentalAcessoDocumento>();
        _serviceCollection.TryAddScoped<IRepositorioAcervoBibliografico, RepositorioAcervoBibliografico>();
        _serviceCollection.TryAddScoped<IRepositorioAcervoBibliograficoAssunto, RepositorioAcervoBibliograficoAssunto>();
        _serviceCollection.TryAddScoped<IRepositorioImportacaoArquivo, RepositorioImportacaoArquivo>();
        _serviceCollection.TryAddScoped<IRepositorioParametroSistema, RepositorioParametroSistema>();
        _serviceCollection.TryAddScoped<IRepositorioAcervoSolicitacao, RepositorioAcervoSolicitacao>();
        _serviceCollection.TryAddScoped<IRepositorioAcervoSolicitacaoItem, RepositorioAcervoSolicitacaoItem>();
        _serviceCollection.TryAddScoped<IRepositorioEvento, RepositorioEvento>();
        _serviceCollection.TryAddScoped<IRepositorioEventoFixo, RepositorioEventoFixo>();
        _serviceCollection.TryAddScoped<IRepositorioAcervoEmprestimo, RepositorioAcervoEmprestimo>();
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
        _serviceCollection.TryAddScoped<IServicoIdioma, ServicoIdioma>();
        _serviceCollection.TryAddScoped<IServicoMaterial, ServicoMaterial>();
        _serviceCollection.TryAddScoped<IServicoSuporte, ServicoSuporte>();
        _serviceCollection.TryAddScoped<IServicoCEP, ServicoCEP>();
        _serviceCollection.TryAddScoped<IServicoCreditoAutor, ServicoCreditoAutor>();
        _serviceCollection.TryAddScoped<IServicoEditora, ServicoEditora>();
        _serviceCollection.TryAddScoped<IServicoAssunto, ServicoAssunto>();
        _serviceCollection.TryAddScoped<IServicoSerieColecao, ServicoSerieColecao>();
        _serviceCollection.TryAddScoped<IServicoMenu, ServicoMenu>();
        _serviceCollection.TryAddScoped<IServicoMenu, ServicoMenu>();
        _serviceCollection.TryAddScoped<IServicoAcervo, ServicoAcervoAuditavel>();
        _serviceCollection.TryAddScoped<IServicoAcervoFotografico, ServicoAcervoFotografico>();
        _serviceCollection.TryAddScoped<IServicoUploadArquivo, ServicoUploadArquivo>();
        _serviceCollection.TryAddScoped<IServicoExcluirArquivo, ServicoExcluirArquivo>();
        _serviceCollection.TryAddScoped<IServicoArmazenamentoArquivoFisico, ServicoArmazenamentoArquivoFisico>();
        _serviceCollection.TryAddScoped<IServicoArmazenamento, ServicoArmazenamento>();
        _serviceCollection.TryAddScoped<IServicoDownloadArquivo, ServicoDownloadArquivo>();
        _serviceCollection.TryAddScoped<IServicoMoverArquivoTemporario, ServicoMoverArquivoTemporario>();
        _serviceCollection.TryAddScoped<IServicoGerarMiniatura, ServicoGerarMiniatura>();
        _serviceCollection.TryAddScoped<IServicoMensageriaLogs, ServicoMensageriaLogs>();
        _serviceCollection.TryAddScoped<IServicoAcervoArteGrafica, ServicoAcervoArteGrafica>();
        _serviceCollection.TryAddScoped<IServicoAcervoTridimensional, ServicoAcervoTridimensional>();
        _serviceCollection.TryAddScoped<IServicoAcervoAudiovisual, ServicoAcervoAudiovisual>();
        _serviceCollection.TryAddScoped<IServicoAcervoDocumental, ServicoAcervoDocumental>();
        _serviceCollection.TryAddScoped<IServicoAcervoBibliografico, ServicoAcervoBibliografico>();
        _serviceCollection.TryAddScoped<IServicoImportacaoArquivoBase, ServicoImportacaoArquivoBase>();
        _serviceCollection.TryAddScoped<IServicoImportacaoArquivoAcervoBibliografico, ServicoImportacaoArquivoAcervoBibliografico>();
        _serviceCollection.TryAddScoped<IServicoImportacaoArquivoAcervoDocumental, ServicoImportacaoArquivoAcervoDocumental>();
        _serviceCollection.TryAddScoped<IServicoImportacaoArquivoAcervoArteGrafica, ServicoImportacaoArquivoAcervoArteGrafica>();
        _serviceCollection.TryAddScoped<IServicoImportacaoArquivoAcervoAudiovisual, ServicoImportacaoArquivoAcervoAudiovisual>();
        _serviceCollection.TryAddScoped<IServicoImportacaoArquivoAcervoFotografico, ServicoImportacaoArquivoAcervoFotografico>();
        _serviceCollection.TryAddScoped<IServicoImportacaoArquivoAcervoTridimensional, ServicoImportacaoArquivoAcervoTridimensional>();
        _serviceCollection.TryAddScoped<IServicoImportacaoArquivoAcervo, ServicoImportacaoArquivoAcervo>();
        _serviceCollection.TryAddScoped<IServicoAcervoSolicitacao, ServicoAcervoSolicitacao>();
        _serviceCollection.TryAddScoped<IServicoEvento, ServicoEvento>();
        _serviceCollection.TryAddScoped<IServicoMensageria, ServicoMensageria>();
        _serviceCollection.TryAddScoped<IServicoNotificacaoEmail, ServicoNotificacaoEmail>();
        _serviceCollection.TryAddScoped<IServicoAcervoEmprestimo, ServicoAcervoEmprestimo>();
    }
    protected virtual void RegistrarHttpClients()
    {
        _serviceCollection.AdicionarHttpClients(_configuration);
    }
}
