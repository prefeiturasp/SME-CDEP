using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoBase
    {
        Task<IEnumerable<Arquivo>> ObterArquivosPorIds(long[] ids);
        Task MoverArquivosTemporarios(TipoArquivo tipoAcervoFotografico);
        Task MoverArquivosTemporarios(TipoArquivo tipoAcervoFotografico, IEnumerable<Arquivo> arquivosAMover);
        Task ExcluirArquivosArmazenamento();
        Task<(IEnumerable<long>, IEnumerable<long>)> ObterArquivosInseridosExcluidosMovidos(long[] arquivosAlterados, long[] arquivosExistentes);
        Task<bool> Excluir(long id);
    }
}
