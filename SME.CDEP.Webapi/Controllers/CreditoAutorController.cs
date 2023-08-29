﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
[Authorize("Bearer")]
public class CreditoAutorController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(IdNomeTipoExcluidoAuditavelDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CRD_I, Policy = "Bearer")]
    public async Task<IActionResult> Inserir([FromBody] NomeDTO credito, [FromServices] IServicoCreditoAutor servicoCreditoAutor)
    {
        return Ok(await servicoCreditoAutor.Inserir(new IdNomeTipoExcluidoAuditavelDTO() { Nome = credito.Nome}));
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(IdNomeTipoExcluidoAuditavelDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CRD_A, Policy = "Bearer")]
    public async Task<IActionResult> Alterar([FromBody] IdNomeDTO credito, [FromServices] IServicoCreditoAutor servicoCreditoAutor)
    {
        return Ok(await servicoCreditoAutor.Alterar(new IdNomeTipoExcluidoAuditavelDTO() {Id = credito.Id, Nome = credito.Nome}));
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginacaoResultadoDTO<IdNomeExcluidoAuditavelDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CRD_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterTodosOuPorNome([FromQuery] string? nome,[FromServices]IServicoCreditoAutor servicoCreditoAutor)
    {
        return Ok(await servicoCreditoAutor.ObterPaginado(nome));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IdNomeTipoExcluidoAuditavelDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CRD_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterPorId([FromRoute] long id,[FromServices]IServicoCreditoAutor servicoCreditoAutor)
    {
        return Ok(await servicoCreditoAutor.ObterPorId(id));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CRD_E, Policy = "Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long id, [FromServices] IServicoCreditoAutor servicoCreditoAutor)
    {
        return Ok(await servicoCreditoAutor.Excluir(id));
    }
}