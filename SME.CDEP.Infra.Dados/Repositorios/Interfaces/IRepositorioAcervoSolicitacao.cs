using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoSolicitacao : IRepositorioBaseAuditavel<AcervoSolicitacao>
    {
        Task<IEnumerable<AcervoTipoTituloAcervoIdCreditosAutores>> ObterItensDoAcervoPorAcervosIds(long[] acervosIds);
        Task Excluir(long acervoSolicitacaoId);
    }
}