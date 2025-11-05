using System.Data;

namespace SME.CDEP.Infra.Dados;

public interface ITransacao
{
    IDbTransaction Iniciar();
    IDbTransaction Iniciar(IsolationLevel il);
}
