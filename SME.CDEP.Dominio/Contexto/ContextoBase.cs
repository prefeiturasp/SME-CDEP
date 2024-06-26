﻿using SME.CDEP.Dominio.Contexto;

namespace SME.CDEP.Dominio.Contexto;

public abstract class ContextoBase : IContextoAplicacao
{
    protected ContextoBase()
    {
        Variaveis = new Dictionary<string, object>();
    }

    public string NomeUsuario => ObterVariavel<string>("NomeUsuario") ?? "Sistema";
    public string UsuarioLogado => ObterVariavel<string>("UsuarioLogado") ?? "Sistema";
    public string PerfilUsuario => ObterVariavel<string>("PerfilUsuario") ?? string.Empty;
    public IDictionary<string, object> Variaveis { get; set; }
    public string Administrador => ObterVariavel<string>("Administrador") ?? string.Empty;
    public abstract void AdicionarVariaveis(IDictionary<string, object> variaveis);
    public abstract IContextoAplicacao AtribuirContexto(IContextoAplicacao contexto);

    public T ObterVariavel<T>(string nome)
    {

        if (Variaveis.TryGetValue(nome, out object valor))
            return (T)valor;

        return default;
    }
    
    public List<Tuple<string, string>> ObterVariavel(string nome)
    {
        if (Variaveis.TryGetValue(nome, out object valor))
            return (List<Tuple<string, string>>)valor;

        return default;
    }
}