using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> Inserir([FromBody] NomeTipoDTO creditoAutor, [FromServices] IServicoCreditoAutor servicoCreditoAutor)
    {
        return Ok(await servicoCreditoAutor.Inserir(new IdNomeTipoExcluidoAuditavelDTO() { Nome = creditoAutor.Nome, Tipo = creditoAutor.Tipo}));
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(IdNomeTipoExcluidoAuditavelDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CRD_A, Policy = "Bearer")]
    public async Task<IActionResult> Alterar([FromBody] IdNomeTipoDTO creditoAutor, [FromServices] IServicoCreditoAutor servicoCreditoAutor)
    {
        return Ok(await servicoCreditoAutor.Alterar(new IdNomeTipoExcluidoAuditavelDTO() {Id = creditoAutor.Id, Nome = creditoAutor.Nome, Tipo = creditoAutor.Tipo}));
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginacaoResultadoDTO<IdNomeExcluidoAuditavelDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CRD_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterTodosOuPorNome([FromQuery] NomeTipoCreditoAutoriaDTO nomeTipoDto,[FromServices]IServicoCreditoAutor servicoCreditoAutor)
    {
        return Ok(await servicoCreditoAutor.ObterPaginado(nomeTipoDto));
    }
    
    [HttpGet("resumido")]
    [ProducesResponseType(typeof(IdNomeDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CRD_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterTodos([FromServices]IServicoCreditoAutor servicoCreditoAutor)
    {
        return Ok((await servicoCreditoAutor.ObterTodos()).Select(s=> new IdNomeDTO {Id = s.Id, Nome = s.Nome}));
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