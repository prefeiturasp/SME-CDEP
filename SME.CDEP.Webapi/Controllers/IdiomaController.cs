using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class IdiomaController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(IdiomaDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Authorize("Bearer")]
    public async Task<IActionResult> CadastrarAlterar([FromBody] IdiomaDTO acessoDocumentoDTO, [FromServices] IServicoIdioma servicoIdioma)
    {
        return acessoDocumentoDTO.Id > 0 ? Ok(await servicoIdioma.Alterar(acessoDocumentoDTO)) : Ok(await servicoIdioma.Inserir(acessoDocumentoDTO));
    }

    [HttpGet("obter-todos")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdiomaDTO), 200)]  
    [Authorize("Bearer")]
    public async Task<IActionResult> ObterTodos([FromServices]IServicoIdioma servicoIdioma)
    {
        return Ok(await servicoIdioma.ObterTodos());
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdiomaDTO), 200)]  
    [Authorize("Bearer")]
    public async Task<IActionResult> ObterTodos([FromRoute] long id,[FromServices]IServicoIdioma servicoIdioma)
    {
        return Ok(await servicoIdioma.ObterPorId(id));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(bool), 200)]
    [Authorize("Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long id, [FromServices] IServicoIdioma servicoIdioma)
    {
        return Ok(await servicoIdioma.Excluir(id));
    }
}