﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class AcervoAudiovisualController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(AcervoAudiovisualCadastroDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.ACR_I, Policy = "Bearer")]
    public async Task<IActionResult> Inserir([FromBody] AcervoAudiovisualCadastroDTO acervoAudiovisual, [FromServices] IServicoAcervoAudiovisual servicoAcervoAudiovisual)
    {
        return Ok(await servicoAcervoAudiovisual.Inserir(acervoAudiovisual));
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(AcervoAudiovisualDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.ACR_A, Policy = "Bearer")]
    public async Task<IActionResult> Alterar([FromBody] AcervoAudiovisualAlteracaoDTO acervoAudiovisual, [FromServices] IServicoAcervoAudiovisual servicoAcervoAudiovisual)
    {
        return Ok(await servicoAcervoAudiovisual.Alterar(acervoAudiovisual));
    }
    
    [HttpGet("{acervoId}")]
    [ProducesResponseType(typeof(AcervoAudiovisualDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.ACR_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterPorId([FromRoute] long acervoId,[FromServices] IServicoAcervoAudiovisual servicoAcervoAudiovisual)
    {
        return Ok(await servicoAcervoAudiovisual.ObterPorId(acervoId));
    }
    
    [HttpDelete("{acervoId}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.ACR_E, Policy = "Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long acervoId, [FromServices] IServicoAcervoAudiovisual servicoAcervoAudiovisual)
    {
        return Ok(await servicoAcervoAudiovisual.Excluir(acervoId));
    }
}