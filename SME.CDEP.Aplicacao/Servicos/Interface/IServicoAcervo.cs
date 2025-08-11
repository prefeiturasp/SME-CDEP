using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervo : IServicoAplicacao,IServicoAcervoTipo,IServicoPesquisaAcervo, IServicoTipoAcervoPermitido
    {
        Task<long> Inserir(Acervo acervo);
        Task<IEnumerable<AcervoDTO>> ObterTodos();
        Task<AcervoDTO> Alterar(AcervoDTO acervoDTO);
        Task<AcervoDTO> AlterarCreditoAutor(Acervo acervo);
        Task<AcervoDTO> ObterPorId(long acervoId);
        Task<bool> Excluir(long entidaId);
        Task<PaginacaoResultadoDTO<AcervoTableRowDTO>> ObterPorFiltro(int? tipoAcervo, string titulo, long? creditoAutorId, string codigo);
        Task<AcervoDetalheDTO> ObterDetalhamentoPorTipoAcervoECodigo(FiltroDetalharAcervoDTO filtro);
        Task<string> ObterTermoDeCompromisso();
        Task<IdNomeCodigoTipoParaEmprestimoDTO> PesquisarAcervoPorCodigoTombo(FiltroCodigoTomboDTO filtroCodigoTomboDto);
        Task<string?> ObterImagemBase64(string nomeArquivo);
    }
}
