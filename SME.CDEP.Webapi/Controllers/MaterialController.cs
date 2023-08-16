﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class MaterialController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(IdNomeTipoExcluidoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Authorize("Bearer")]
    public async Task<IActionResult> Inserir([FromBody] NomeTipoDTO material, [FromServices] IServicoMaterial servicoMaterial)
    {
        return Ok(await servicoMaterial.Inserir(new IdNomeTipoExcluidoDTO() { Nome = material.Nome, Tipo = material.Tipo}));
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(IdNomeTipoExcluidoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Authorize("Bearer")]
    public async Task<IActionResult> Alterar([FromBody] IdNomeTipoDTO material, [FromServices] IServicoMaterial servicoMaterial)
    {
        return Ok(await servicoMaterial.Alterar(new IdNomeTipoExcluidoDTO() {Id = material.Id, Nome = material.Nome, Tipo = material.Tipo}));
    }

    [HttpGet]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeTipoExcluidoDTO), 200)]  
    [Authorize("Bearer")]
    public async Task<IActionResult> ObterTodos([FromServices]IServicoMaterial servicoMaterial)
    {
        return Ok(await servicoMaterial.ObterTodos());
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeTipoExcluidoDTO), 200)]  
    [Authorize("Bearer")]
    public async Task<IActionResult> ObterTodos([FromRoute] long id,[FromServices]IServicoMaterial servicoMaterial)
    {
        return Ok(await servicoMaterial.ObterPorId(id));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(bool), 200)]
    [Authorize("Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long id, [FromServices] IServicoMaterial servicoMaterial)
    {
        return Ok(await servicoMaterial.Excluir(id));
    }
}