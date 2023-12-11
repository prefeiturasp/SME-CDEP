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
public class ArmazenamentoController: BaseController
{
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Authorize("Bearer")]
    public async Task<IActionResult> UploadTemp(IFormFile file, [FromServices] IServicoUploadArquivo servicoUploadArquivo)
    {
        return Ok(await servicoUploadArquivo.Upload(file));
    }
    
    [HttpPost("Converter")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    // [Authorize("Bearer")]
    public async Task<IActionResult> ConverterTiffToJpeg(Guid codigoArquivo, [FromServices] IServicoDownloadArquivo servicoDownloadArquivo)
    {
        return Ok(await servicoDownloadArquivo.Converter(codigoArquivo));
    }
    
    [HttpGet("{codigoArquivo}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Authorize("Bearer")]
    public async Task<IActionResult> Download(Guid codigoArquivo, [FromServices] IServicoDownloadArquivo servicoDownloadArquivo)
    {
        var (arquivo, contentType, nomeArquivo) = await servicoDownloadArquivo.Download(codigoArquivo);
        if (arquivo.EhNulo()) return NoContent();

        return File(arquivo, contentType, nomeArquivo);
    }
    
    [HttpPost("upload/tipo-arquivo")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Authorize("Bearer")]
    public async Task<IActionResult> UploadPorTipo(IFormFile file, TipoArquivo tipoArquivo,[FromServices] IServicoUploadArquivo servicoUploadArquivo)
    {
        return Ok(await servicoUploadArquivo.Upload(file,tipoArquivo));
    }
    
    [HttpGet("download/tipo-acervo")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Authorize("Bearer")]
    public async Task<IActionResult> DownloadPorTipoAcervo(TipoAcervo tipoAcervo, [FromServices] IServicoDownloadArquivo servicoDownloadArquivo)
    {
        var (arquivo, contentType, nomeArquivo) = await servicoDownloadArquivo.DownloadPorTipoAcervo(tipoAcervo);
        if (arquivo.EhNulo()) return NoContent();

        return File(arquivo, contentType, nomeArquivo);
    }
}