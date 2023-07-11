using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoTipoAnexo : IServicoAplicacao
    {
        Task<long> Inserir(TipoAnexoDTO tipoAnexoDTO);
        Task<IList<TipoAnexoDTO>> ObterTodos();
        Task<TipoAnexoDTO> Alterar(TipoAnexoDTO tipoAnexoDTO);
        Task<TipoAnexoDTO> ObterPorId(long tipoAnexoId);
        Task<bool> Excluir(long tipoAnexoId);
    }
}
