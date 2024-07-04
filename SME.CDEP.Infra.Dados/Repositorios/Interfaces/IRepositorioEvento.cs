using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioEvento : IRepositorioBaseAuditavel<Evento>
    {
        Task<bool> ExisteFeriadoOuSuspensaoNoDia(DateTime data, long? id);
        Task<IEnumerable<Evento>> ObterEventosTagPorData(DateTime data);
        Task<IEnumerable<Evento>> ObterEventosTagPorMesAno(int mes, int ano, long[] tiposAcervosPermitidos);
        Task<IEnumerable<EventoDetalhe>> ObterDetalhesDoDiaPorData(DateTime data, long[] tiposAcervosPermitidos);
        Task<Evento> ObterPorAtendimentoItemId(long atendimentoItemId);
        Task<IEnumerable<DateTime>> ObterEventosDeFeriadoESuspensaoPorDatas(DateTime[] toArray);
    }
}