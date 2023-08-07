using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class AcessoDocumentoController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(IdNomeExcluidoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Authorize("Bearer")]
    public async Task<IActionResult> CadastrarAlterar([FromBody] IdNomeExcluidoDTO acessoDocumentoExcluidoDto, [FromServices] IServicoAcessoDocumento servicoAcessoDocumento)
    {
        return acessoDocumentoExcluidoDto.Id > 0 ? Ok(await servicoAcessoDocumento.Alterar(acessoDocumentoExcluidoDto)) : Ok(await servicoAcessoDocumento.Inserir(acessoDocumentoExcluidoDto));
    }

    [HttpGet]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeExcluidoDTO), 200)]  
    [Authorize("Bearer")]
    public async Task<IActionResult> ObterTodos([FromServices]IServicoAcessoDocumento servicoAcessoDocumento)
    {
        return Ok(await servicoAcessoDocumento.ObterTodos());
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeExcluidoDTO), 200)]  
    [Authorize("Bearer")]
    public async Task<IActionResult> ObterTodos([FromRoute] long id,[FromServices]IServicoAcessoDocumento servicoAcessoDocumento)
    {
        return Ok(await servicoAcessoDocumento.ObterPorId(id));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(bool), 200)]
    [Authorize("Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long id, [FromServices] IServicoAcessoDocumento servicoAcessoDocumento)
    {
        return Ok(await servicoAcessoDocumento.Excluir(id));
    }
}