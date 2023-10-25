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
public class AssuntoController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(IdNomeExcluidoAuditavelDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.ASS_I, Policy = "Bearer")]
    public async Task<IActionResult> Inserir([FromBody] NomeDTO assunto, [FromServices] IServicoAssunto servicoAssunto)
    {
        return Ok(await servicoAssunto.Inserir(new IdNomeExcluidoAuditavelDTO() { Nome = assunto.Nome}));
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(IdNomeExcluidoAuditavelDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.ASS_A, Policy = "Bearer")]
    public async Task<IActionResult> Alterar([FromBody] IdNomeDTO assunto, [FromServices] IServicoAssunto servicoAssunto)
    {
        return Ok(await servicoAssunto.Alterar(new IdNomeExcluidoAuditavelDTO() {Id = assunto.Id, Nome = assunto.Nome}));
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginacaoResultadoDTO<IdNomeExcluidoAuditavelDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.ASS_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterTodosOuPorNome([FromQuery] string? nome,[FromServices]IServicoAssunto servicoAssunto)
    {
        return Ok(await servicoAssunto.ObterPaginado(nome));
    }
    
    [HttpGet("resumido")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeDTO), 200)]  
    [Authorize("Bearer")]
    public async Task<IActionResult> ObterTodos([FromServices]IServicoAssunto servicoAssunto)
    {
        return Ok((await servicoAssunto.ObterTodos()).Select(s=> new IdNomeDTO() { Id = s.Id, Nome = s.Nome}));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IdNomeExcluidoAuditavelDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.ASS_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterPorId([FromRoute] long id,[FromServices]IServicoAssunto servicoAssunto)
    {
        return Ok(await servicoAssunto.ObterPorId(id));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.ASS_E, Policy = "Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long id, [FromServices] IServicoAssunto servicoAssunto)
    {
        return Ok(await servicoAssunto.Excluir(id));
    }
}