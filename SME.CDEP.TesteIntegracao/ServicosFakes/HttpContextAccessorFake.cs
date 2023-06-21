using Microsoft.AspNetCore.Http;

namespace SME.CDEP.TesteIntegracao.ServicosFakes
{
    public class HttpContextAccessorFake : IHttpContextAccessor
    {
        public HttpContext HttpContext { get; set; }

        public HttpContextAccessorFake()
        {
            HttpContext = new DefaultHttpContext();
        }
    }
}