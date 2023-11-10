﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[Route("api/v1/acervo/fotografico/importacao/planilha")]
[ValidaDto]
public class ImportacaoArquivoAcervoFotograficoController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ImportacaoArquivoRetornoDTO<AcervoFotograficoLinhaRetornoDTO>),200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Authorize("Bearer")] 
    public async Task<IActionResult> ImportarArquivo(IFormFile file,[FromServices] IServicoImportacaoArquivoAcervoFotografico servicoImportacaoArquivoAcervoFotografico)
    {
        return Ok(await servicoImportacaoArquivoAcervoFotografico.ImportarArquivo(file));
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(ImportacaoArquivoRetornoDTO<AcervoFotograficoLinhaRetornoDTO>),200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Authorize("Bearer")] 
    public async Task<IActionResult> ObterImportacaoPendente([FromServices] IServicoImportacaoArquivoAcervoFotografico servicoImportacaoArquivoAcervoFotografico)
    {
        return Ok(await servicoImportacaoArquivoAcervoFotografico.ObterImportacaoPendente());
    }
    
    [HttpPut("{Id}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Policy = "Bearer")]
    public async Task<IActionResult> RemoverLinhaDoArquivo([FromRoute] long id, [FromBody] int linhaDoArquivo, [FromServices] IServicoImportacaoArquivoAcervoFotografico servicoImportacaoArquivoAcervoFotografico)
    {
        return Ok(await servicoImportacaoArquivoAcervoFotografico.RemoverLinhaDoArquivo(id, linhaDoArquivo));
    }
    
    [HttpPut("atualizar-linha/{id}/sucesso")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Policy = "Bearer")]
    public async Task<IActionResult> AtualizarLinhaParaSucesso([FromRoute] long id, [FromBody] int linhaDoArquivo, [FromServices] IServicoImportacaoArquivoAcervoFotografico servicoImportacaoArquivoAcervoFotografico)
    {
        return Ok(await servicoImportacaoArquivoAcervoFotografico.AtualizarLinhaParaSucesso(id,linhaDoArquivo));
    }
}