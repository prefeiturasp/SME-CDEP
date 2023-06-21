using SME.CDEP.Dominio.Contexto;

namespace SME.CDEP.Dominio.Contexto;

public abstract class ContextoBase : IContextoAplicacao
{
    protected ContextoBase()
    {
        Variaveis = new Dictionary<string, object>();
    }

    public string LoginUsuario => ObterVariavel<string>("UsuarioLogin") ?? "Sistema";
    public string NomeUsuario => ObterVariavel<string>("UsuarioNome") ?? "Sistema";
    public string PerfilUsuario => ObterVariavel<string>("PerfilUsuario") ?? string.Empty;
    public IDictionary<string, object> Variaveis { get; set; }
    public abstract void AdicionarVariaveis(IDictionary<string, object> variaveis);
    public abstract IContextoAplicacao AtribuirContexto(IContextoAplicacao contexto);

    public T ObterVariavel<T>(string nome)
    {

        if (Variaveis.TryGetValue(nome, out object valor))
            return (T)valor;

        return default;
    }
}