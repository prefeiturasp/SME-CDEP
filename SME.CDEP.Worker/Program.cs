using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Infra.Servicos.Options;
using SME.CDEP.IoC;
using SME.CDEP.IoC.Extensions;
using SME.CDEP.Worker;
using SME.CDEP.Worker.Contexto;


var builder = WebApplication.CreateBuilder(args);
var registradorDeDependencia = new RegistradorDeDependencia(builder.Services, builder.Configuration);
registradorDeDependencia.Registrar();

var serviceProvider = builder.Services.BuildServiceProvider();
var options = serviceProvider.GetService<IOptions<ConfiguracaoRabbitOptions>>().Value;

builder.Services.AddSingleton<IConnectionFactory>(serviceProvider =>
{
    var factory = new ConnectionFactory
    {
        HostName = options.HostName,
        UserName = options.UserName,
        Password = options.Password,
        VirtualHost = options.VirtualHost,
        RequestedHeartbeat = System.TimeSpan.FromSeconds(options.TempoHeartBeat),
    };

    return factory;

});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IContextoAplicacao, ContextoHttp>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddHostedService<WorkerRabbitMQ>();
builder.Services.AddHealthChecks();
builder.Services.AddSingleton(registradorDeDependencia);

var app = builder.Build();
RegistrarConfigsThreads.Registrar(builder.Configuration);

app.UseElasticApm(builder.Configuration,
    new SqlClientDiagnosticSubscriber(),
    new HttpDiagnosticsSubscriber());

app.Run(async (context) => { await context.Response.WriteAsync("WorkerRabbit CDEP"); });
app.Run();