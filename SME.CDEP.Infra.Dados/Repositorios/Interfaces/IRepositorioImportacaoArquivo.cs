using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioImportacaoArquivo : IRepositorioBaseAuditavel<ImportacaoArquivo>
    {
        Task<ImportacaoArquivo> ObterUltimaImportacao(TipoAcervo tipoAcervo);
        Task<long> Salvar(ImportacaoArquivo importacaoArquivo);
    }
}