using Microsoft.AspNetCore.Authorization;
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
    [ProducesResponseType(typeof(IdNomeExcluidoTipoDto), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Authorize("Bearer")]
    public async Task<IActionResult> CadastrarAlterar([FromBody] IdNomeExcluidoTipoDto materialDTO, [FromServices] IServicoMaterial servicoMaterial)
    {
        return materialDTO.Id > 0 ? Ok(await servicoMaterial.Alterar(materialDTO)) : Ok(await servicoMaterial.Inserir(materialDTO));
    }

    [HttpGet]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeExcluidoTipoDto), 200)]  
    [Authorize("Bearer")]
    public async Task<IActionResult> ObterTodos([FromServices]IServicoMaterial servicoMaterial)
    {
        return Ok(await servicoMaterial.ObterTodos());
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeExcluidoTipoDto), 200)]  
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