namespace SME.CDEP.Dominio.Contexto;

public interface IContextoAplicacao
{   
    IDictionary<string, object> Variaveis { get; set; }
    string LoginUsuario { get; }
    string NomeUsuario { get; }
    string PerfilUsuario { get; }
    T ObterVariavel<T>(string nome);
    IContextoAplicacao AtribuirContexto(IContextoAplicacao contexto);
    void AdicionarVariaveis(IDictionary<string, object> variaveis);
}