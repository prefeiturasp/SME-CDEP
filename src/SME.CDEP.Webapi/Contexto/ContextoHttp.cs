using Microsoft.Extensions.Primitives;
using SME.CDEP.Dominio.Contexto;

namespace SME.CDEP.Webapi.Contexto;

public class ContextoHttp : ContextoBase
{
    readonly IHttpContextAccessor httpContextAccessor;

    public ContextoHttp(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;

        CapturarVariaveis();
    }

    private void CapturarVariaveis()
    {
        Variaveis.Add("RF", httpContextAccessor.HttpContext?.User?.FindFirst("login")?.Value ?? "0");
        Variaveis.Add("Claims", GetInternalClaim());
        Variaveis.Add("login", httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(a => a.Type == "login")?.Value ?? string.Empty);
        Variaveis.Add("UsuarioLogado", httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Sistema");
        Variaveis.Add("NomeUsuario", httpContextAccessor.HttpContext?.User?.FindFirst("nome")?.Value ?? "Sistema");
        Variaveis.Add("PerfilUsuario", ObterPerfilAtual() ?? string.Empty);
        Variaveis.Add("NumeroPagina", httpContextAccessor.HttpContext?.Request?.Query["NumeroPagina"].FirstOrDefault() ?? "0");
        Variaveis.Add("NumeroRegistros", httpContextAccessor.HttpContext?.Request?.Query["NumeroRegistros"].FirstOrDefault() ?? "0");
        Variaveis.Add("Ordenacao", httpContextAccessor.HttpContext?.Request?.Query["Ordenacao"].FirstOrDefault() ?? "0");

        var authorizationHeader = httpContextAccessor.HttpContext?.Request?.Headers.Authorization;

        if (authorizationHeader is null || !authorizationHeader.HasValue || authorizationHeader.Value == StringValues.Empty)
        {
            Variaveis.Add("TemAuthorizationHeader", false);
            Variaveis.Add("TokenAtual", string.Empty);
        }
        else
        {
            Variaveis.Add("TemAuthorizationHeader", true);
            Variaveis.Add("TokenAtual", authorizationHeader.Value.Single()!.Split(' ').Last());
        }
    }

    private List<Tuple<string, string>> GetInternalClaim()
    {
        return [.. (httpContextAccessor.HttpContext?.User?.Claims ?? default!).Select(x => new Tuple<string, string>(x.Type, x.Value))];
    }

    private string? ObterPerfilAtual()
    {
        return (httpContextAccessor.HttpContext?.User?.Claims ?? []).FirstOrDefault(x => x.Type.Equals("perfil", StringComparison.CurrentCultureIgnoreCase))?.Value;
    }

    public override IContextoAplicacao AtribuirContexto(IContextoAplicacao contexto)
    {
        throw new Exception("Este tipo de conexto não permite atribuição");
    }

    public override void AdicionarVariaveis(IDictionary<string, object> variaveis)
    {
        Variaveis = variaveis;
    }
}