using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioUsuario : IRepositorioBaseAuditavel<Usuario>
    {
        Task<Usuario?> ObterPorLogin(string login);
    }
}