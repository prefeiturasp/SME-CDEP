using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcessoDocumento : IServicoAplicacao
    {
        Task<long> Inserir(AcessoDocumentoDTO acessoDocumentoDto);
        Task<IList<AcessoDocumentoDTO>> ObterTodos();
        Task<AcessoDocumentoDTO> Alterar(AcessoDocumentoDTO acessoDocumentoDto);
        Task<AcessoDocumentoDTO> ObterPorId(long acessoDocumentoId);
        Task<bool> Excluir(long acessoDocumentoId);
    }
}
