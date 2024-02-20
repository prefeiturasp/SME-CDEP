using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoEvento : IServicoAplicacao
    {
        Task<long> Inserir(EventoCadastroDTO eventoCadastroDto);
        Task<IEnumerable<EventoDTO>> ObterTodos();
        Task<EventoDTO> Alterar(EventoCadastroDTO eventoCadastroDto);
        Task<EventoDTO> ObterPorId(long eventoId);
        Task<IEnumerable<EventoTagDTO>> ObterEventosTagPorData(DiaMesDTO diaMesDto);
    }
}
