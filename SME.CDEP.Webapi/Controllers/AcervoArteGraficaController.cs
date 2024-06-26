﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class AcervoArteGraficaController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(AcervoArteGraficaCadastroDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_I, Policy = "Bearer")]
    public async Task<IActionResult> Inserir([FromBody] AcervoArteGraficaCadastroDTO acervoArteGrafica, [FromServices] IServicoAcervoArteGrafica servicoAcervoArteGrafica)
    {
        return Ok(await servicoAcervoArteGrafica.Inserir(acervoArteGrafica));
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(AcervoArteGraficaDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_A, Policy = "Bearer")]
    public async Task<IActionResult> Alterar([FromBody] AcervoArteGraficaAlteracaoDTO acervoArteGrafica, [FromServices] IServicoAcervoArteGrafica servicoAcervoArteGrafica)
    {
        return Ok(await servicoAcervoArteGrafica.Alterar(acervoArteGrafica));
    }
    
    [HttpGet("{acervoId}")]
    [ProducesResponseType(typeof(AcervoArteGraficaDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterPorId([FromRoute] long acervoId,[FromServices] IServicoAcervoArteGrafica servicoAcervoArteGrafica)
    {
        return Ok(await servicoAcervoArteGrafica.ObterPorId(acervoId));
    }
    
    [HttpDelete("{acervoId}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_E, Policy = "Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long acervoId, [FromServices] IServicoAcervoArteGrafica servicoAcervoArteGrafica)
    {
        return Ok(await servicoAcervoArteGrafica.Excluir(acervoId));
    }
}