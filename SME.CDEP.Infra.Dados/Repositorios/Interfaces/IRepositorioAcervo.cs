using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervo : IRepositorioBaseAuditavel<Acervo>
    {
        Task<IEnumerable<Acervo>> PesquisarPorFiltro(int? tipoAcervo, string titulo, long? creditoAutorId, string codigo);
        Task<bool> ExisteCodigo(string codigo, long id, TipoAcervo tipo);
        Task<IEnumerable<PesquisaAcervo>> ObterPorTextoLivreETipoAcervo(string? textoLivre, TipoAcervo? tipoAcervo, int? anoInicial, int? anoFinal);
        Task<IEnumerable<AcervoSolicitacaoItemCompleto>> ObterAcervosSolicitacoesItensCompletoPorId(long acervoSolicitacaoId, long[] tiposAcervosPermitidos);
        Task<IEnumerable<ArquivoCodigoNomeAcervoId>> ObterArquivosPorAcervoId(long[] acervosIds);
        Task<Acervo> PesquisarAcervoPorCodigoTombo(string codigoTombo, long[] tiposAcervosPermitidos);
    }
}