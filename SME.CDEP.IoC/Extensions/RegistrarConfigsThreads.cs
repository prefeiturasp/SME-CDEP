﻿using Microsoft.Extensions.Configuration;
using SME.CDEP.Infra.Servicos.Utilitarios;

namespace SME.CDEP.IoC.Extensions;

public static class RegistrarConfigsThreads
{
    public static void Registrar(IConfiguration Configuration)
    {
        var threadPoolOptions = new ThreadPoolOptions();
        Configuration.GetSection(ThreadPoolOptions.Secao).Bind(threadPoolOptions, c => c.BindNonPublicProperties = true);
        if (threadPoolOptions.WorkerThreads > 0 && threadPoolOptions.CompletionPortThreads > 0)
            ThreadPool.SetMinThreads(threadPoolOptions.WorkerThreads, threadPoolOptions.CompletionPortThreads);
    }
}