using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioArquivo : IRepositorioBase<Arquivo>
    {
        Task<Arquivo> ObterPorCodigo(Guid codigo);
        Task<IEnumerable<Arquivo>> ObterPorCodigos(Guid[] codigos);
        Task<IEnumerable<Arquivo>> ObterPorIds(long[] ids);
        Task<bool> ExcluirArquivoPorCodigo(Guid codigoArquivo);
        Task<bool> ExcluirArquivoPorId(long id);
        Task<long> ObterIdPorCodigo(Guid arquivoCodigo);
        Task<bool> ExcluirArquivosPorIds(long[] ids);
        Task<long> SalvarAsync(Arquivo arquivo);
        Task<IEnumerable<AcervoCodigoNomeResumido>> ObterAcervoCodigoNomeArquivoPorAcervoId(long[] acervosIds);
        Task<Arquivo> ObterArquivoPorNomeTipoArquivo(string nome, TipoArquivo tipoArquivo);
    }
}