using System.Data;

namespace SME.CDEP.Infra.Dados;

public interface ICdepConexao : IDisposable
{
    void Abrir();
    void Fechar();
    IDbConnection Obter();
}
