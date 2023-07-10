using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class SuporteController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(SuporteDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Authorize("Bearer")]
    public async Task<IActionResult> CadastrarAlterar([FromBody] SuporteDTO suporteDTO, [FromServices] IServicoSuporte servicoSuporte)
    {
        return suporteDTO.Id > 0 ? Ok(await servicoSuporte.Alterar(suporteDTO)) : Ok(await servicoSuporte.Inserir(suporteDTO));
    }

    [HttpGet("obter-todos")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(SuporteDTO), 200)]  
    [Authorize("Bearer")]
    public async Task<IActionResult> ObterTodos([FromServices]IServicoSuporte servicoSuporte)
    {
        return Ok(await servicoSuporte.ObterTodos());
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(SuporteDTO), 200)]  
    [Authorize("Bearer")]
    public async Task<IActionResult> ObterTodos([FromRoute] long id,[FromServices]IServicoSuporte servicoSuporte)
    {
        return Ok(await servicoSuporte.ObterPorId(id));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(bool), 200)]
    [Authorize("Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long id, [FromServices] IServicoSuporte servicoSuporte)
    {
        return Ok(await servicoSuporte.Excluir(id));
    }
}