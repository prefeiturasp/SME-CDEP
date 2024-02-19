using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoEvento : IServicoAplicacao
    {
        Task<long> Inserir(EventoDTO eventoDto);
        Task<IEnumerable<EventoDTO>> ObterTodos();
        Task<EventoDTO> Alterar(EventoDTO eventoDto);
        Task<EventoDTO> ObterPorId(long eventoId);
    }
}
