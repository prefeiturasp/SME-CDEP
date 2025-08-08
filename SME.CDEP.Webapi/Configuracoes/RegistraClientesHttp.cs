namespace SME.CDEP.Webapi.Configuracoes
{
    public static class RegistraClientesHttp
    {
        public static void Registrar(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient(name: "apiSR", c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiServidorRelatorios").Value);
                c.DefaultRequestHeaders.Add("x-sr-api-key", configuration.GetSection("ApiKeySr").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.Timeout = Timeout.InfiniteTimeSpan;
            });
        }
    }
}
