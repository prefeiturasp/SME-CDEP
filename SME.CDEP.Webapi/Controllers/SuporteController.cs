using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class SuporteController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(IdNomeTipoExcluidoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Authorize("Bearer")]
    public async Task<IActionResult> Inserir([FromBody] NomeTipoDTO suporte, [FromServices] IServicoSuporte servicoSuporte)
    {
        return Ok(await servicoSuporte.Inserir(new IdNomeTipoExcluidoDTO() { Nome = suporte.Nome, Tipo = suporte.Tipo}));
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(IdNomeTipoExcluidoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Authorize("Bearer")]
    public async Task<IActionResult> Alterar([FromBody] IdNomeTipoDTO suporte, [FromServices] IServicoSuporte servicoSuporte)
    {
        return Ok(await servicoSuporte.Alterar(new IdNomeTipoExcluidoDTO() {Id = suporte.Id, Nome = suporte.Nome, Tipo = suporte.Tipo}));
    }

    [HttpGet]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeTipoExcluidoDTO), 200)]  
    [Authorize("Bearer")]
    public async Task<IActionResult> ObterTodos(TipoSuporte tipoSuporte, [FromServices]IServicoSuporte servicoSuporte)
    {
        var suportes = await servicoSuporte.ObterTodos();
        return Ok(tipoSuporte == TipoSuporte.NAO_DEFINIDO ? suportes : suportes.Where(w=> w.Tipo == (int)tipoSuporte));

    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeTipoExcluidoDTO), 200)]  
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