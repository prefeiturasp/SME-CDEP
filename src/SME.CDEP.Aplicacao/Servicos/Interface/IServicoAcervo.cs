using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervo : IServicoAplicacao,IServicoAcervoTipo,IServicoPesquisaAcervo, IServicoTipoAcervoPermitido
    {
        Task<long> Inserir(Acervo acervo);
        Task<IEnumerable<AcervoDto>> ObterTodos();
        Task<AcervoDto> Alterar(AcervoDto acervoDTO);
        Task<AcervoDto> AlterarCreditoAutor(Acervo acervo);
        Task<AcervoDto> ObterPorId(long acervoId);
        Task<bool> Excluir(long entidaId);
        Task<PaginacaoResultadoDTO<AcervoTableRowDTO>> ObterPorFiltro(int? tipoAcervo, string? titulo, long? creditoAutorId, string? codigo, int? idEditora);
        Task<AcervoDetalheDTO> ObterDetalhamentoPorTipoAcervoECodigo(FiltroDetalharAcervoDTO filtro);
        Task<string> ObterTermoDeCompromisso();
        Task<IdNomeCodigoTipoParaEmprestimoDTO> PesquisarAcervoPorCodigoTombo(FiltroCodigoTomboDTO filtroCodigoTomboDto);
        Task<string?> ObterImagemBase64(string nomeArquivo);
        Task<AcervoSolicitacaoRetornoCadastroDTO> ObterAcervosSolicitacoesPorIdTiposPermitidosAsync(long acervoSolicitacaoId, long[] tiposAcervosPermitidos);
    }
}
