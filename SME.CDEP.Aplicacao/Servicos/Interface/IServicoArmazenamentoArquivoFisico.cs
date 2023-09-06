using Microsoft.AspNetCore.Http;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoArmazenamentoArquivoFisico
    {
        Task<string> Armazenar(IFormFile formFile, string codigo, TipoArquivo tipoArquivo);
    }
}
