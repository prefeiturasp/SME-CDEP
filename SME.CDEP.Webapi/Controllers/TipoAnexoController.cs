using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class TipoAnexoController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(TipoAnexoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Authorize("Bearer")]
    public async Task<IActionResult> CadastrarAlterar([FromBody] TipoAnexoDTO tipoAnexoDTO, [FromServices] IServicoTipoAnexo servicoTipoAnexo)
    {
        return tipoAnexoDTO.Id > 0 ? Ok(await servicoTipoAnexo.Alterar(tipoAnexoDTO)) : Ok(await servicoTipoAnexo.Inserir(tipoAnexoDTO));
    }

    [HttpGet("obter-todos")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(TipoAnexoDTO), 200)]  
    [Authorize("Bearer")]
    public async Task<IActionResult> ObterTodos([FromServices]IServicoTipoAnexo servicoTipoAnexo)
    {
        return Ok(await servicoTipoAnexo.ObterTodos());
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(TipoAnexoDTO), 200)]  
    [Authorize("Bearer")]
    public async Task<IActionResult> ObterTodos([FromRoute] long id,[FromServices]IServicoTipoAnexo servicoTipoAnexo)
    {
        return Ok(await servicoTipoAnexo.ObterPorId(id));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(bool), 200)]
    [Authorize("Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long id, [FromServices] IServicoTipoAnexo servicoTipoAnexo)
    {
        return Ok(await servicoTipoAnexo.Excluir(id));
    }
}