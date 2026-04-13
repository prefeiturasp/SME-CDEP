using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace SME.CDEP.Infra.Dados;

[ExcludeFromCodeCoverage]
public class Transacao : ITransacao
{
    private readonly ICdepConexao _conexao;

    public Transacao(ICdepConexao conexao)
    {
        _conexao = conexao;
    }

    public IDbTransaction Iniciar()
    {
        return _conexao.Obter().BeginTransaction();
    }

    public IDbTransaction Iniciar(IsolationLevel il)
    {
        return _conexao.Obter().BeginTransaction(il);
    }
}
