using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoArmazenamentoArquivoFisico
    {
        Task<ArquivoArmazenadoDTO> Armazenar(IFormFile formFile, TipoArquivo tipoArquivo);
    }
}
