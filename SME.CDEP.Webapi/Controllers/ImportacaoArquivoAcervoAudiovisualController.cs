﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[Route("api/v1/acervo/audiovisual/importacao/planilha")]
[ValidaDto]
public class ImportacaoArquivoAcervoAudiovisualController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ImportacaoArquivoRetornoDTO<AcervoAudiovisualLinhaRetornoDTO>),200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Authorize("Bearer")] 
    public async Task<IActionResult> ImportarArquivo(IFormFile file,[FromServices] IServicoImportacaoArquivoAcervoAudiovisual servicoImportacaoArquivoAcervoAudiovisual)
    {
        return Ok(await servicoImportacaoArquivoAcervoAudiovisual.ImportarArquivo(file));
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(ImportacaoArquivoRetornoDTO<AcervoAudiovisualLinhaRetornoDTO>),200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Authorize("Bearer")] 
    public async Task<IActionResult> ObterImportacaoPendente([FromServices] IServicoImportacaoArquivoAcervoAudiovisual servicoImportacaoArquivoAcervoAudiovisual)
    {
        return Ok(await servicoImportacaoArquivoAcervoAudiovisual.ObterImportacaoPendente());
    }
}