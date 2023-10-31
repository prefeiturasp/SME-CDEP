
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoDownloadArquivo
    {
        Task<(byte[], string, string)> Download(Guid codigoArquivo);
        Task<(byte[], string, string)> DownloadCsv(TipoAcervo tipoAcervo);
    }
}
