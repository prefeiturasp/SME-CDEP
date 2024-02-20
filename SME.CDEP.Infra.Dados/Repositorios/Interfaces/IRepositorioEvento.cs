using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioEvento : IRepositorioBaseAuditavel<Evento>
    {
        Task<bool> ExisteFeriadoOuSuspensaoNoDia(DateTime data, long? id);
        Task<IEnumerable<Evento>> ObterEventosTagPorData(DateTime data);
    }
}