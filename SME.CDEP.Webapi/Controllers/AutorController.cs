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
public class AutorController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(IdNomeExcluidoAuditavelDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.AUT_I, Policy = "Bearer")]
    public async Task<IActionResult> Inserir([FromBody] NomeDTO autor, [FromServices] IServicoAutor servicoAutor)
    {
        return Ok(await servicoAutor.Inserir(new IdNomeExcluidoAuditavelDTO() { Nome = autor.Nome}));
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(IdNomeExcluidoAuditavelDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.AUT_A, Policy = "Bearer")]
    public async Task<IActionResult> Alterar([FromBody] IdNomeDTO autor, [FromServices] IServicoAutor servicoAutor)
    {
        return Ok(await servicoAutor.Alterar(new IdNomeExcluidoAuditavelDTO() {Id = autor.Id, Nome = autor.Nome}));
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginacaoResultadoDTO<IdNomeExcluidoAuditavelDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.AUT_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterTodosOuPorNome([FromQuery] string? nome,[FromServices]IServicoAutor servicoAutor)
    {
        return Ok(await servicoAutor.ObterPaginado(nome));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IdNomeExcluidoAuditavelDTO), 200)]  
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]  
    [Permissao(Permissao.AUT_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterPorId([FromRoute] long id,[FromServices]IServicoAutor servicoAutor)
    {
        return Ok(await servicoAutor.ObterPorId(id));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.AUT_E, Policy = "Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long id, [FromServices] IServicoAutor servicoAutor)
    {
        return Ok(await servicoAutor.Excluir(id));
    }
}