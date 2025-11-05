using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoUploadArquivo
    {
        Task<ArquivoArmazenadoDTO> Upload(IFormFile formFile, TipoArquivo tipoArquivo = TipoArquivo.Temp);
    }
}
