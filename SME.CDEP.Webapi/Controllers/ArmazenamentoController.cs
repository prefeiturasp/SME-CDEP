using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class ArmazenamentoController: BaseController
{
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Authorize("Bearer")]
    public async Task<IActionResult> Upload(IFormFile file, [FromServices] IServicoUploadArquivo servicoUploadArquivo)
    {
        return Ok(await servicoUploadArquivo.Upload(file));
    }
    
    [HttpDelete]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Authorize("Bearer")]
    public async Task<IActionResult> Excluir([FromBody] Guid[] codigos, [FromServices] IServicoExcluirArquivo servicoExcluirArquivo)
    {
        return Ok(await servicoExcluirArquivo.Excluir(codigos));
    }
    
    [HttpGet("{codigoArquivo}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Authorize("Bearer")]
    public async Task<IActionResult> Download(Guid codigoArquivo, [FromServices] IServicoDownloadArquivo servicoDownloadArquivo)
    {
        var (arquivo, contentType, nomeArquivo) = await servicoDownloadArquivo.Download(codigoArquivo);
        if (arquivo == null) return NoContent();

        return File(arquivo, contentType, nomeArquivo);
    }
}