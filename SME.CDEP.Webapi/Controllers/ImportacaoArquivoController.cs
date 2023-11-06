using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class ImportacaoArquivoController: BaseController
{
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    // [Authorize("Bearer")]
    public async Task<IActionResult> UploadPorTipoAcervo(IFormFile file, TipoAcervo tipoAcervo,[FromServices] IServicoImportacaoArquivo servicoImportacaoArquivo)
    {
        return Ok(await servicoImportacaoArquivo.ImportarArquivoPorTipoAcervo(file,tipoAcervo));
    }
}